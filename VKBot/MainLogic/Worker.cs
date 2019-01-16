using System;
using System.Deployment.Application;
using System.Linq;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace VKBot.MainLogic
{
	public class Worker
	{
		private readonly IVkApi _vkApi;

		public Worker(IVkApi vkApi)
		{
			_vkApi = vkApi;
		}

		public void Execute(string id)
		{
			try
			{
				var wallGetParams = GetWalParams(id);
				var posts = _vkApi.Wall.Get(wallGetParams);
				//Utilities.GetNullableLongId(new VkResponse())
				//var wallGetParams = 0;37841547  ; id73326429
				//var posts = _vkApi.Wall.Get(new WallGetParams { OwnerId = 37841547, Count = 5 });
				//var res = _vkApi.Groups.Get(new GroupsGetParams());

				Console.WriteLine($"Count of found posts: {posts.WallPosts.Count}");
				if (posts.WallPosts.Count > 0)
				{
					Console.WriteLine($"First post: {posts.WallPosts.First().Text}");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private static WallGetParams GetWalParams(string id)
		{
			var wallGetParams = new WallGetParams {Count = 5};
			if (long.TryParse(id, out var ownerId))
			{
				wallGetParams.OwnerId = ownerId;
			}
			else
			{
				//todo добавить проверку domain
				wallGetParams.Domain = id;
			}

			return wallGetParams;
		}
	}
}