using System.Collections.Generic;
using System.Linq;

namespace VKBot.Extensions
{
	public static class StringExtensions
	{
		public static Dictionary<char, double> GetStatsOfLetters(this string text)
		{
			var countOfLetters = new Dictionary<char, int>();
			foreach (var symbol in text)
			{
				if (char.IsLetter(symbol))
				{
					if (!countOfLetters.ContainsKey(symbol))
					{
						countOfLetters[symbol] = 0;
					}
					countOfLetters[symbol]++;
				}
			}

			var totalLetterCount = countOfLetters.Sum(x => x.Value);
			return countOfLetters.ToDictionary(x => x.Key, x => (double)x.Value / totalLetterCount);
		}
	}
}