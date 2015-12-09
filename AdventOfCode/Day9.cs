using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace AdventOfCode
{
	internal static class Day9
	{
		public static IEnumerable<T[]> Permutations<T>(T[] values, int fromInd = 0)
		{
			if (fromInd + 1 == values.Length)
				yield return values;
			else
			{
				foreach (var v in Permutations(values, fromInd + 1))
					yield return v;

				for (var i = fromInd + 1; i < values.Length; i++)
				{
					SwapValues(values, fromInd, i);
					foreach (var v in Permutations(values, fromInd + 1))
						yield return v;
					SwapValues(values, fromInd, i);
				}
			}
		}

		private static void SwapValues<T>(T[] values, int pos1, int pos2)
		{
			if (pos1 != pos2)
			{
				T tmp = values[pos1];
				values[pos1] = values[pos2];
				values[pos2] = tmp;
			}
		}

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
			foreach (var permut in Permutations(graph.Keys.ToArray()))
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
			foreach (var permut in Permutations(graph.Keys.ToArray()))
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