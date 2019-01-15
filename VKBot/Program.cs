using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VKBot
{
	class Program
	{
		static void Main(string[] args)
		{
			var api = new VkApi();
			api.Authorize(new ApiAuthParams
			{
				ApplicationId = 6820217,
				Login = "",
				Password = "",
				Settings = Settings.Wall
			});

			var posts = api.Wall.Get(new WallGetParams());
			var res = api.Groups.Get(new GroupsGetParams());

			Console.WriteLine(res.TotalCount);

			Console.ReadLine();
		}
	}
}
