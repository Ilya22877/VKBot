using System;

namespace VKBot.Log
{
	public class ConsoleLog : ILog
	{
		public void Info(string message)
		{
			WriteLine(message, "Info");
		}

		public void Warn(string message)
		{
			WriteLine(message, "Warning", ConsoleColor.Yellow);
		}

		public void Error(string message)
		{
			WriteLine(message, "Error", ConsoleColor.Red);
		}

		private static void WriteLine(string message, string level, ConsoleColor color = ConsoleColor.White)
		{
			Console.ForegroundColor = color;
			Console.WriteLine($"{DateTime.Now:T} [{level}]	{message}");
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}