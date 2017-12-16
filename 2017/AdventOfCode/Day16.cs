using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day16
    {
        public static void Solve(string input)
        {
            char[] progs = "abcdefghijklmnop".ToCharArray();
            string[] insts = input.Split(',');
            Dictionary<string, int> seen = new Dictionary<string, int>();

            for (int times = 0; times < 1000000000; times++)
            {
                if (seen.TryGetValue(new string(progs), out int seenIters))
                {
                    int cycleLen = times - seenIters;
                    times += (1000000000 - times) / cycleLen * cycleLen;
                }
                else
                    seen.Add(new string(progs), times);

                foreach (string inst in insts)
                {
                    if (inst[0] == 's')
                    {
                        for (int i = 0, end = int.Parse(inst.Substring(1)); i < end; i++)
                        {
                            char newFront = progs.Last();
                            for (int j = progs.Length - 1; j > 0; j--)
                                progs[j] = progs[j - 1];

                            progs[0] = newFront;
                        }
                        continue;
                    }

                    if (inst[0] == 'x')
                    {
                        int slash = inst.IndexOf('/');
                        int a = int.Parse(inst.Substring(1, slash - 1));
                        int b = int.Parse(inst.Substring(slash + 1));
                        char c = progs[a];
                        progs[a] = progs[b];
                        progs[b] = c;
                        continue;
                    }

                    if (inst[0] == 'p')
                    {
                        char c1 = inst[1];
                        char c2 = inst[3];
                        int a = -1, b = -1;
                        for (int i = 0; i < progs.Length; i++)
                        {
                            if (progs[i] == c1)
                                a = i;
                            if (progs[i] == c2)
                                b = i;
                        }

                        char c = progs[a];
                        progs[a] = progs[b];
                        progs[b] = c;
                    }
                }

                if (times == 0)
                    Console.WriteLine(new string(progs));
            }

            Console.WriteLine(new string(progs));
        }
    }
}
