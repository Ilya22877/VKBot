using System;
using CommandLine;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VKBot.Models;

namespace VKBot
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var options = GetOptions(args);
				var worker = GetWorker(options);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			Console.ReadLine();
		}

		private static Worker GetWorker(Options options)
		{
			var api = new VkApi();
			api.Authorize(new ApiAuthParams
			{
				ApplicationId = 6820217,
				Login = options.Login,
				Password = options.Password,
				Settings = Settings.All
			});

			return new Worker(api);
		}

		private static Options GetOptions(string[] args)
		{
			Options result = null;
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(options => result = options)
				.WithNotParsed(errors => throw new Exception("Parsing command line error."));

			return result;
		}
	}
}
