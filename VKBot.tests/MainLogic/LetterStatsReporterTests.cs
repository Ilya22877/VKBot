using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VKBot.Log;
using VKBot.MainLogic;

namespace VKBot.tests.MainLogic
{
	public class LetterStatsReporterTests
	{
		private LetterStatsReporter _sut;
		private IVkApi _api;
		private IWallCategory _wall;
		private ILog _log;
		private readonly Fixture _fixture = new Fixture();
		private const int PostId = 123;
		private const string Id = "12345";

		[SetUp]
		public void SetUp()
		{
			_api = Mock.Of<IVkApi>();
			_wall = Mock.Of<IWallCategory>();
			Mock.Get(_api)
				.Setup(x => x.Wall)
				.Returns(() => _wall);
			Mock.Get(_wall)
				.Setup(x => x.Post(It.IsAny<WallPostParams>()))
				.Returns(() => PostId);
			var posts = new List<Post>
			{
				CreateSomePost(),
				CreateSomePost(),
				CreateSomePost(),
				CreateSomePost(),
				CreateSomePost()
			};
			Mock.Get(_wall)
				.Setup(x => x.Get(It.IsAny<WallGetParams>(), It.IsAny<bool>()))
				.Returns(() => new WallGetObject { WallPosts = new ReadOnlyCollection<Post>(posts) });
			CreateSomePost();
			_log = Mock.Of<ILog>();
			_sut = new LetterStatsReporter(_api, _log);
		}

		[Test]
		public void SuccessfulResponsesFromVK_PostMethodWasCalled()
		{
			_sut.Execute(Id);

			Mock.Get(_wall)
				.Verify(x => x.Get(It.IsAny<WallGetParams>(), It.IsAny<bool>()), Times.Once);
			Mock.Get(_wall)
				.Verify(x => x.Post(It.IsAny<WallPostParams>()), Times.Once);
			Mock.Get(_log)
				.Verify(x => x.Error(It.IsAny<string>()), Times.Never);
			Mock.Get(_log)
				.Verify(x => x.Info($"Post was created. Id: {PostId}"), Times.Once);
		}

		[Test]
		public void CountOfPostsLessThenNeed_ErrorInLog()
		{
			_sut = new LetterStatsReporter(_api, _log, 10);

			_sut.Execute(Id);

			Mock.Get(_wall)
				.Verify(x => x.Get(It.IsAny<WallGetParams>(), It.IsAny<bool>()), Times.Once);
			Mock.Get(_wall)
				.Verify(x => x.Post(It.IsAny<WallPostParams>()), Times.Never);
			Mock.Get(_log)
				.Verify(x => x.Error(It.IsAny<string>()), Times.Once);
		}

		[Test]
		public void ThrowAnException_ErrorInLog()
		{
			Mock.Get(_wall)
				.Setup(x => x.Get(It.IsAny<WallGetParams>(), It.IsAny<bool>()))
				.Returns(() => throw new Exception());

			_sut.Execute(Id);

			Mock.Get(_wall)
				.Verify(x => x.Get(It.IsAny<WallGetParams>(), It.IsAny<bool>()), Times.Once);
			Mock.Get(_wall)
				.Verify(x => x.Post(It.IsAny<WallPostParams>()), Times.Never);
			Mock.Get(_log)
				.Verify(x => x.Error(It.IsAny<string>()), Times.Once);
		}

		private Post CreateSomePost()
		{
			return _fixture.Build<Post>()
				.Without(x => x.CopyHistory)
				.Create();
		}
	}
}