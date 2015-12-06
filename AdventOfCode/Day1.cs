using System;
using System.Linq;

namespace AdventOfCode
{
	internal static class Day1
	{
		internal static int Part1(string input)
		{
			return input.Select(c => c == '(' ? 1 : -1).Sum();
		}

		internal static int Part2(string input)
		{
			int floor = 0;
			for (int i = 0; i < input.Length; i++)
			{
				floor += input[i] == '(' ? 1 : -1;
				if (floor == -1)
					return i + 1;
			}

			throw new ArgumentException("No solution");
		}
	}
}