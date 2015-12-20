using System;
using System.Collections.Generic;

namespace AdventOfCode
{
	internal static class Day20
	{
		public static int Part1()
		{
			const int input = 34000000;
			int sieveSize = 1000;

			while (true)
			{
				int[] sieve = new int[sieveSize];
				for (int i = 1; i < sieve.Length; i++)
				{
					for (int j = i; j < sieve.Length; j += i)
					{
						sieve[j - 1] += i*10;
					}
				}

				for (int i = 0; i < sieve.Length; i++)
				{
					if (sieve[i] >= input)
						return i + 1;
				}

				sieveSize *= 2;
			}
		}

		public static int Part2()
		{
			const int input = 34000000;
			int sieveSize = 1000;

			while (true)
			{
				int[] sieve = new int[sieveSize];
				for (int i = 1; i < sieve.Length; i++)
				{
					int delivered = 0;
					for (int j = i; j < sieve.Length; j += i)
					{
						sieve[j - 1] += i*11;
						delivered++;
						if (delivered == 50)
							break;
					}
				}

				for (int i = 0; i < sieve.Length; i++)
				{
					if (sieve[i] >= input)
						return i + 1;
				}

				sieveSize *= 2;
			}
		}
	}
}