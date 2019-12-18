using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day18
    {
        public static void Solve()
        {
            string[] inputa = File.ReadAllLines("day18a.txt");
            Console.WriteLine(SolveTsp(inputa, FindPos(inputa, '@'), true));
            string[] inputb = File.ReadAllLines("day18b.txt");
            (int sx, int sy) = FindPos(inputb, '@');
            Console.WriteLine(
                SolveTsp(inputb, (sx, sy), false) +
                SolveTsp(inputb, (sx + 2, sy), false) +
                SolveTsp(inputb, (sx, sy + 2), false) +
                SolveTsp(inputb, (sx + 2, sy + 2), false));
        }

        private static (int x, int y) FindPos(string[] map, char c)
        {
            int y = Array.FindIndex(map, s => s.Contains(c));
            int x = map[y].IndexOf(c);
            return (x, y);
        }

        private static readonly (int x, int y)[] s_neis = { (0, -1), (1, 0), (0, 1), (-1, 0), };
        private static int SolveTsp(string[] map, (int x, int y) start, bool requireKeys)
        {
            var seen = new HashSet<(int x, int y)>();
            var queue = new Queue<(int x, int y)>();
            seen.Add((start.x, start.y));
            queue.Enqueue((start.x, start.y));
            string keys = "";
            while (queue.Count > 0)
            {
                (int px, int py) = queue.Dequeue();
                char c = map[py][px];
                if (c >= 'a' && c <= 'z')
                    keys += c;

                foreach ((int ox, int oy) in s_neis)
                {
                    if (map[py + oy][px + ox] == '#')
                        continue;

                    if (seen.Add((px + ox, py + oy)))
                        queue.Enqueue((px + ox, py + oy));
                }
            }

            keys = string.Concat(keys.OrderBy(c => c));

            // This assumes there is only one path between each key, which seems to be the case.
            int[,] distances = new int[keys.Length, keys.Length];
            uint[,] neededKeys = new uint[keys.Length, keys.Length];
            var shortestPathLength = new Dictionary<(uint visited, int end), int>();
            for (int k1 = 0; k1 < keys.Length; k1++)
            {
                (int k1x, int k1y) = FindPos(map, keys[k1]);
                (int initLength, uint initNeeded) = GetPathDetails(map, keys, start.x, start.y, k1x, k1y);

                if (!requireKeys || initNeeded == 0)
                    shortestPathLength[(1u << k1, k1)] = initLength;

                for (int k2 = 0; k2 < keys.Length; k2++)
                {
                    (int k2x, int k2y) = FindPos(map, keys[k2]);
                    (distances[k1, k2], neededKeys[k1, k2]) = GetPathDetails(map, keys, k1x, k1y, k2x, k2y);
                }
            }

            for (int prevLength = 1; prevLength < keys.Length; prevLength++)
            {
                List<uint> newVisited = new List<uint>();
                foreach ((uint prevVisited, int prevLast) in shortestPathLength.Keys)
                {
                    if (BitOperations.PopCount(prevVisited) != prevLength)
                        continue;

                    for (int i = 0; i < keys.Length; i++)
                    {
                        if ((prevVisited & (1u << i)) == 0)
                            newVisited.Add(prevVisited | (1u << i));
                    }
                }

                foreach (uint visited in newVisited)
                {
                    foreach (int last in new EnumBits(visited))
                    {
                        uint prevVisited = visited & ~(1u << last);
                        int min = int.MaxValue;
                        foreach (int prevLast in new EnumBits(prevVisited))
                        {
                            if (!shortestPathLength.TryGetValue((prevVisited, prevLast), out int prevShortestLength))
                                continue;

                            // Check if we can get 'last' now, after having 'prevVisited' keys
                            if (requireKeys && (~prevVisited & neededKeys[prevLast, last]) != 0)
                                continue; // Nope, we need keys that we don't have from the previous path

                            // Yep, we have everything
                            min = Math.Min(min, prevShortestLength + distances[prevLast, last]);
                        }

                        if (min != int.MaxValue)
                            shortestPathLength[(visited, last)] = min;
                    }
                }
            }

            return shortestPathLength.Where(kvp => BitOperations.PopCount(kvp.Key.visited) == keys.Length)
                                     .Select(kvp => kvp.Value)
                                     .Min();
        }


        private struct EnumBits
        {
            private uint _val;
            public EnumBits(uint val) => _val = val;
            public BitsEnumerator GetEnumerator() => new BitsEnumerator(_val);
        }

        private struct BitsEnumerator
        {
            private uint _val;

            public BitsEnumerator(uint val)
            {
                _val = val;
                Current = 0;
            }

            public int Current { get; private set; }

            public bool MoveNext()
            {
                if (_val == 0)
                    return false;

                Current = BitOperations.TrailingZeroCount(_val);
                _val &= ~(1u << Current);
                return true;
            }
        }

        private static (int dist, uint neededKeys) GetPathDetails(string[] input, string keysStr, int sx, int sy, int ex, int ey)
        {
            List<(int x, int y)> path = Path(input, sx, sy, ex, ey);
            uint keys = 0;
            foreach ((int x, int y) in path)
            {
                char c = input[y][x];
                if (c < 'A' || c > 'Z')
                    continue;

                int index = keysStr.IndexOf(char.ToLower(c));
                if (index != -1)
                    keys |= 1u << index;
            }

            return (path.Count, keys);
        }

        private static List<(int x, int y)> Path(string[] input, int startX, int startY, int endX, int endY)
        {
            var parents = new Dictionary<(int x, int y), (int px, int py)>();
            var queue = new Queue<(int x, int y)>();
            (int x, int y)[] neis = { (0, 1), (-1, 0), (0, -1), (1, 0), };

            parents.Add((startX, startY), (startX, startY));
            queue.Enqueue((startX, startY));

            while (queue.Count > 0)
            {
                (int x, int y) = queue.Dequeue();
                if ((x, y) == (endX, endY))
                {
                    List<(int x, int y)> path = new List<(int x, int y)>();
                    while ((x, y) != (startX, startY))
                    {
                        path.Add((x, y));
                        (x, y) = parents[(x, y)];
                    }

                    path.Reverse();
                    return path;
                }

                foreach ((int nx, int ny) in neis)
                {
                    if (input[y + ny][x + nx] == '#')
                        continue;

                    if (parents.ContainsKey((x + nx, y + ny)))
                        continue;

                    parents[(x + nx, y + ny)] = (x, y);
                    queue.Enqueue((x + nx, y + ny));
                }
            }

            throw new Exception("No path?");
        }
    }
}
