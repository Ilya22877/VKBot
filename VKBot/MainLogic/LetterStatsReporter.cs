using System;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using VKBot.Extensions;

namespace VKBot.MainLogic
{
	public class LetterStatsReporter
	{
		private readonly IVkApi _vkApi;
		private readonly int _postCount;

		public LetterStatsReporter(IVkApi vkApi, int postCount = 5)
		{
			_vkApi = vkApi;
			_postCount = postCount;
		}

		public void Execute(string id)
		{
			try
			{
				var posts = GetPosts(id);
				var stats = CountStats(posts);
				Report(id, stats);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private void Report(string id, string stats)
		{
			Console.WriteLine($"{id}, статистика для последних {_postCount} постов: {stats}");

		}

		private static string CountStats(ReadOnlyCollection<Post> posts)
		{
			var totalText = string.Join("", posts.Select(x => x.Text));
			return JsonConvert.SerializeObject(totalText.CountStatsOfLetters());
		}

		private ReadOnlyCollection<Post> GetPosts(string id)
		{
			var wallGetParams = CreateWallGetParams(id);
			var posts = _vkApi.Wall.Get(wallGetParams);
			EnsurePostCount(posts.WallPosts.Count);
			return posts.WallPosts;
		}

		private void EnsurePostCount(int postCount)
		{
			if (postCount < _postCount)
			{
				throw new Exception($"Count of found posts ({postCount}) less then {_postCount}");
			}
		}

		private WallGetParams CreateWallGetParams(string id)
		{
			var wallGetParams = new WallGetParams {Count = (ulong)_postCount };
			if (long.TryParse(id, out var ownerId))
			{
				wallGetParams.OwnerId = ownerId;
			}
			else
			{
				var vkObj = _vkApi.Utils.ResolveScreenName(id);
				wallGetParams.OwnerId = vkObj?.Id ?? throw new Exception("Invalid screen name");
			}

			return wallGetParams;
		}
	}
}