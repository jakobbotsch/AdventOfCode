using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCSharp
{
    internal static class Day9
    {
        internal static void Solve1(string input)
        {
            StringBuilder result = new StringBuilder();
            Regex reg = new Regex("^\\((\\d+)x(\\d+)\\).*$");
            for (int i = 0; i < input.Length; i++)
            {
                var match = reg.Match(input.Substring(i));
                if (!match.Success)
                {
                    result.Append(input[i]);
                    continue;
                }

                int numChars = int.Parse(match.Groups[1].Value);
                int repeats = int.Parse(match.Groups[2].Value);

                i += match.Groups[1].Length + match.Groups[2].Length + 3;
                string substring = input.Substring(i, numChars);
                for (int j = 0; j < repeats; j++)
                    result.Append(substring);

                i += numChars;
                i--;
            }

            Console.WriteLine("Length: {0}", result.ToString().Where(c => !char.IsWhiteSpace(c)));
        }

        internal static void Solve2(string input)
        {
            long length = 0;
            Regex reg = new Regex("^\\((\\d+)x(\\d+)\\).*$");
            input = "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN";

            void Scan(int multiplier, int start, int end)
            {
                for (int i = start; i < end; i++)
                {
                    var match = reg.Match(input.Substring(i));
                    if (!match.Success)
                    {
                        length += multiplier;
                        continue;
                    }

                    int numChars = int.Parse(match.Groups[1].Value);
                    int repeats = int.Parse(match.Groups[2].Value);

                    i += match.Groups[1].Length + match.Groups[2].Length + 3;

                    Scan(multiplier * repeats, i, i + numChars);
                    i += numChars;
                    i--;
                }
            }

            Scan(1, 0, input.Length);

            Console.WriteLine("Length: {0}", length);
        }
    }
}
