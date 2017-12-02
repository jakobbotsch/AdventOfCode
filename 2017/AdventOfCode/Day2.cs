using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal static class Day2
    {
        public static void Solve1(string input)
        {
            int[][] nums =
                input.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(s => Regex.Matches(s, "[0-9]+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray()).ToArray();

            Console.WriteLine(nums.Sum(row => row.Max() - row.Min()));
        }

        public static void Solve2(string input)
        {
            int[][] nums =
                input.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(s => Regex.Matches(s, "[0-9]+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray()).ToArray();

            Console.WriteLine(nums.Sum(row =>
            {
                for (int i = 0; i < row.Length; i++)
                {
                    for (int j = 0; j < row.Length; j++)
                    {
                        if (i == j)
                            continue;

                        if (row[i] % row[j] == 0 && row[i] > row[j])
                            return row[i] / row[j];
                    }
                }

                throw new Exception("unreach");
            }));
        }
    }
}
