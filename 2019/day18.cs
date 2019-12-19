using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day18
    {
        public static void Solve()
        {
            string[] inputa = File.ReadAllLines("day18a.txt");
            Console.WriteLine(Solve(inputa, FindPoses(inputa, '@').ToArray(), 0));
            string[] inputb = File.ReadAllLines("day18b.txt");
            Console.WriteLine(Solve(inputb, FindPoses(inputb, '@').ToArray(), 0));
        }

        private static readonly (int x, int y)[] s_neis = { (0, -1), (1, 0), (0, 1), (-1, 0), };
        private static readonly Dictionary<(string poses, uint keys), int> _cache = new Dictionary<(string poses, uint keys), int>();
        private static int Solve(string[] map, (int x, int y)[] poses, uint haveKeys)
        {
            if (BitOperations.PopCount(haveKeys) == 'z' - 'a' + 1)
                return 0;

            string posStr = string.Concat(poses.Select(t => t.x + "|" + t.y + "|"));
            if (_cache.TryGetValue((posStr, haveKeys), out int best))
                return best;

            var reachableKeys = new List<(int x, int y, int numSteps, int robot)>();
            var seen = new HashSet<(int x, int y)>();
            var queue = new Queue<(int x, int y, int steps, int robot)>();
            for (int i = 0; i < poses.Length; i++)
            {
                (int x, int y) = poses[i];
                seen.Add((x, y));
                queue.Enqueue((x, y, 0, i));
            }

            while (queue.Count > 0)
            {
                (int px, int py, int steps, int robot) = queue.Dequeue();

                foreach ((int ox, int oy) in s_neis)
                {
                    char c = map[py + oy][px + ox];
                    if (c == '#')
                        continue;

                    if (c >= 'A' && c <= 'Z' && (haveKeys & (1u << (c - 'A'))) == 0)
                        continue;

                    if (seen.Add((px + ox, py + oy)))
                    {
                        if (c >= 'a' && c <= 'z' && (haveKeys & (1u << (c - 'a'))) == 0)
                            reachableKeys.Add((px + ox, py + oy, steps + 1, robot));
                        queue.Enqueue((px + ox, py + oy, steps + 1, robot));
                    }
                }
            }

            best = int.MaxValue;
            foreach ((int x, int  y, int numSteps, int robot) in reachableKeys)
            {
                uint thisKey = 1u << (map[y][x] - 'a');
                var oldPos = poses[robot];
                poses[robot] = (x, y);
                best = Math.Min(best, numSteps + Solve(map, poses, haveKeys | thisKey));
                poses[robot] = oldPos;
            }

            _cache[(posStr, haveKeys)] = best;
            return best;
        }

        private static IEnumerable<(int x, int y)> FindPoses(string[] map, char c)
        {
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == c)
                        yield return (x, y);
                }
            }
        }
    }
}
