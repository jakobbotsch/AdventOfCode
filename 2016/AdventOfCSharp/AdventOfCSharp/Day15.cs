using System;
using System.Text.RegularExpressions;

namespace AdventOfCSharp
{
    internal static class Day15
    {
        internal static void Solve(string input)
        {
            input += "\r\nDisc #0 has 11 positions; at time=0, it is at position 0.";
            for (int startTime = 0; ; startTime++)
            {
                int baseTime = startTime;
                foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None))
                {
                    baseTime++;
                    var match = Regex.Match(line, "Disc #(\\d+) has (\\d+) positions; at time=(\\d+), it is at position (\\d+)\\.");
                    int numPos = int.Parse(match.Groups[2].Value);
                    int startPos = int.Parse(match.Groups[4].Value);

                    if ((baseTime + startPos) % numPos != 0)
                    {
                        goto NextTime;
                    }
                }

                Console.WriteLine("Time: {0}", startTime);
                return;
                NextTime:
                ;
            }
        }
    }
}
