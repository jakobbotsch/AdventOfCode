using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	internal static class Day19
	{
		public static int Part1(string[] input)
		{
			string start = input.Last();

			HashSet<string> strings = new HashSet<string>();
			for (int i = 0; i < input.Length - 2; i++)
			{
				var match = Regex.Match(input[i], @"(?<from>\S+) => (?<to>\S+)");

				string from = match.Groups["from"].Value;
				string to = match.Groups["to"].Value;

				int index = 0;
				while ((index = start.IndexOf(from, index)) != -1)
				{
					string replaced = start.Remove(index) + to + start.Substring(index + from.Length);
					strings.Add(replaced);
					index++;
				}
			}

			return strings.Count;
		}

		public static int Part2(string[] input)
		{
			List<KeyValuePair<string, string>> replacements = new List<KeyValuePair<string, string>>();
			for (int i = 0; i < input.Length - 2; i++)
			{
				var match = Regex.Match(input[i], @"(?<from>\S+) => (?<to>\S+)");

				string from = match.Groups["from"].Value;
				string to = match.Groups["to"].Value;
				replacements.Add(new KeyValuePair<string, string>(to, from));
			}

			replacements.Sort((kvp1, kvp2) => kvp2.Key.Length.CompareTo(kvp1.Key.Length));

			int best = int.MaxValue;

			const int generations = 500;
			Random rand = new Random();
			for (int i = 0; i < generations; i++)
			{
				string str = input.Last();
				int steps = 0;
				int iterations = 0;
				while (str != "e")
				{
					var rep = replacements[rand.Next(replacements.Count)];
					int index = str.IndexOf(rep.Key);
					if (index == -1)
					{
						iterations++;
						if (iterations > 100)
							break;

						continue;
					}

					str = str.Remove(index) + rep.Value + str.Substring(index + rep.Key.Length);
					steps++;
					iterations = 0;
				}

				if (str == "e" && steps < best)
				{
					best = steps;
					Console.WriteLine("{0}", best);
				}
			}

			return best;
		}
	}
}