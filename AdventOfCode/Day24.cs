using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;

namespace AdventOfCode
{
	internal static class Day24
	{
		public static long Part1(string[] lines)
		{
			List<int> packages = lines.Select(int.Parse).ToList();
			return FindMinQE(packages, 3);
		}

		public static long Part2(string[] lines)
		{
			List<int> packages = lines.Select(int.Parse).ToList();
			return FindMinQE(packages, 4);
		}

		private static long FindMinQE(List<int> packages, int partitions)
		{
			int target = packages.Sum()/partitions;
			long best = long.MaxValue;
			for (int i = 1; i <= packages.Count; i++)
			{
				Console.WriteLine(i);
				foreach (IList<int> comb in new Combinations<int>(packages, i))
				{
					int sum = comb.Sum();
					if (sum != target)
						continue;

					if (HasPartition(packages.Except(comb).ToList(), target, partitions - 1))
					{
						long mul = 1;
						foreach (int j in comb)
							mul = mul*j;
						best = Math.Min(best, mul);
					}
				}

				if (best != long.MaxValue)
					break;
			}

			return best;
		}

		private static bool HasPartition(List<int> packages, int target, int partitionsLeft)
		{
			for (int i = 1; i <= packages.Count; i++)
			{
				foreach (IList<int> comb in new Combinations<int>(packages, i))
				{
					if (comb.Sum() != target)
						continue;

					if (partitionsLeft == 2)
						return true;

					return HasPartition(packages.Except(comb).ToList(), target, partitionsLeft - 1);
				}
			}

			return false;
		}
	}
}