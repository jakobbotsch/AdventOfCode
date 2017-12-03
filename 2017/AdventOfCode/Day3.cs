using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    internal static class Day3
    {
        public static void Solve1(int input)
        {
            int x = 1, y = 0, dx = 1, dy = 0;

            for (int i = 0; ; i++)
            {
                if (2 + i == input)
                {
                    Console.WriteLine("{0}", Math.Abs(x) + Math.Abs(y));
                    return;
                }

                if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
                {
                    int tmp = dx;
                    dx = -dy;
                    dy = tmp;
                }
                x += dx;
                y += dy;
            }
        }

        public static void Solve2(int input)
        {
            int x = 1, y = 0, dx = 1, dy = 0;

            Dictionary<(int x, int y), int> vals = new Dictionary<(int x, int y), int>();
            vals[(0, 0)] = 1;
            for (int i = 0; ; i++)
            {
                int val = 0;
                for (int ny = -1; ny <= 1; ny++)
                {
                    for (int nx = -1; nx <= 1; nx++)
                    {
                        if (vals.TryGetValue((x + nx, y + ny), out int v))
                            val += v;
                    }
                }

                if (val > input)
                {
                    Console.WriteLine(val);
                    return;
                }

                vals[(x, y)] = val;

                if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
                {
                    int tmp = dx;
                    dx = -dy;
                    dy = tmp;
                }
                x += dx;
                y += dy;
            }
        }
    }
}
