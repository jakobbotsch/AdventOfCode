using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCSharp
{
    internal static class Day4
    {
        internal static void Solve1(string input)
        {
            Regex regex = new Regex("(?<name>.*)-(?<id>\\d+)\\[(?<check>.*)\\]");
            int sum = 0;
            foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None))
            {
                var match = regex.Match(line);
                string name = match.Groups["name"].Value;
                int id = int.Parse(match.Groups["id"].Value);
                string check = match.Groups["check"].Value;

                string correctCheck = new string(name.Where(c => c != '-')
                    .GroupBy(c => c)
                    .OrderByDescending(g => g.Count())
                    .ThenBy(g => g.Key)
                    .Select(g => g.Key)
                    .Take(5)
                    .ToArray());

                if (correctCheck == check)
                {
                    sum += id;
                }
            }

            Console.WriteLine("Sum: {0}", sum);
        }

        internal static void Solve2(string input)
        {
            Regex regex = new Regex("(?<name>.*)-(?<id>\\d+)\\[(?<check>.*)\\]");
            foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None))
            {
                var match = regex.Match(line);
                string name = match.Groups["name"].Value;
                int id = int.Parse(match.Groups["id"].Value);
                string check = match.Groups["check"].Value;

                string correctCheck = new string(name.Where(c => c != '-')
                    .GroupBy(c => c)
                    .OrderByDescending(g => g.Count())
                    .ThenBy(g => g.Key)
                    .Select(g => g.Key)
                    .Take(5)
                    .ToArray());

                if (correctCheck == check)
                {
                    int shift = id % 26;
                    StringBuilder sb = new StringBuilder();
                    foreach (char c in name)
                    {
                        if (c == '-')
                            sb.Append(" ");
                        else
                        {
                            char newC = (char)('a' + ((c - 'a' + shift) % 26));
                            sb.Append(newC);
                        }
                    }

                    Console.WriteLine(sb + ": " + id);
                }
            }
        }
    }
}
