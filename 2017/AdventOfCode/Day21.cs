using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    internal class Day21
    {
        public static void Solve(string input)
        {
            string[] pattern =
            {
                ".#.",
                "..#",
                "###"
            };

//            input = @"../.# => ##./#../...
//.#./..#/### => #..#/..../..../#..#";

            Dictionary<string, string> rules =
                Util.GetLines(input)
                .Select(l => l.Split(new[] { " => " }, StringSplitOptions.None))
                .ToDictionary(a => a[0], a => a[1]);

            foreach (KeyValuePair<string, string> rule in rules.ToList())
            {
                string[] parsed = rule.Key.Split('/');
                char[][] rotated = parsed.Select(s => s.ToCharArray()).ToArray();

                List<string[]> variants = new List<string[]>();
                for (int i = 0; i < 4; i++)
                {
                    string[] variant = rotated.Select(c => new string(c)).ToArray();
                    variants.Add(variant);
                    // flip across horizontal
                    variants.Add(variant.Reverse().ToArray());
                    // flip across vertical
                    variants.Add(variant.Select(a => new string(a.Reverse().ToArray())).ToArray());
                    // flip across both
                    variants.Add(variant.Reverse().Select(a => new string(a.Reverse().ToArray())).ToArray());

                    char[][] oldRotated = rotated.Select(r => r.ToArray()).ToArray();
                    for (int y = 0; y < parsed.Length; y++)
                    {
                        for (int x = 0; x < parsed.Length; x++)
                        {
                            rotated[y][x] = oldRotated[parsed.Length - 1 - x][y];
                        }
                    }
                }

                var distinct = variants.Select(v => string.Join("/", v)).Distinct().ToList();
                foreach (string variant in distinct)
                {
                    if (rules.TryGetValue(variant, out string other))
                        Trace.Assert(other == rule.Value);
                    else
                        rules.Add(variant, rule.Value);
                }
            }

            for (int i = 0; i < 18; i++)
            {
                NextRound();
            }

            Console.WriteLine(pattern.Sum(p => p.Count(c => c == '#')));

            void NextRound()
            {
                List<string[]> patterns = new List<string[]>();

                int newSize = pattern.Length % 2 == 0 ? 2 : 3;
                int numNewSquares = pattern.Length / newSize;
                for (int ny = 0; ny < numNewSquares; ny++)
                {
                    for (int nx = 0; nx < numNewSquares; nx++)
                    {
                        int xb = nx * newSize;
                        int yb = ny * newSize;
                        string[] newPattern = new string[newSize];
                        for (int i = 0; i < newSize; i++)
                            newPattern[i] = pattern[yb + i].Substring(xb, newSize);

                        patterns.Add(newPattern);
                    }
                }

                List<string[]> replacedPatterns = patterns.Select(s => rules[string.Join("/", s)].Split('/')).ToList();
                int newReplacedSize = replacedPatterns.Take(numNewSquares).Sum(s => s.Length);
                char[][] grid = new char[newReplacedSize][];
                for (int i = 0; i < newReplacedSize; i++)
                    grid[i] = new char[newReplacedSize];

                for (int sy = 0; sy < numNewSquares; sy++)
                {
                    for (int sx = 0; sx < numNewSquares; sx++)
                    {
                        string[] square = replacedPatterns[sy * numNewSquares + sx];
                        int xb = sx * square.Length;
                        int yb = sy * square.Length;
                        for (int y = 0; y < square.Length; y++)
                        {
                            for (int x = 0; x < square.Length; x++)
                            {
                                Trace.Assert(grid[yb + y][xb + x] == 0);
                                grid[yb + y][xb + x] = square[y][x];
                            }
                        }
                    }
                }

                Trace.Assert(grid.All(ca => ca.All(c => c != 0)));
                pattern = grid.Select(ca => new string(ca)).ToArray();
            }
        }
    }
}
