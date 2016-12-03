using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCSharp
{
    internal static class Day3
    {
        internal static void Solve1(string input)
        {
            int count = 0;
            string[] lines = input.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string[] split = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var (s1, s2, s3) = (int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));

                if (s1 < s2 + s3 &&
                    s2 < s1 + s3 &&
                    s3 < s1 + s2)
                    count++;
            }

            Console.WriteLine(count);
        }

        internal static void Solve2(string input)
        {
            int count = 0;
            int[][] lines = input.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();

            void Check(int s1, int s2, int s3)
            {
                if (s1 < s2 + s3 &&
                    s2 < s1 + s3 &&
                    s3 < s1 + s2)
                    count++;
            }

            for (int i = 0; i < lines.Length; i += 3)
            {
                Check(lines[i][0], lines[i + 1][0], lines[i + 2][0]);
                Check(lines[i][1], lines[i + 1][1], lines[i + 2][1]);
                Check(lines[i][2], lines[i + 1][2], lines[i + 2][2]);
            }

            Console.WriteLine(count);
        }
    }
}
