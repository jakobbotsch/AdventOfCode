using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode
{
    internal static class Day9
    {
        public static async Task SolveAsync()
        {
            int[] program = File.ReadAllText("day9.txt").Split(',').Select(int.Parse).ToArray();
            for (int i = 1; i <= 2; i++)
            {
                BufferBlock<long> inputs = new BufferBlock<long>();
                BufferBlock<long> outputs = new BufferBlock<long>();
                inputs.Post(i);
                await RunAsync(SetUpMem(program), inputs, outputs);
                outputs.TryReceiveAll(out var l);
                foreach (var val in l)
                    Console.WriteLine(val);
            }

            static long[] SetUpMem(int[] p)
                => p.Select(i => (long)i).Concat(Enumerable.Repeat(0L, 1024*1024)).ToArray();

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
