using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal static class Day8
    {
        public static void Solve(string input)
        {
            Dictionary<string, int> regs = new Dictionary<string, int>();
            int maxDuring = 0;
            foreach (string line in Util.GetLines(input))
            {
                string[] split = line.Split();
                string mod = split[0];
                string modOp = split[1];
                int modConst = int.Parse(split[2]);
                string guardReg = split[4];
                string guardOp = split[5];
                int guardConst = int.Parse(split[6]);

                int guardVal = GetReg(guardReg);
                switch (guardOp)
                {
                    case "<": if (guardVal >= guardConst) continue; break;
                    case "<=": if (guardVal > guardConst) continue; break;
                    case "==": if (guardVal != guardConst) continue; break;
                    case "!=": if (guardVal == guardConst) continue; break;
                    case ">=": if (guardVal < guardConst) continue; break;
                    case ">": if (guardVal <= guardConst) continue; break;
                    default: Trace.Fail("abc"); break;
                }

                int regVal = GetReg(mod);
                switch(modOp)
                {
                    case "inc": regs[mod] = regVal + modConst; break;
                    case "dec": regs[mod] = regVal - modConst; break;
                    default: Trace.Fail("abc2"); break;
                }

                if (regs[mod] > maxDuring)
                    maxDuring = regs[mod];
            }

            Console.WriteLine(regs.Values.Max());
            Console.WriteLine(maxDuring);

            int GetReg(string name) => regs.TryGetValue(name, out int val) ? val : 0;
        }
    }
}
