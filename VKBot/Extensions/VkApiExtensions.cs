using System;
using System.Collections.ObjectModel;
using VkNet.Abstractions;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace VKBot.Extensions
{
	public static class VkApiExtensions
	{
		public static ReadOnlyCollection<Post> GetPosts(this IVkApi vkApi, string id, int postCount)
		{
			var wallGetParams = new WallGetParams { Count = (ulong)postCount };
			if (long.TryParse(id, out var ownerId))
			{
				wallGetParams.OwnerId = ownerId;
			}
			else
			{
				var vkObj = vkApi.Utils.ResolveScreenName(id);
				wallGetParams.OwnerId = vkObj?.Id ?? throw new Exception("Invalid screen name");
			}

			return vkApi.Wall.Get(wallGetParams).WallPosts;
		}
	}
}