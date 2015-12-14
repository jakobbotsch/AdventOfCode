using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using MoreLinq;

namespace AdventOfCode
{
	internal static class Day9
	{
		private static Dictionary<string, Dictionary<string, int>> Parse(string[] lines)
		{
			Dictionary<string, Dictionary<string, int>> weights = new Dictionary<string, Dictionary<string, int>>();

			foreach (string line in lines)
			{
				Match match = Regex.Match(line, @"(?<from>\S+) to (?<to>\S+) = (?<weight>\S+)");

				string from = match.Groups["from"].Value;
				string to = match.Groups["to"].Value;
				int weight = int.Parse(match.Groups["weight"].Value);

				if (!weights.ContainsKey(from))
					weights.Add(from, new Dictionary<string, int>());

				weights[from].Add(to, weight);

				if (!weights.ContainsKey(to))
					weights.Add(to, new Dictionary<string, int>());

				weights[to].Add(from, weight);
			}

			return weights;
		} 

		internal static int Part1(string[] lines)
		{
			var graph = Parse(lines);

			int best = int.MaxValue;
			foreach (var permut in graph.Keys.Permutations())
			{
				string prev = null;
				int cost = 0;
				foreach (var city in permut)
				{
					if (prev != null)
						cost += graph[prev][city];

					prev = city;
				}

				best = Math.Min(best, cost);
			}

			return best;
		}

		internal static int Part2(string[] lines)
		{
			var graph = Parse(lines);

			int best = int.MinValue;
			foreach (var permut in graph.Keys.Permutations())
			{
				string prev = null;
				int cost = 0;
				foreach (var city in permut)
				{
					if (prev != null)
						cost += graph[prev][city];

					prev = city;
				}

				best = Math.Max(best, cost);
			}

			return best;
		}
	}
}