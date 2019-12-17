using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day17
    {
        public static async Task SolveAsync()
        {
            long[] prog = File.ReadAllText("day17.txt").Split(',').Select(long.Parse).ToArray();
            Channel<long> input = Channel.CreateUnbounded<long>();
            Channel<long> output = Channel.CreateUnbounded<long>();
            await IntCode.RunAsync(prog, input, output);
            var map = new Dictionary<(int x, int y), char>();
            int x = 0;
            int y = 0;
            await foreach (long val in output.Reader.ReadAllAsync())
            {
                char c =(char)val;
                if (c == '\n')
                {
                    x = 0;
                    y++;
                    continue;
                }

                map.Add((x, y), c);
                x++;
            }

            int width = map.Keys.Select(t => t.x).Max();
            int height = map.Keys.Select(t => t.y).Max();

            (int x, int y)[] neis = { (0, -1), (1, 0), (0, 1), (-1, 0) };
            var intersections =
                map.Keys.Where(t => neis.Sum(nei => map.TryGetValue((t.x + nei.x, t.y + nei.y), out char c) && c == '#' ? 1 : 0) >= 3)
                        .Where(t => map[t] == '#')
                        .ToList(); 
            Console.WriteLine(intersections.Sum(t => t.x * t.y));
            (int sdx, int sdy) =
                map.Values.Contains('^') ? (0, -1) :
                map.Values.Contains('>') ? (1, 0) :
                map.Values.Contains('v') ? (0, 1) :
                map.Values.Contains('<') ? (-1, 0) :
                throw new Exception("No");

            (int dx, int dy) = (sdx, sdy);

            (int vx, int vy) = map.SingleOrDefault(kvp => "^>v<".Contains(kvp.Value)).Key;
            string path = "";
            while (true)
            {
                if (map.TryGetValue((vx + dx, vy + dy), out char ahead) && ahead == '#')
                {
                    path += "F";
                    vx += dx;
                    vy += dy;
                    continue;
                }

                (int ndx, int ndy) = (-1, -1);
                foreach ((int cdx, int cdy) in neis)
                {
                    if (cdx == -dx && cdy == -dy)
                        continue; // Don't flip around

                    if (map.TryGetValue((vx + cdx, vy + cdy), out char c) && c == '#')
                    {
                        Trace.Assert(ndx == -1 && ndy == -1);
                        (ndx, ndy) = (cdx, cdy);
                    }
                }

                if ((ndx, ndy) == (-1, -1))
                    break;

                (int ldx, int ldy) = (dy, -dx);
                (int rdx, int rdy) = (-dy, dx);
                if ((ndx, ndy) == (ldx, ldy))
                    path += "L";
                else
                {
                    Debug.Assert((ndx, ndy) == (rdx, rdy));
                    path += "R";
                }

                (dx, dy) = (ndx, ndy);
            }

            Console.WriteLine("Path needed: {0}", path);

            prog[0] = 2;
            input = Channel.CreateUnbounded<long>();
            output = Channel.CreateUnbounded<long>();
            async Task WriteLineAsync(string s)
            {
                foreach (char c in s)
                    await input.Writer.WriteAsync(c);
                await input.Writer.WriteAsync('\n');
            }
            await WriteLineAsync("A,B,A,B,C,C,B,A,B,C");
            await WriteLineAsync("L,12,L,10,R,8,L,12");
            await WriteLineAsync("R,8,R,10,R,12");
            await WriteLineAsync("L,10,R,12,R,8");
            await WriteLineAsync("n");
            await IntCode.RunAsync(prog, input, output);
            long last = 0;
            await foreach (long l in output.Reader.ReadAllAsync())
            {
                last = l;
            }

            Console.WriteLine(last);
        }
    }
}
