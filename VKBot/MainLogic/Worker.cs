using System;
using System.Deployment.Application;
using System.Linq;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VKBot.Extensions;

namespace VKBot.MainLogic
{
	public class Worker
	{
		private readonly IVkApi _vkApi;
		private readonly int _postCount;

		public Worker(IVkApi vkApi, int postCount = 5)
		{
			_vkApi = vkApi;
			_postCount = postCount;
		}

		public void Execute(string id)
		{
			try
			{
				var wallGetParams = GetWalParams(id);
				var posts = _vkApi.Wall.Get(wallGetParams);
				EnsurePostCount(posts.WallPosts.Count);
				var totalText = string.Join("", posts.WallPosts.Select(x => x.Text));
				var stats = JsonConvert.SerializeObject(totalText.GetStatsOfLetters());
				//_vkApi.Utils.ResolveScreenName()
				Console.WriteLine($"Count of found posts: {posts.WallPosts.Count}");
				if (posts.WallPosts.Count > 0)
				{
					Console.WriteLine($"{id}, статистика для последних {_postCount} постов: {stats}");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private void EnsurePostCount(int postCount)
		{
			if (postCount < _postCount)
			{
				throw new Exception($"Count of found posts ({postCount}) less then {_postCount}");
			}
		}

		private WallGetParams GetWalParams(string id)
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