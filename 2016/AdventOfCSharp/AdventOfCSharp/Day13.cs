using System;
using System.Collections.Generic;

namespace AdventOfCSharp
{
    internal static class Day13
    {
        internal static void Solve1(int input)
        {
            HashSet<(int, int)> seen = new HashSet<(int, int)>();
            Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
            queue.Enqueue((1, 1, 0));
            seen.Add((1, 1));

            bool IsClosed(int x, int y)
            {
                if (x < 0 || y < 0)
                    return true;
                int pos = x * x + 3 * x + 2 * x * y + y + y * y;
                pos += input;
                uint v = (uint)pos;
                v ^= v >> 16;
                v ^= v >> 8;
                v ^= v >> 4;
                v &= 0xf;
                return ((0x6996 >> (int)v) & 1) != 0;
            }

            (int, int)[] neighbors = { (-1, 0), (1, 0), (0, -1), (0, 1) };
            while (queue.Count > 0)
            {
                var (x, y, steps) = queue.Dequeue();
                if (x == 31 && y == 39)
                {
                    Console.WriteLine("Steps: {0}", steps);
                    break;
                }

                foreach (var (xo, yo) in neighbors)
                {
                    var (nx, ny) = (x + xo, y + yo);

                    if (IsClosed(nx, ny) || !seen.Add((nx, ny)))
                        continue;

                    queue.Enqueue((nx, ny, steps + 1));
                }
            }
        }

        internal static void Solve2(int input)
        {
            HashSet<(int, int)> seen = new HashSet<(int, int)>();
            Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
            queue.Enqueue((1, 1, 0));
            seen.Add((1, 1));

            bool IsClosed(int x, int y)
            {
                if (x < 0 || y < 0)
                    return true;
                int pos = x * x + 3 * x + 2 * x * y + y + y * y;
                pos += input;
                uint v = (uint)pos;
                v ^= v >> 16;
                v ^= v >> 8;
                v ^= v >> 4;
                v &= 0xf;
                return ((0x6996 >> (int)v) & 1) != 0;
            }

            (int, int)[] neighbors = { (-1, 0), (1, 0), (0, -1), (0, 1) };
            while (queue.Count > 0)
            {
                var (x, y, steps) = queue.Dequeue();

                if (steps == 50)
                    continue;

                foreach (var (xo, yo) in neighbors)
                {
                    var (nx, ny) = (x + xo, y + yo);

                    if (IsClosed(nx, ny) || !seen.Add((nx, ny)))
                        continue;

                    queue.Enqueue((nx, ny, steps + 1));
                }
            }

            Console.WriteLine("Seen: {0}", seen.Count);
        }
    }
}
