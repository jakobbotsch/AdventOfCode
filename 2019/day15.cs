using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day15
    {
        private static readonly (int, int)[] s_neis = { (0, -1), (1, 0), (0, 1), (-1, 0) };
        public static async Task SolveAsync()
        {
            long[] prog = File.ReadAllText("day15.txt").Split(',').Select(long.Parse).ToArray();
            Channel<long> input = Channel.CreateUnbounded<long>();
            Channel<long> output = Channel.CreateUnbounded<long>();
            Task droid = IntCode.RunAsync(prog, input, output);
            (int x, int y) = (0, 0);
            var map = new Dictionary<(int x, int y), char>();

            var moves = new Stack<int>();
            map[(0, 0)] = '.';
            moves.Push(1);
            moves.Push(2);
            moves.Push(3);
            moves.Push(4);

            while (moves.Count > 0)
            {
                int move = moves.Pop();
                (int dx, int dy) = GetDir(move);

                await input.Writer.WriteAsync(move);
                long status = await output.Reader.ReadAsync();
                switch (status)
                {
                    case 0:
                        map[(x + dx, y + dy)] = '#';
                        continue;
                    case 1:
                    case 2:
                        x += dx;
                        y += dy;

                        // Have we already explored this?
                        if (map.ContainsKey((x, y)))
                        {
                            // Coming back, so don't go back yet again
                        }
                        else
                        {
                            map[(x, y)] = status == 2 ? 'O' : '.';
                            // Move back
                            moves.Push(move switch
                            {
                                1 => 2,
                                2 => 1,
                                3 => 4,
                                4 => 3,
                                _ => throw new Exception("Unhandled"),
                            });

                            // Move in unexplored directions
                            for (int nextMove = 1; nextMove <= 4; nextMove++)
                            {
                                (dx, dy) = GetDir(nextMove);
                                if (map.TryGetValue((x + dx, y + dy), out char m))
                                    continue;
                                moves.Push(nextMove);
                            }
                        }

                        continue;
                    default:
                        throw new Exception("Unknown state");
                }
            }

            var parents = new Dictionary<(int x, int y), (int px, int py)>();
            Bfs(0, 0);
            var oxygen = map.Single(kvp => kvp.Value == 'O');
            Console.WriteLine(GetPath(oxygen.Key.x, oxygen.Key.y).Count);
            Bfs(oxygen.Key.x, oxygen.Key.y);
            Console.WriteLine(map.Where(kvp => kvp.Value == '.').Select(p => GetPath(p.Key.x, p.Key.y).Count).Max());

            int minx = map.Keys.Select(kvp => kvp.x).Min();
            int miny = map.Keys.Select(kvp => kvp.y).Min();
            int maxx = map.Keys.Select(kvp => kvp.x).Max();
            int maxy = map.Keys.Select(kvp => kvp.y).Max();
            for (int my = miny; my <= maxy; my++)
            {
                for (int mx = minx; mx <= maxx; mx++)
                {
                    Console.Write(map.TryGetValue((mx, my), out char m) ? m : ' ');
                }
                Console.WriteLine();
            }

            static (int x, int y) GetDir(int move)
                => move switch
                {
                    1 => (0, -1),
                    2 => (0, 1),
                    3 => (-1, 0),
                    4 => (1, 0),
                    _ => throw new Exception("Invalid move"),
                };

            void Bfs(int sx, int sy)
            {
                parents.Clear();
                var queue = new Queue<(int x, int y)>();
                queue.Enqueue((sx, sy));
                parents.Add((sx, sy), (sx, sy));
                while (queue.Count > 0)
                {
                    (int cx, int cy) = queue.Dequeue();
                    foreach ((int nx, int ny) in s_neis)
                    {
                        (int, int) nei = (cx + nx, cy + ny);
                        if (!map.TryGetValue(nei, out char c) || c == '#')
                            continue;

                        if (parents.ContainsKey(nei))
                            continue;

                        parents.Add(nei, (cx, cy));
                        queue.Enqueue(nei);
                    }
                }
            }

            List<(int x, int y)> GetPath(int tx, int ty)
            {
                var l = new List<(int x, int y)>();
                while (parents[(tx, ty)] != (tx, ty))
                {
                    l.Add((tx, ty));
                    (tx, ty) = parents[(tx, ty)];
                }

                l.Reverse();
                return l;
            }
        }
    }
}
