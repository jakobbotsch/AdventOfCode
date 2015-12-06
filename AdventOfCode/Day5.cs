using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode
{
	internal static class Day5
	{
		internal static int Part1(string[] lines)
		{
			int nice = 0;
			foreach (string s in lines)
			{
				int numVowels = s.Count(c => "aeiou".Contains(c));
				if (numVowels < 3)
					continue;

				Func<bool> hasConsecutive = () =>
				                            {
					                            for (int i = 0; i < s.Length - 1; i++)
					                            {
						                            if (s[i] == s[i + 1])
							                            return true;
					                            }

					                            return false;
				                            };

				if (!hasConsecutive())
					continue;

				if (new[] {"ab", "cd", "pq", "xy"}.Any(ss => s.Contains(ss)))
					continue;

				nice++;
			}

			return nice;
		}

		internal static int Part2(string[] lines)
		{
			int nice = 0;
			foreach (string s in lines)
			{
				if (IsNice2(s))
					nice++;
			}

			return nice;
		}

		private static bool IsNice2(string s)
		{
			Func<bool> hasPair = () =>
			                     {
				                     for (int i = 0; i < s.Length - 1; i++)
				                     {
					                     string pair = s.Substring(i, 2);
					                     if (s.IndexOf(pair, i + 2) != -1)
						                     return true;
				                     }

				                     return false;
			                     };

			if (!hasPair())
				return false;

			for (int i = 0; i < s.Length - 2; i++)
			{
				if (s[i] == s[i + 2])
					return true;
			}

			return false;
		}
	}
}