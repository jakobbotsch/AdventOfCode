using System;
using System.Linq;

namespace AdventOfCode
{
	internal static class Day2
	{
		internal static int Part1(string[] lines)
		{
			int feet = 0;
			foreach (string line in lines)
			{
				int[] dims = line.Split('x').Select(int.Parse).ToArray();
				int l = dims[0];
				int w = dims[1];
				int h = dims[2];

				feet += 2*l*w + 2*w*h + 2*h*l + Math.Min(l*w, Math.Min(w*h, h*l));
			}

			return feet;
		}

		internal static int Part2(string[] lines)
		{
			int feet = 0;
			foreach (string line in lines)
			{
				int[] dims = line.Split('x').Select(int.Parse).OrderBy(d => d).ToArray();
				int smallest = dims[0];
				int secondSmallest = dims[1];

				feet += smallest*2 + secondSmallest*2 + dims[0]*dims[1]*dims[2];
			}

			return feet;
		}
	}
}