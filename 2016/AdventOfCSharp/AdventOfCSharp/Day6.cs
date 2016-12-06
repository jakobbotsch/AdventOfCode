using System;
using System.Linq;
using System.Text;

namespace AdventOfCSharp
{
    internal static class Day6
    {
        internal static void Solve1(string input)
        {
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder(lines[0]);
            for (int i = 0; i < lines[0].Length; i++)
            {
                sb[i] = lines.Select(l => l[i]).GroupBy(c => c).OrderByDescending(g => g.Count()).First().Key;
            }

            Console.WriteLine("Pass: {0}", sb);
        }

        internal static void Solve2(string input)
        {
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder(lines[0]);
            for (int i = 0; i < lines[0].Length; i++)
            {
                sb[i] = lines.Select(l => l[i]).GroupBy(c => c).OrderBy(g => g.Count()).First().Key;
            }

            Console.WriteLine("Pass: {0}", sb);
        }
    }
}
