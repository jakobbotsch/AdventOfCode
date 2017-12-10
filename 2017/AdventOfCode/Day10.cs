using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    internal static class Day10
    {
        public static void Solve1(string input)
        {
            int[] list = Enumerable.Range(0, 256).ToArray();

            int curPos = 0;
            int skipSize = 0;
            foreach (int len in input.Split(',').Select(int.Parse))
            {
                Round(list, curPos, len);
                curPos += len + skipSize;
                skipSize++;
            }

            Console.WriteLine(list[0] * list[1]);
        }

        private static void Round(int[] list, int curPos, int len)
        {
            int end = curPos + len;
            for (int i = curPos, j = end - 1; j > i; i++, j--)
            {
                int v = list[i % list.Length];
                list[i % list.Length] = list[j % list.Length];
                list[j % list.Length] = v;
            }
        }

        public static void Solve2(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray();

            int curPos = 0;
            int skipSize = 0;
            int[] list = Enumerable.Range(0, 256).ToArray();
            for (int i = 0; i < 64; i++)
            {
                foreach (int len in bytes)
                {
                    Round(list, curPos, len);
                    curPos += len + skipSize;
                    skipSize++;
                }
            }

            List<byte> dense = new List<byte>();
            for (int i = 0; i < 16; i++)
                dense.Add((byte)list.Skip(i * 16).Take(16).Aggregate((cur, elem) => cur ^ elem));

            Console.WriteLine(BitConverter.ToString(dense.ToArray()).Replace("-", "").ToLowerInvariant());
        }
    }
}
