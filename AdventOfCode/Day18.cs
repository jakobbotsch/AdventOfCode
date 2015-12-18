using System;
using System.Linq;
using Combinatorics.Collections;

namespace AdventOfCode
{
	internal static class Day18
	{
		public static int Part1(string[] input)
		{
			bool[,] lights = new bool[102, 102];
			for (int y = 0; y < 100; y++)
			{
				for (int x = 0; x < 100; x++)
					lights[x + 1, y + 1] = input[y][x] == '#';
			}

			bool[,] next = new bool[102, 102];
			for (int i = 0; i < 100; i++)
			{
				for (int y = 0; y < 100; y++)
				{
					for (int x = 0; x < 100; x++)
					{
						int numNeighbors = CountNeighbors(lights, x, y);

						next[x + 1, y + 1] = lights[x + 1, y + 1] && (numNeighbors == 2 || numNeighbors == 3) || numNeighbors == 3;
					}
				}

				bool[,] temp = lights;
				lights = next;
				next = temp;
			}

			return lights.OfType<bool>().Count(b => b);
		}

		private static int CountNeighbors(bool[,] lights, int x, int y)
		{
			int numNeighbors = 0;
			for (int xo = -1; xo <= 1; xo++)
			{
				for (int yo = -1; yo <= 1; yo++)
				{
					if (xo == 0 && yo == 0)
						continue;

					if (lights[x + 1 + xo, y + 1 + yo])
						numNeighbors++;
				}
			}
			return numNeighbors;
		}

		public static int Part2(string[] input)
		{
			bool[,] lights = new bool[102, 102];
			for (int y = 0; y < 100; y++)
			{
				for (int x = 0; x < 100; x++)
					lights[x + 1, y + 1] = input[y][x] == '#';
			}

			Action<bool[,]> setCorners = arr =>
			                             {
				                             arr[1, 1] = true;
				                             arr[100, 1] = true;
				                             arr[1, 100] = true;
				                             arr[100, 100] = true;
			                             };

			bool[,] next = new bool[102, 102];
			for (int i = 0; i < 100; i++)
			{
				setCorners(lights);
				for (int y = 0; y < 100; y++)
				{
					for (int x = 0; x < 100; x++)
					{
						int numNeighbors = CountNeighbors(lights, x, y);

						next[x + 1, y + 1] = lights[x + 1, y + 1] && (numNeighbors == 2 || numNeighbors == 3) || numNeighbors == 3;
					}
				}

				bool[,] temp = lights;
				lights = next;
				next = temp;
			}

			setCorners(lights);
			return lights.OfType<bool>().Count(b => b);
		}

	}
}