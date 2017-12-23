using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day23
    {
        public static void Solve(string input)
        {
            long[] regs = new long[8];
            regs[0] = 1;
            string[] lines = Util.GetLines(input);
            int ans = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split();
                switch (parts[0])
                {
                    case "set": GetReg(parts[1]) = Parse(parts[2]); break;
                    case "sub": GetReg(parts[1]) -= Parse(parts[2]); break;
                    case "mul": GetReg(parts[1]) *= Parse(parts[2]); ans++; break;
                    case "jnz":
                        long val = Parse(parts[1]);
                        if (val != 0)
                        {
                            i += checked((int)Parse(parts[2]));
                            i--;
                            continue;
                        }
                        break;
                    default: Trace.Fail("noo"); break;
                }
            }

            Console.WriteLine(ans);

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

        public static void Solve2(int min, int max)
        {
            int count = 0;
            for (int num = min; num <= max; num += 17)
            {
                for (int d = 2; d < num; d++)
                {
                    if (num % d == 0)
                    {
                        count++;
                        break;
                    }
                }
            }
            Console.WriteLine(count);
        }
    }
}
