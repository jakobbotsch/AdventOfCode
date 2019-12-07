using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode
{
    internal static class Day7
    {
        public static async Task SolveAsync()
        {
            int[] program = File.ReadAllText("day7.txt").Split(',').Select(int.Parse).ToArray();
            int max = -1;
            foreach (int[] perm in Permutations.Unordered(new[] { 0, 1, 2, 3, 4 }))
            {
                int signal = 0;
                for (int i = 0; i < perm.Length; i++)
                {
                    BufferBlock<int> inputs = new BufferBlock<int>();
                    inputs.Post(perm[i]);
                    inputs.Post(signal);
                    BufferBlock<int> outputs = new BufferBlock<int>();
                    await RunAsync(program.ToArray(), inputs, outputs);
                    signal = await outputs.ReceiveAsync();
                }

                max = Math.Max(signal, max);
            }

            Console.WriteLine(max);

            max = -1;
            foreach (int[] perm in Permutations.Unordered(new[] { 5, 6, 7, 8, 9 }))
            {
                BufferBlock<int>[] blocks = Enumerable.Range(0, perm.Length).Select(_ => new BufferBlock<int>()).ToArray();
                List<Task> amps = new List<Task>();
                for (int i = 0; i < perm.Length; i++)
                {
                    blocks[i].Post(perm[i]);
                    amps.Add(RunAsync(program.ToArray(), blocks[i], blocks[(i + 1) % blocks.Length]));
                }

                blocks[0].Post(0);
                await Task.WhenAll(amps);
                max = Math.Max(max, await blocks[0].ReceiveAsync());
            }

            Console.WriteLine(max);

            static async Task RunAsync(int[] mem, BufferBlock<int> inputs, BufferBlock<int> outputs)
            {
                int ip = 0;
                while (true)
                {
                    ref int GetParam(int index)
                    {
                        ref int fst = ref mem[ip + 1 + index];
                        int divisor = 1;
                        for (int i = 0; i < index; i++)
                            divisor *= 10;
                        int mode = (mem[ip] / 100) / divisor % 10;
                        if (mode == 0)
                            return ref mem[fst];
                        Trace.Assert(mode == 1);
                        return ref fst;
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
                            int input = await inputs.ReceiveAsync();
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
