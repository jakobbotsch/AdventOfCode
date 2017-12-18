using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day18
    {
        public static void Solve1(string input)
        {
            long[] regs = new long[30];
            long lastFreq = -1;
            string[] lines = Util.GetLines(input);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split();
                switch (parts[0])
                {
                    case "snd": lastFreq = Parse(parts[1]); break;
                    case "set": GetReg(parts[1]) = Parse(parts[2]); break;
                    case "add": GetReg(parts[1]) += Parse(parts[2]); break;
                    case "mul": GetReg(parts[1]) *= Parse(parts[2]); break;
                    case "mod": GetReg(parts[1]) %= Parse(parts[2]); break;
                    case "rcv":
                        long val = GetReg(parts[1]);
                        if (val == 0)
                            continue;

                        Console.WriteLine(lastFreq);
                        return;
                    case "jgz":
                        val = GetReg(parts[1]);
                        if (val > 0)
                        {
                            i += checked((int)Parse(parts[2]));
                            i--;
                            continue;
                        }
                        break;
                    default: Trace.Fail("noo"); break;
                }
            }

            ref long GetReg(string val)
            {
                Trace.Assert(val.Length == 1);
                return ref regs[val[0] - 'a'];
            }

            long Parse(string val)
            {
                if (val.Length == 1 && val[0] >= 'a' && val[0] <= 'z')
                    return regs[val[0] - 'a'];

                return long.Parse(val);
            }
        }

        public static void Solve2(string input)
        {
            BlockingCollection<long>[] queues = new BlockingCollection<long>[2];
            queues[0] = new BlockingCollection<long>();
            queues[1] = new BlockingCollection<long>();

            Task.WhenAll(Enumerable.Range(0, 2).Select(
                pid => Task.Run(() =>
                {
                    int other = 1 - pid;
                    int sent = 0;

                    long[] regs = new long[30];
                    regs['p' - 'a'] = pid;
                    string[] lines = Util.GetLines(input);
                    for (int i = 0; i >= 0 && i < lines.Length; i++)
                    {
                        string line = lines[i];
                        string[] parts = line.Split();
                        switch (parts[0])
                        {
                            case "snd": queues[other].Add(Parse(parts[1])); sent++; break;
                            case "set": GetReg(parts[1]) = Parse(parts[2]); break;
                            case "add": GetReg(parts[1]) += Parse(parts[2]); break;
                            case "mul": GetReg(parts[1]) *= Parse(parts[2]); break;
                            case "mod": GetReg(parts[1]) %= Parse(parts[2]); break;
                            case "rcv": GetReg(parts[1]) = queues[pid].Take(); break;
                            case "jgz":
                                long val = Parse(parts[1]);
                                if (val > 0)
                                {
                                    i += checked((int)Parse(parts[2]));
                                    i--;
                                }
                                break;
                            default: Trace.Fail("noo"); break;
                        }

                        if (pid == 1)
                            Console.WriteLine(sent);
                    }

                    ref long GetReg(string val)
                    {
                        Trace.Assert(val.Length == 1);
                        return ref regs[val[0] - 'a'];
                    }

                    long Parse(string val)
                    {
                        if (val.Length == 1 && val[0] >= 'a' && val[0] <= 'z')
                            return regs[val[0] - 'a'];

                        return long.Parse(val);
                    }
                }))).Wait();
        }
    }
}
