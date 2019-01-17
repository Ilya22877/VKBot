using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using VKBot.Extensions;

namespace VKBot.tests.Extensions
{
	public class StringExtensionsTests
	{
		[TestCase("", ExpectedResult = "{}")]
		[TestCase("a", ExpectedResult = "{\"a\":1.0}")]
		[TestCase("ab", ExpectedResult = "{\"a\":0.5,\"b\":0.5}")]
		[TestCase("ba", ExpectedResult = "{\"a\":0.5,\"b\":0.5}")]
		[TestCase("aaad", ExpectedResult = "{\"a\":0.75,\"d\":0.25}")]
		[TestCase("aad", ExpectedResult = "{\"a\":0.66667,\"d\":0.33333}")]
		[TestCase("абвгд", ExpectedResult = "{\"а\":0.2,\"б\":0.2,\"в\":0.2,\"г\":0.2,\"д\":0.2}")]
		[TestCase("32()&&% ", ExpectedResult = "{}")]
		[TestCase("a^b)cd3232 3", ExpectedResult = "{\"a\":0.25,\"b\":0.25,\"c\":0.25,\"d\":0.25}")]
		public string GetStatsOfLetters(string text)
		{
			var result = text.GetStatsOfLetters();

			if (result.Any())
			{
				result.Sum(x => x.Value).Should().BeInRange(0.9999, 1.0001);
			}
			return JsonConvert.SerializeObject(result);
		}
	}
}