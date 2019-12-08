using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day8
    {
        public static async Task SolveAsync()
        {
            const int Width = 25;
            const int Height = 6;
            int[] input = File.ReadAllText("day8.txt").Select(c => c - '0').ToArray();
            int numLayers = input.Length / Width / Height;
            int[][][] layers = new int[numLayers][][];
            int index = 0;
            for (int layer = 0; layer < numLayers; layer++)
            {
                layers[layer] = new int[Height][];
                for (int y = 0; y < Height; y++)
                {
                    layers[layer][y] = new int[Width];
                    for (int x = 0; x < Width; x++)
                    {
                        layers[layer][y][x] = input[index++];
                    }
                }
            }

            int[][] best = layers.OrderBy(l => l.Sum(r => r.Count(p => p == 0))).First();
            Console.WriteLine(best.Sum(r => r.Count(p => p == 1)) *
                              best.Sum(r => r.Count(p => p == 2)));

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int val = Enumerable.Range(0, numLayers).Select(i => layers[i][y][x]).First(p => p != 2);
                    Console.Write(val == 0 ? ' ' : '#');
                }

                Console.WriteLine();
            }
        }
    }
}
