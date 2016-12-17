using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCSharp
{
    internal static class Day17
    {
        internal static void Solve(string input)
        {
            Queue<(int, int, string)> queue = new Queue<(int, int, string)>();
            queue.Enqueue((0, 0, ""));

            (int, int, string)[] neis = { (0, -1, "U"), (0, 1, "D"), (-1, 0, "L"), (1, 0, "R") };
            using (var md5 = MD5.Create())
            {
                bool shortest = false;
                int longest = 0;
                while (queue.Count > 0)
                {
                    var (x, y, steps) = queue.Dequeue();
                    if (x == 3 && y == 3)
                    {
                        if (!shortest)
                        {
                            Console.WriteLine("Shortest: {0}", steps);
                            shortest = true;
                        }

                        longest = steps.Length;
                        continue;
                    }

                    string hash = string.Concat(md5.ComputeHash(Encoding.ASCII.GetBytes(input + steps)).Select(b => b.ToString("x2")));
                    for (int i = 0; i < neis.Length; i++)
                    {
                        var (xo, yo, dir) = neis[i];
                        var (nx, ny) = (x + xo, y + yo);
                        if (nx < 0 || nx >= 4 || ny < 0 || ny >= 4)
                            continue;

                        bool open = hash[i] >= 'b' && hash[i] <= 'f';
                        if (!open)
                            continue;

                        queue.Enqueue((nx, ny, steps + dir));
                    }
                }

                Console.WriteLine("Longest: {0}", longest);
            }
        }
    }
}
