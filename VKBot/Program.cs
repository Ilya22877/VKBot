using System;
using CommandLine;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VKBot.Log;
using VKBot.MainLogic;
using VKBot.Models;

namespace VKBot
{
	class Program
	{
		private static readonly ILog Log = new ConsoleLog();
		static void Main(string[] args)
		{
			try
			{
				var options = GetOptions(args);
				var reporter = GetReporter(options, Log);
				Log.Info("VKBot is started!");
				string id;
				while (!string.IsNullOrWhiteSpace(id = GetId()))
				{
					reporter.Execute(id);
				}
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
				Console.ReadLine();
			}
		}

		private static string GetId()
		{
			Console.WriteLine("Enter user or group id:");
			return Console.ReadLine();
		}

		private static LetterStatsReporter GetReporter(Options options, ILog log)
		{
			var api = new VkApi();
			api.Authorize(new ApiAuthParams
			{
				ApplicationId = 6820217,
				Login = options.Login,
				Password = options.Password,
				Settings = Settings.All
			});

			return new LetterStatsReporter(api, log);
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
