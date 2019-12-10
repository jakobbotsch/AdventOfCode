using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Numerics;

namespace AdventOfCode
{
    internal static class Day10
    {
        public static async Task SolveAsync()
        {
            char[][] map = File.ReadAllLines("day10.txt").Select(s => s.ToCharArray()).ToArray();
            int best = 0;
            (int bestx, int besty) = (0, 0);
            for (int ys = 0; ys < map.Length; ys++)
            {
                for (int xs = 0; xs < map[ys].Length; xs++)
                {
                    if (map[ys][xs] != '#')
                        continue;

                    int num = 0;
                    for (int ye = 0; ye < map.Length; ye++)
                    {
                        for (int xe = 0; xe < map[ye].Length; xe++)
                        {
                            if (map[ye][xe] != '#' || (xs, ys) == (xe, ye))
                                continue;

                            num += CanSee(map, xs, ys, xe, ye) ? 1 : 0;
                        }
                    }

                    if (num > best)
                    {
                        best = num;
                        (bestx, besty) = (xs, ys);
                    }
                }
            }

            Console.WriteLine(best);

            (int pulvx, int pulvy) = FirstHit(map, bestx, besty, bestx, 0);
            int quadrant = 0;
            int hit = 0;
            while (true)
            {
                Dump();
                Console.ReadLine();
                map[pulvy][pulvx] = '.';
                hit++;

                if (hit == 200)
                {
                    Console.WriteLine(pulvx*100 + pulvy);
                    break;
                }

                (int nx, int ny) = (-1, -1);
                double bestAng = double.MaxValue;

                for (int y = 0; y < map.Length; y++)
                {
                    for (int x = 0; x < map[y].Length; x++)
                    {
                        if ((x, y) == (bestx, besty))
                            continue;

                        if (map[y][x] != '#')
                            continue;

                        if (Perp2D(pulvx - bestx, pulvy - besty, x - bestx, y - besty) <= 0)
                            continue; // Left side or on line

                        Vector2 dpulv = new Vector2(pulvx - bestx, pulvy - besty);
                        Vector2 dir = new Vector2(x - bestx, y - besty);
                        double ang = Math.Acos(Vector2.Dot(dpulv, dir) /
                                               (dpulv.Length() * dir.Length()));
                        if (ang < bestAng)
                        {
                            bestAng = ang;
                            (nx, ny) = (x, y);
                        }
                    }
                }

                (pulvx, pulvy) = FirstHit(map, bestx, besty, nx, ny);
            }

            void Dump()
            {
                for (int y = 0; y < map.Length; y++)
                {
                    for (int x = 0; x < map[y].Length; x++)
                    {
                        if ((x, y) == (pulvx, pulvy))
                            Console.ForegroundColor = ConsoleColor.Red;
                        else if ((x, y) == (bestx, besty))
                            Console.ForegroundColor = ConsoleColor.Green;
                        else if (Perp2D(pulvx - bestx, pulvy - besty, x - bestx, y - besty) > 0)
                            Console.ForegroundColor = ConsoleColor.Yellow;

                        Console.Write(map[y][x]);

                        Console.ResetColor();
                    }

                    Console.WriteLine();
                }
            }
        }

        private static int Perp2D(int v1, int v2, int w1, int w2)
            => v1 * w2 - v2 * w1;

        private static bool CanSee(char[][] map, int xs, int ys, int xe, int ye) 
            => FirstHit(map, xs, ys, xe, ye) == (xe, ye);

        private static (int, int) FirstHit(char[][] map, int xs, int ys, int xe, int ye)
        {
            int gcd = GCD(ye - ys, xe - xs);
            (int dx, int dy) = ((xe - xs)/gcd, (ye - ys)/gcd);

            int x = xs + dx;
            int y = ys + dy;
            while (x != xe || y != ye)
            {
                if (map[y][x] == '#')
                    return (x, y);

                x += dx;
                y += dy;
            }

            return (x, y);
        }


        private static int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }
    }
}
