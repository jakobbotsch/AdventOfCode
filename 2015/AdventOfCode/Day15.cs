using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
	internal static class Day15
	{
		public static long Part1()
		{
			long max = long.MinValue;
			for (int x = 0; x <= 100; x++)
			{
				int remainingY = 100 - x;
				for (int y = 0; y <= remainingY; y++)
				{
					int remainingZ = 100 - x - y;
					for (int z = 0; z <= remainingZ; z++)
					{
						int w = 100 - x - y - z;
						if (x + y + z + w != 100)
							continue;

						var score = ComputeScore(x, y, w, z);

						max = Math.Max(max, score);
					}
				}
			}

			return max;
		}

		private static long ComputeScore(int sprinkles, int peanutButter, int frosting, int sugar)
		{
			long capacity = sprinkles*5 + peanutButter*-1 + frosting*-1;
			long durability = sprinkles*-1 + peanutButter*3 + sugar*-1;
			long flavor = sugar*4;
			long texture = frosting*2;

			long score = Math.Max(0, capacity)*Math.Max(0, durability)*Math.Max(0, flavor)*
			             Math.Max(0, texture);
			return score;
		}

		public static long Part2()
		{
			long max = long.MinValue;
			for (int x = 0; x <= 100; x++)
			{
				int remainingY = 100 - x;
				for (int y = 0; y <= remainingY; y++)
				{
					int remainingZ = 100 - x - y;
					for (int z = 0; z <= remainingZ; z++)
					{
						int w = 100 - x - y - z;
						if (x + y + z + w != 100)
							continue;

						long calories = x*5 + y*1 + z*6 + w*8;
						if (calories != 500)
							continue;

						long score = ComputeScore(x, y, z, w);

						max = Math.Max(max, score);
					}
				}
			}

			return max;
		}
	}
}