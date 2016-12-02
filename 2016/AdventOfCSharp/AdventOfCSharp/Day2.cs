using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCSharp
{
    internal static class Day2
    {
        internal static void Solve1(string input)
        {
            int[,] keypad = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };

            var (x, y) = (1, 1);
            foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None))
            {
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case 'U': y--; break;
                        case 'D': y++; break;
                        case 'L': x--; break;
                        case 'R': x++; break;
                    }

                    x = Clamp(0, 2, x);
                    y = Clamp(0, 2, y);
                }

                Console.Write(keypad[y, x]);
            }

            int Clamp(int val, int min, int max)
            {
                if (val < min)
                    return min;
                if (val > max)
                    return max;
                return val;
            }
        }

        internal static void Solve2(string input)
        {
            string[] keypad =
            {
                "       ",
                "   1   ",
                "  234  ",
                " 56789 ",
                "  ABC  ",
                "   D   ",
                "       ",
            };

            var (x, y) = (1, 3);
            foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None))
            {
                foreach (char c in line)
                {
                    var (nx, ny) = (x, y);
                    switch (c)
                    {
                        case 'U': ny--; break;
                        case 'D': ny++; break;
                        case 'L': nx--; break;
                        case 'R': nx++; break;
                    }

                    if (keypad[ny][nx] != ' ')
                        (x, y) = (nx, ny);
                }

                Console.Write(keypad[y][x]);
            }
        }
    }
}
