using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day11
    {
        public static void Solve(string input)
        {
            int curX = 0;
            int curY = 0;

            int max = 0;

            foreach (string dir in input.Split(','))
            {
                Move(dir, ref curX, ref curY);
                max = Math.Max(max, DistanceToOrigin(curX, curY));
            }

            Console.WriteLine(DistanceToOrigin(curX, curY));
            Console.WriteLine(max);

            void Move(string dir, ref int x, ref int y)
            {
                switch (dir)
                {
                    case "n": y--; break;
                    case "ne": x++; y--; break;
                    case "se": x++; break;
                    case "s": y++; break;
                    case "sw": x--; y++; break;
                    case "nw": x--; break;
                }
            }

            int DistanceToOrigin(int x, int y) => (Math.Abs(x) + Math.Abs(x + y) + Math.Abs(y)) / 2;
        }
    }
}
