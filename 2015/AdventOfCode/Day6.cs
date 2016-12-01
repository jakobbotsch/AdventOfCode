using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	internal static class Day6
	{
		internal static int Part1(string[] lines)
		{
			bool[,] lights = new bool[1000, 1000];
			foreach (string line in lines)
			{
				Point min;
				Point max;
				ParseBounds(line, out min, out max);

				Func<bool, bool> action;
				if (line.StartsWith("turn on"))
					action = b => true;
				else if (line.StartsWith("turn off"))
					action = b => false;
				else
					action = b => !b;

				for (int y = min.Y; y <= max.Y; y++)
				{
					for (int x = min.X; x <= max.X; x++)
					{
						lights[x, y] = action(lights[x, y]);
					}
				}
			}

			return lights.OfType<bool>().Count(b => b);
		}

		internal static int Part2(string[] lines)
		{
			int[,] lights = new int[1000, 1000];
			foreach (string line in lines)
			{
				Point min;
				Point max;
				ParseBounds(line, out min, out max);

				Func<int, int> action;
				if (line.StartsWith("turn on"))
					action = i => i + 1;
				else if (line.StartsWith("turn off"))
					action = i => Math.Max(0, i - 1);
				else
					action = i => i + 2;

				for (int y = min.Y; y <= max.Y; y++)
				{
					for (int x = min.X; x <= max.X; x++)
					{
						lights[x, y] = action(lights[x, y]);
					}
				}
			}

			return lights.OfType<int>().Sum();
		}

		private static void ParseBounds(string s, out Point min, out Point max)
		{
			Match match = Regex.Match(s, "(?<minX>[0-9]+),(?<minY>[0-9]+) through (?<maxX>[0-9]+),(?<maxY>[0-9]+)");

			min = new Point(int.Parse(match.Groups["minX"].Value), int.Parse(match.Groups["minY"].Value));
			max = new Point(int.Parse(match.Groups["maxX"].Value), int.Parse(match.Groups["maxY"].Value));
		}
	}
}