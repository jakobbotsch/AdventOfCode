using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day16
    {
        public static void Solve()
        {
            string input = File.ReadAllText("day16.txt");
            //string input = "69317163492948606335995924319873";
            int[] signal = input.Trim().Select(c => c - '0').ToArray();
            int[] part1 = signal;
            for (int i = 0; i < 100; i++)
            {
                part1 = FFT(part1);
            }

            Console.WriteLine(string.Concat(part1.Take(8)));
            int[] part2 = Enumerable.Repeat(signal, 10000).SelectMany(t => t).ToArray();
            for (int i = 0; i < 100; i++)
            {
                part2 = FFT(part2);
            }
            Console.WriteLine(string.Concat(part2.Skip(int.Parse(string.Concat(signal.Take(7)))).Take(8)));
        }

        private static int[] FFT(int[] input)
        {
            long[] cumulativeSum = new long[input.Length + 1];
            long totalSum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                cumulativeSum[i] = totalSum;
                totalSum += input[i];
            }

            cumulativeSum[^1] = totalSum;

            int[] newFft = new int[input.Length];
            for (int i = 0; i < newFft.Length; i++)
            {
                int runLength = i + 1;
                int runCoeff = 1;
                int runStart = i;
                long sum = 0;
                while (runStart < input.Length)
                {
                    int runEnd = Math.Min(input.Length, runStart + runLength);
                    sum += runCoeff * (cumulativeSum[runEnd] - cumulativeSum[runStart]);
                    runCoeff *= -1;
                    runStart += runLength * 2;
                }

                newFft[i] = (int)(Math.Abs(sum % 10));
            }
            return newFft;
        }
    }
}
