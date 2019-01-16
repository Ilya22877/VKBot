using System;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VKBot
{
	public class Worker
	{
		private readonly IVkApi _vkApi;

		public Worker(IVkApi vkApi)
		{
			_vkApi = vkApi;
		}

		public void Execute()
		{
			//Utilities.GetNullableLongId(new VkResponse())
			//var wallGetParams = 0;37841547  ; id73326429
			//var posts = api.Wall.Get(new WallGetParams{Domain = "id37841547", Count = 5});
			var posts = _vkApi.Wall.Get(new WallGetParams { OwnerId = 37841547, Count = 5 });
			var res = _vkApi.Groups.Get(new GroupsGetParams());

			Console.WriteLine(res.TotalCount);
		}
	}
}