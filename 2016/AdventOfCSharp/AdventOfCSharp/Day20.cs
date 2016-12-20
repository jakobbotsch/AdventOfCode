using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCSharp
{
    internal static class Day20
    {
        internal static void Solve(string input)
        {
            List<(long, long)> pairs = new List<(long, long)>();
            foreach (string line in input.Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                long min = long.Parse(line.Split('-')[0]);
                long max = long.Parse(line.Split('-')[1]);

                pairs.Add((min, max));
            }

            long NextAllowed(long index)
            {
                while (true)
                {
                    foreach (var (min, max) in pairs)
                    {
                        if (index >= min && index <= max)
                        {
                            index = max + 1;
                            goto next;
                        }
                    }

                    return index;
                next:
                    ;
                }
            }

            long NextDisallowed(long index)
            {
                long amin = long.MaxValue;
                foreach (var (min, max) in pairs)
                {
                    if (min >= index && min < amin)
                        amin = min;
                }

                return amin;
            }

            Console.WriteLine(NextAllowed(0));

            long sum = 0;
            bool allowed = false;
            long cur = 0;
            while (cur <= uint.MaxValue)
            {
                if (!allowed)
                {
                    cur = NextAllowed(cur);
                }
                else
                {
                    long next = NextDisallowed(cur);
                    sum += next - cur;
                    cur = next;
                }

                allowed = !allowed;
            }

            Console.WriteLine(sum);
        }
    }
}
