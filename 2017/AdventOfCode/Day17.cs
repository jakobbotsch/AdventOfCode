using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    internal static class Day17
    {
        public static void Solve(int input)
        {
            List<int> vals = new List<int> { 0 };

            int curPos = 0;
            for (int i = 1; i < 2018; i++)
            {
                curPos = (curPos + input) % vals.Count;
                curPos++;
                vals.Insert(curPos, i);
            }

            Console.WriteLine(vals[curPos + 1]);

            curPos = 0;
            int len = 1;
            int afterZero = -1;
            for (int i = 1; i < 50000000; i++)
            {
                curPos = (curPos + input) % len;
                curPos++;
                if (curPos == 1)
                    afterZero = i;

                len++;
            }

            Console.WriteLine(afterZero);
        }
    }
}
