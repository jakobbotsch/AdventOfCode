using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    internal static class Day20
    {
        public static void Solve()
        {
            string[] map = File.ReadAllLines("day20.txt");

            var portalStrs = new Dictionary<string, (int x, int y)>();
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] != '.')
                        continue;

                    void Check(int dx, int dy)
                    {
                        int x1 = dx < 0 ? x + dx + dx : x + dx;
                        int y1 = dy < 0 ? y + dy + dy : y + dy;
                        int x2 = dx < 0 ? x + dx : x + dx + dx;
                        int y2 = dy < 0 ? y + dy : y + dy + dy;

                        string str = "" + map[y1][x1] + map[y2][x2];
                        if (!Regex.IsMatch(str, "^[A-Z][A-Z]$"))
                            return;

                        if (portalStrs.ContainsKey(str))
                            portalStrs.Add(str + "2", (x, y));
                        else
                            portalStrs.Add(str, (x, y));
                    }

                    Check(0, -1);
                    Check(1, 0);
                    Check(0, 1);
                    Check(-1, 0);
                }
            }

            var portals = new Dictionary<(int x, int y), (int x, int y)>();
            foreach (var kvp in portalStrs)
            {
                if (kvp.Key.Contains("2") || kvp.Key == "AA" || kvp.Key == "ZZ")
                    continue;

                portals.Add(kvp.Value, portalStrs[kvp.Key + "2"]);
                portals.Add(portalStrs[kvp.Key + "2"], kvp.Value);
            }

            var start = portalStrs["AA"];
            var end = portalStrs["ZZ"];

            (int x, int y)[] neis = { (0, -1), (1, 0), (0, 1), (-1, 0) };
            var queue = new Queue<(int x, int y, int steps)>();
            var seen = new HashSet<(int x, int y)>();
            seen.Add(start);
            queue.Enqueue((start.x, start.y, 0));

            while (queue.Count > 0)
            {
                (int x, int y, int steps) = queue.Dequeue();

                if ((x, y) == end)
                {
                    Console.WriteLine(steps);
                    break;
                }

                void EnqueueNei(int nx, int ny)
                {
                    if (map[ny][nx] == '.' && seen.Add((nx, ny)))
                        queue.Enqueue((nx, ny, steps + 1));
                }

                if (portals.TryGetValue((x, y), out var portalEnd))
                    EnqueueNei(portalEnd.x, portalEnd.y);

                foreach (var (ox, oy) in neis)
                    EnqueueNei(x + ox, y + oy);
            }

            bool IsInnerPortal((int x, int y) portal)
                => portal.x >= 5 && portal.y >= 5 && portal.x <= map[0].Length - 5 && portal.y <= map.Length - 5;

            queue = null;
            seen = null;
            var queue2 = new Queue<(int x, int y, int level, int steps)>();
            var seen2 = new HashSet<(int x, int y, int level)>();
            seen2.Add((start.x, start.y, 0));
            queue2.Enqueue((start.x, start.y, 0, 0));

            while (queue2.Count > 0)
            {
                (int x, int y, int steps, int level) = queue2.Dequeue();

                if ((x, y) == end && level == 0)
                {
                    Console.WriteLine(steps);
                    break;
                }

                void EnqueueNei(int nx, int ny, int levelOffs)
                {
                    if (map[ny][nx] == '.' && seen2.Add((nx, ny, level + levelOffs)))
                        queue2.Enqueue((nx, ny, steps + 1, level + levelOffs));
                }

                if (portals.TryGetValue((x, y), out var portalEnd) && (level > 0 || IsInnerPortal((x, y))))
                    EnqueueNei(portalEnd.x, portalEnd.y, IsInnerPortal((x, y)) ? 1 : -1);

                foreach (var (ox, oy) in neis)
                    EnqueueNei(x + ox, y + oy, 0);
            }
        }
    }
}
