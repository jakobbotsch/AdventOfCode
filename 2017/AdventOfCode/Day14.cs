using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    internal static class Day14
    {
        public static void Solve(string input)
        {
            bool[,] grid = new bool[128, 128];

            int count = 0;
            for (int y = 0; y < 128; y++)
            {
                byte[] bytes = Day10.KnotHash($"{input}-{y}");
                for (int x = 0; x < bytes.Length; x++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if ((bytes[x] & (1 << (7 - i))) == 0)
                            continue;

                        count++;
                        grid[x * 8 + i, y] = true;
                    }
                }
            }

            Console.WriteLine(count);

            (int, int)[] neis = { (1, 0), (-1, 0), (0, 1), (0, -1) };
            HashSet<(int, int)> seen = new HashSet<(int, int)>();
            int numRegs = 0;
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    if (!grid[x, y] || !seen.Add((x, y)))
                        continue;

                    numRegs++;
                    Queue<(int, int)> queue = new Queue<(int, int)>();
                    queue.Enqueue((x, y));
                    while (queue.Count > 0)
                    {
                        var (cx, cy) = queue.Dequeue();
                        foreach (var (ox, oy) in neis)
                        {
                            int nx = cx + ox;
                            int ny = cy + oy;
                            if (nx < 0 || nx >= 128 || ny < 0 || ny >= 128)
                                continue;

                            if (!grid[nx, ny] || !seen.Add((nx, ny)))
                                continue;

                            queue.Enqueue((nx, ny));
                        }
                    }
                }
            }

            Console.WriteLine(numRegs);
        }
    }
}
