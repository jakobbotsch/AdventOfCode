using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCSharp
{
    internal static class Day1
    {
        internal static void Solve2(string input)
        {
            var (curX, curY) = (0, 0);
            var (dirX, dirY) = (0, 1);
            HashSet<(int, int)> seen = new HashSet<(int, int)>();
            foreach (string s in input.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (s[0] == 'R')
                    (dirX, dirY) = (dirY, -dirX);
                else
                    (dirX, dirY) = (-dirY, dirX);

                int distance = int.Parse(s.Substring(1));
                for (int i = 0; i < distance; i++)
                {
                    if (!seen.Add((curX, curY)))
                    {
                        Console.WriteLine("Distance: {0}", Math.Abs(curX) + Math.Abs(curY));
                        goto Done;
                    }

                    (curX, curY) = (curX + dirX, curY + dirY);
                }
            }

            Done:
            ;
        }
    }
}
