using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	internal static class Day14
	{
		public static int Part1(string[] input)
		{
			const int time = 2503;

			return input.Select(line => GetDistanceForDeer(line, time)).Max();
		}

		private static int GetDistanceForDeer(string line, int time)
		{
			var match = Regex.Match(line,
			                        @"(\S+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds");

			int speed = int.Parse(match.Groups[2].Value);
			int duration = int.Parse(match.Groups[3].Value);
			int rest = int.Parse(match.Groups[4].Value);
			int distance =
				Enumerable.Repeat(speed, duration)
				          .Concat(Enumerable.Repeat(0, rest))
				          .Repeat(time)
				          .Take(time)
				          .Sum();
			return distance;
		}

		public static int Part2(string[] input)
		{
			const int time = 2503;

			Dictionary<int, int> points = new Dictionary<int, int>();
			for (int i = 1; i <= time; i++)
			{
				var distances = input.Select((line, j) =>
				                             {
					                             int distance = GetDistanceForDeer(line, i);

					                             return new {Index = j, Distance = distance};
				                             }).ToList();

				int maxDistance = distances.Max(d2 => d2.Distance);
				foreach (int deer in distances.Where(d => d.Distance == maxDistance).Select(d => d.Index))
				{
					if (!points.ContainsKey(deer))
						points[deer] = 0;

					points[deer]++;
				}
			}

			return points.Values.Max();
		}
	}
}