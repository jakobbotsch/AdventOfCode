using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	internal static class Day13
	{
		internal static int Part1(string[] input)
		{
			var dict = Parse(input);

			int best = dict.Keys.Permutations()
				.Select(p =>
				        {
					        int change = 0;
					        for (int i = 0, j = p.Count - 1; i < p.Count; j = i++)
					        {
						        change += dict[p[j]][p[i]];
						        change += dict[p[i]][p[j]];
					        }

					        return change;
				        }).Max();
			return best;
		}

		private static Dictionary<string, Dictionary<string, int>> Parse(string[] input)
		{
			Dictionary<string, Dictionary<string, int>> dict =
				new Dictionary<string, Dictionary<string, int>>();

			foreach (string line in input)
			{
				Match match = Regex.Match(line,
				                          @"(\S+) would (gain|lose) (\S+) happiness units by sitting next to (\S+)\.");

				string p1 = match.Groups[1].Value;
				int happiness = int.Parse(match.Groups[3].Value);
				if (match.Groups[2].Value == "lose")
					happiness *= -1;

				string p2 = match.Groups[4].Value;

				if (!dict.ContainsKey(p1))
					dict[p1] = new Dictionary<string, int>();

				dict[p1][p2] = happiness;
			}

			return dict;
		}

		public static int Part2(string[] input)
		{
			var dict = Parse(input);
			dict["me"] = new Dictionary<string, int>();
			foreach (string other in dict.Keys.ToArray())
			{
				dict[other]["me"] = 0;
				dict["me"][other] = 0;
			}

			int best = dict.Keys.Permutations()
				.Select(p =>
				        {
					        int change = 0;
					        for (int i = 0, j = p.Count - 1; i < p.Count; j = i++)
					        {
						        change += dict[p[j]][p[i]];
						        change += dict[p[i]][p[j]];
					        }

					        return change;
				        }).Max();

			return best;
		}
	}
}