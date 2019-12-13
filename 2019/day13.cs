using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal static class Day13
    {
        public static async Task SolveAsync()
        {
            long[] program = File.ReadAllText("day13.txt").Split(',').Select(long.Parse).ToArray();
            var input = new BufferBlock<long>();
            var output = new BufferBlock<long>();
            await RunAsync(program, input, output);

            output.TryReceiveAll(out IList<long> vals);
            var screen = new Dictionary<(long, long), long>();
            for (int i = 0; i < vals.Count; i += 3)
            {
                screen[(vals[i + 0], vals[i + 1])] = vals[i + 2];
            }

            Console.WriteLine(screen.Values.Count(v => v == 2));

            program[0] = 2;

            input = new BufferBlock<long>();
            output = new BufferBlock<long>();
            Task game = RunAsync(program, input, output);
            long score = 0;
            int lastMove = 1;
            while (true)
            {
                int ballx = -1;
                int padx = -1;
                while (ballx == -1 || (lastMove != 0 && padx == -1))
                {
                    Task<long> task = output.ReceiveAsync();
                    await Task.WhenAny(game, task);
                    if (game.IsCompleted)
                    {
                        Console.WriteLine(score);
                        return;
                    }

                    long x = task.Result;
                    long y = await output.ReceiveAsync();
                    long t = await output.ReceiveAsync();
                    if (x == -1)
                    {
                        score = t;
                    }
                    else
                    {
                        if (t == 3)
                            padx = (int)x;
                        else if (t == 4)
                            ballx = (int)x;
                    }
                }

                lastMove = Math.Clamp(ballx - padx, -1, 1);
                input.Post(lastMove);
            }

            static async Task RunAsync(long[] mem, BufferBlock<long> inputs, BufferBlock<long> outputs)
            {
                mem = mem.Concat(Enumerable.Repeat(0L, 1024 * 1024)).ToArray();
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

        private static long LCM(long a, long b) => a * b / GCD(a, b);

        private static long GCD(long a, long b)
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
