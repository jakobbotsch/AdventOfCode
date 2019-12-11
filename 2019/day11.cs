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
    internal static class Day11
    {
        public static async Task SolveAsync()
        {
            long[] program = File.ReadAllText("day11.txt").Split(',').Select(long.Parse).ToArray();
            var inputs = new BufferBlock<long>();
            var outputs = new BufferBlock<long>();
            Task t = RunAsync(SetUpMem(program), inputs, outputs);

            var colors = new Dictionary<(int x, int y), int>();
            colors[(0, 0)] = 1;
            (int x, int y) = (0, 0);
            (int dx, int dy) = (0, -1);
            while (true)
            {
                inputs.Post(colors.TryGetValue((x, y), out int color) ? color : 0);
                Task<long> colorTask = outputs.ReceiveAsync();
                await Task.WhenAny(t, colorTask);
                if (t.IsCompleted)
                    break;
                colors[(x, y)] = checked((int)colorTask.Result);
                (dx, dy) = (await outputs.ReceiveAsync()) switch {
                    0 => (dy, -dx),
                    1 => (-dy, dx),
                    _ => throw new Exception("wtf"),
                };
                (x, y) = (x + dx, y + dy);
            }

            Console.WriteLine(colors.Count);
            int minx = colors.Keys.Select(t => t.x).Min();
            int maxx = colors.Keys.Select(t => t.x).Max();
            int miny = colors.Keys.Select(t => t.y).Min();
            int maxy = colors.Keys.Select(t => t.y).Max();

            for (int oy = miny; oy <= maxy; oy++)
            {
                for (int ox = minx; ox <= maxx; ox++)
                {
                    colors.TryGetValue((ox, oy), out int c);
                    Console.Write(c == 0 ? '.' : '#');
                }
                Console.WriteLine();
            }

            static long[] SetUpMem(long[] p)
                => p.Concat(Enumerable.Repeat(0L, 1024*1024)).ToArray();

            static async Task RunAsync(long[] mem, BufferBlock<long> inputs, BufferBlock<long> outputs)
            {
                long ip = 0;
                long relBase = 0;
                while (true)
                {
                    ref long GetParam(long index)
                    {
                        ref long fst = ref mem[ip + 1 + index];
                        int divisor = 1;
                        for (long i = 0; i < index; i++)
                            divisor *= 10;
                        long mode = (mem[ip] / 100) / divisor % 10;
                        if (mode == 0)
                            return ref mem[fst];
                        if (mode == 1)
                            return ref fst;
                        Trace.Assert(mode == 2);
                        return ref mem[relBase + fst];
                    }

                    switch (mem[ip] % 100)
                    {
                        case 1:
                            GetParam(2) = GetParam(0) + GetParam(1);
                            ip += 4;
                            break;
                        case 2:
                            GetParam(2) = GetParam(0) * GetParam(1);
                            ip += 4;
                            break;
                        case 3:
                            long input = await inputs.ReceiveAsync();
                            GetParam(0) = input;
                            ip += 2;
                            break;
                        case 4:
                            outputs.Post(GetParam(0));
                            ip += 2;
                            break;
                        case 5:
                            if (GetParam(0) != 0)
                                ip = GetParam(1);
                            else
                                ip += 3;
                            break;
                        case 6:
                            if (GetParam(0) == 0)
                                ip = GetParam(1);
                            else
                                ip += 3;
                            break;
                        case 7:
                            GetParam(2) = GetParam(0) < GetParam(1) ? 1 : 0;
                            ip += 4;
                            break;
                        case 8:
                            GetParam(2) = GetParam(0) == GetParam(1) ? 1 : 0;
                            ip += 4;
                            break;
                        case 9:
                            relBase += GetParam(0);
                            ip += 2;
                            break;
                        case 99:
                            return;
                        default:
                            Trace.Fail("Oh shit " + mem[ip]);
                            break;
                    }
                }
            }
        }
    }
}
