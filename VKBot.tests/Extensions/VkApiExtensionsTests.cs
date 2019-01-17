using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VKBot.Extensions;

namespace VKBot.tests.Extensions
{
	public class VkApiExtensionsTests
	{
		private IVkApi _api;
		private IWallCategory _wall;
		private IUtilsCategory _utilsCategory;

		[SetUp]
		public void SetUp()
		{
			_api = Mock.Of<IVkApi>();
			_wall = Mock.Of<IWallCategory>();
			_utilsCategory = Mock.Of<IUtilsCategory>();
			Mock.Get(_api)
				.Setup(x => x.Wall)
				.Returns(() => _wall);
			Mock.Get(_api)
				.Setup(x => x.Utils)
				.Returns(() => _utilsCategory);
			Mock.Get(_wall)
				.Setup(x => x.Get(It.IsAny<WallGetParams>(), It.IsAny<bool>()))
				.Returns(() => new WallGetObject { WallPosts = new ReadOnlyCollection<Post>(new List<Post>()) });
			Mock.Get(_utilsCategory)
				.Setup(x => x.ResolveScreenName(It.IsAny<string>()))
				.Returns(() => new VkObject { Id = 12345 });
		}

		[Test]
		public void GetPosts_digitalId_WallGetMethodWasCalled()
		{
			var result = _api.GetPosts("14564564", 1);

			result.Should().NotBeNull();
			Mock.Get(_wall)
				.Verify(
					x => x.Get(It.IsAny<WallGetParams>(), It.IsAny<bool>()),
					Times.Once);
		}

		[Test]
		public void GetPosts_valideScreenName_WallGetAndResolveScreenNameMethodsWereCalled()
		{
			var result = _api.GetPosts("someValideScreenName", 1);

			result.Should().NotBeNull();
			Mock.Get(_wall)
				.Verify(
					x => x.Get(It.IsAny<WallGetParams>(), It.IsAny<bool>()),
					Times.Once);
			Mock.Get(_utilsCategory)
				.Verify(
					x => x.ResolveScreenName(It.IsAny<string>()),
					Times.Once);
		}

		[Test]
		public void GetPosts_invalideScreenName_ThrowException()
		{
			Mock.Get(_utilsCategory)
				.Setup(x => x.ResolveScreenName(It.IsAny<string>()))
				.Returns(() => null);

			_api.Invoking(x => x.GetPosts("someInvalideScreenName", 1))
				.Should().Throw<Exception>()
				.WithMessage("Invalid screen name (id).");

			Mock.Get(_utilsCategory)
				.Setup(x => x.ResolveScreenName(It.IsAny<string>()))
				.Returns(() => new VkObject{Id = null});

			_api.Invoking(x => x.GetPosts("someInvalideScreenName", 1))
				.Should().Throw<Exception>()
				.WithMessage("Invalid screen name (id).");
		}
	}
}