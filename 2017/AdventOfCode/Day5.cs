using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day5
    {
        public static void Solve(string input)
        {
            int[] jumps = Util.GetLines(input).Select(int.Parse).ToArray();

            int steps = 0;
            int ip = 0;
            while (ip >= 0 && ip < jumps.Length)
            {
                jumps[ip]++;
                ip += jumps[ip] - 1;
                steps++;
            }

            Console.WriteLine(steps);

            jumps = Util.GetLines(input).Select(int.Parse).ToArray();

            steps = 0;
            ip = 0;
            while (ip >= 0 && ip < jumps.Length)
            {
                int newIp = ip + jumps[ip];
                if (jumps[ip] >= 3)
                    jumps[ip]--;
                else
                    jumps[ip]++;

                ip = newIp;
                steps++;
            }

            Console.WriteLine(steps);
        }
    }
}
