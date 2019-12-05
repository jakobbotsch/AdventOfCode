using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode
{
    internal static class Day5
    {
        public static void Solve()
        {
            string text = File.ReadAllText("day5.txt");
            int[] start = text.Split(',').Select(int.Parse).ToArray();
            int[] mem = start.ToArray();
            Run(mem);

            void Run(int[] mem)
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
                            Console.Write("Input: ");
                            GetParam(0) = int.Parse(Console.ReadLine());
                            ip += 2;
                            break;
                        case 4:
                            Console.WriteLine(GetParam(0));
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
