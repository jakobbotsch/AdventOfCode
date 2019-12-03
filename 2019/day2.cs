using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day2
    {
        public static void Solve()
        {
            string text = File.ReadAllText("day2.txt");
            int[] start = text.Split(',').Select(int.Parse).ToArray();
            int[] mem = start.ToArray();
            mem[1] = 12;
            mem[2] = 2;
            Console.WriteLine(Run(mem));

            for (int i = 0; i < start.Length; i++)
            {
                for (int j = 0; j < start.Length; j++)
                {
                    mem = start.ToArray();
                    mem[1] = i;
                    mem[2] = j;

                    if (Run(mem) == 19690720)
                    {
                        Console.WriteLine(100 * i + j);
                    }
                }
            }

            int Run(int[] mem)
            {
                int ip = 0;
                while (mem[ip] != 99)
                {
                    switch (mem[ip])
                    {
                        case 1:
                            mem[mem[ip + 3]] = mem[mem[ip + 1]] + mem[mem[ip + 2]];
                            ip += 4;
                            break;
                        case 2:
                            mem[mem[ip + 3]] = mem[mem[ip + 1]] * mem[mem[ip + 2]];
                            ip += 4;
                            break;
                        default:
                            Trace.Fail("Oh shit");
                            break;
                    }
                }
                return mem[0];
            }
        }
    }
}
