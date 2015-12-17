using System.Linq;
using Combinatorics.Collections;

namespace AdventOfCode
{
	internal static class Day17
	{
		public static int Part1(string[] input)
		{
			int count = 0;
			for (int i = 1; i <= input.Length; i++)
			{
				count += new Combinations<string>(input, i).Count(comb => comb.Sum(int.Parse) == 150);
			}

			return count;
		}

		public static int Part2(string[] input)
		{
			for (int i = 1; i <= input.Length; i++)
			{
				int count = new Combinations<string>(input, i).Count(comb => comb.Sum(int.Parse) == 150);
				if (count > 0)
					return count;
			}

			return -1;
		}
	}
}