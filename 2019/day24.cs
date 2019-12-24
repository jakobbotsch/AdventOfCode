using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day24
    {
        public static void Solve()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            char[][] map = File.ReadAllLines("day24.txt").Select(l => l.ToCharArray()).ToArray();
            char[][] newMap = map.Select(l => l.ToArray()).ToArray();
            var seen = new HashSet<uint>();
            uint Biodiversity(char[][] map)
            {
                uint biod = 0;
                int index = 0;
                for (int y = 0; y < map.Length; y++)
                {
                    for (int x = 0; x < map[y].Length; x++)
                    {
                        if (map[y][x] == '#')
                            biod |= 1u << index;

                        index++;
                    }
                }

                return biod;
            }
            while (true)
            {
                uint biod = Biodiversity(map);
                if (!seen.Add(biod))
                {
                    Console.WriteLine(biod);
                    break;
                }
                for (int y = 0; y < newMap.Length; y++)
                {
                    for (int x = 0; x < newMap[y].Length; x++)
                    {
                        int Check(int cx, int cy)
                            => cy >= 0 && cy < map.Length && cx >= 0 && cx < map[cy].Length &&
                            map[cy][cx] == '#' ? 1 : 0;

                        int count = Check(x, y - 1) + Check(x + 1, y) + Check(x, y + 1) + Check(x - 1, y);
                        if (map[y][x] == '#')
                            newMap[y][x] = count == 1 ? '#' : '.';
                        else
                            newMap[y][x] = count == 1 || count == 2 ? '#' : '.';
                    }
                }

                Swap(ref map, ref newMap);
            }
        }

        private static (int x, int y)[] s_neis = {(0, -1), (1, 0), (0, 1), (-1, 0)};
        private static void Part2()
        {
            string[] initialMap = File.ReadAllLines("day24.txt").ToArray();
            var map = new HashSet<(int level, int x, int y)>();
            for (int y = 0; y < initialMap.Length; y++)
            {
                for (int x = 0; x < initialMap[y].Length; x++)
                {
                    if (initialMap[y][x] == '#')
                        map.Add((0, x, y));
                }
            }

            int height = initialMap.Length;
            int width = initialMap[0].Length;
            for (int minute = 0; minute < 200; minute++)
            {
                IEnumerable<(int level, int x, int y)> Adjacent(
                    int level, int cx, int cy)
                {
                    foreach ((int x, int y) in s_neis)
                    {
                        (int nx, int ny) = (cx + x, cy + y);
                        if (nx == width / 2 && ny == height / 2)
                        {
                            // go in
                            if (y > 0)
                            {
                                for (int i = 0; i < width; i++)
                                    yield return (level + 1, i, 0);
                            }
                            else if (x > 0)
                            {
                                for (int i = 0; i < height; i++)
                                    yield return (level + 1, 0, i);
                            }
                            else if (y < 0)
                            {
                                for (int i = 0; i < width; i++)
                                    yield return (level + 1, i, height - 1);
                            }
                            else
                            {
                                for (int i = 0; i < height; i++)
                                    yield return (level + 1, width - 1, i);
                            }

                            continue;
                        }

                        if (nx < 0)
                        {
                            yield return (level - 1, width / 2 - 1, height / 2);
                            continue;
                        }
                        if (ny < 0)
                        {
                            yield return (level - 1, width / 2, height / 2 - 1);
                            continue;
                        }
                        if (nx >= width)
                        {
                            yield return (level - 1, width / 2 + 1, height / 2);
                            continue;
                        }
                        if (ny >= height)
                        {
                            yield return (level - 1, width / 2, height / 2 + 1);
                            continue;
                        }

                        yield return (level, nx, ny);
                    }
                }

                int minLevel = map.Select(t => t.level).Min();
                int maxLevel = map.Select(t => t.level).Max();
                var newMap = new HashSet<(int level, int x, int y)>();
                for (int level = minLevel - 1; level <= maxLevel + 1; level++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if ((x, y) == (width / 2, height / 2))
                                continue;

                            int aliveNeis = 0;
                            foreach (var t in Adjacent(level, x, y))
                            {
                                aliveNeis += map.Contains(t) ? 1 : 0;
                            }

                            bool isAlive =
                                map.Contains((level, x, y))
                                ? (aliveNeis == 1)
                                : (aliveNeis == 1 || aliveNeis == 2);

                            if (isAlive)
                                newMap.Add((level, x, y));
                        }
                    }
                }

                map = newMap;
            }

            Console.WriteLine(map.Count);
        }

        private static void Swap<T>(ref T v1, ref T v2)
        {
            T temp = v1;
            v1 = v2;
            v2 = temp;
        }
    }
}
