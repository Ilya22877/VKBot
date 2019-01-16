using CommandLine;

namespace VKBot.Models
{
	public class Options
	{
		[Option('l', "login", HelpText = "E-mail or phone number for vk authorization", Required = true)]
		public string Login { get; set; }
		[Option('p', "password", HelpText = "Password for vk authorization", Required = true)]
		public string Password { get; set; }
	}
}