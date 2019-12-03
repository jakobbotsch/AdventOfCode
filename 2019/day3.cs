using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode
{
    internal static class Day3
    {
        public static void Solve()
        {
            string[] paths = File.ReadAllLines("day3.txt");
            Dictionary<(int, int), Dictionary<int, int>> map = new Dictionary<(int, int), Dictionary<int, int>>();
            for (int wire = 0; wire < paths.Length; wire++)
            {
                string path = paths[wire];
                (int x, int y) = (0, 0);
                int steps = 0;
                foreach (string command in path.Split(','))
                {
                    (int dx, int dy) = command[0] switch
                    {
                        'R' => (1, 0),
                        'U' => (0, 1),
                        'L' => (-1, 0),
                        'D' => (0, -1),
                        _ => throw new Exception()
                    };

                    int dist = int.Parse(command.Substring(1));
                    for (int i = 0; i < dist; i++)
                    {
                        x += dx;
                        y += dy;
                        steps++;

                        if (!map.TryGetValue((x, y), out var visits))
                            map.Add((x, y), visits = new Dictionary<int, int>());

                        if (!visits.ContainsKey(wire))
                            visits[wire] = steps;
                    }
                }
            }

            Console.WriteLine(map.Where(kvp => kvp.Value.Count >= 2).Select(kvp => Math.Abs(kvp.Key.Item1) + Math.Abs(kvp.Key.Item2)).Min());
            Console.WriteLine(map.Where(kvp => kvp.Value.Count >= 2).Select(kvp => kvp.Value.Values.Sum()).Min());
        }
    }
}
