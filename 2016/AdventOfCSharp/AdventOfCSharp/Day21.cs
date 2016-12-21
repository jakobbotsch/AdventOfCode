using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCSharp
{
    internal static class Day21
    {
        internal static void Solve1(string input)
        {
            List<char> chars = new List<char>("abcdefgh");

            foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None))
            {
                string[] split = line.Split(' ');
                int[] ints = split.Select(s => int.TryParse(s, out int i) ? i : 0).ToArray();
                if (split[0] == "swap")
                {
                    if (split[1] == "position")
                    {
                        char c = chars[ints[2]];
                        chars[ints[2]] = chars[ints[5]];
                        chars[ints[5]] = c;
                    }
                    else if (split[1] == "letter")
                    {
                        char l1 = split[2][0];
                        char l2 = split[5][0];

                        chars = chars.Select(c => c == l1 ? l2 : c == l2 ? l1 : c).ToList();
                    }
                }
                else if (split[0] == "reverse")
                {
                    int min = ints[2];
                    int max = ints[4];
                    for (int i = min, j = max; i < j; i++, j--)
                    {
                        char c = chars[i];
                        chars[i] = chars[j];
                        chars[j] = c;
                    }
                }
                else if (split[0] == "rotate")
                {
                    if (split[1] == "left")
                    {
                        chars = chars.Select((c, i) => chars[(i + ints[2]) % chars.Count]).ToList();
                    }
                    else if (split[1] == "right")
                    {
                        RotateRight(ints[2]);
                    }
                    else if (split[1] == "based")
                    {
                        char l = split[6][0];
                        int index = chars.FindIndex(c => c == l);
                        RotateRight(1 + index + (index >= 4 ? 1 : 0));
                    }
                }
                else if (split[0] == "move")
                {
                    char c = chars[ints[2]];
                    chars.RemoveAt(ints[2]);
                    chars.Insert(ints[5], c);
                }

                Console.WriteLine("After '{0}': {1}", line, new string(chars.ToArray()));
            }

            Console.WriteLine("Result: {0}", new string(chars.ToArray()));

            void RotateRight(int positions)
            {
                chars = chars.Select((c, i) =>
                {
                    int index = (i - positions) % chars.Count;
                    if (index < 0)
                        index += chars.Count;
                    return chars[index];
                }).ToList();
            }
        }

        internal static void Solve2(string input)
        {
            List<char> chars = new List<char>("fbgdceah");

            foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None).Reverse())
            {
                string[] split = line.Split(' ');
                int[] ints = split.Select(s => int.TryParse(s, out int i) ? i : 0).ToArray();
                if (split[0] == "swap")
                {
                    if (split[1] == "position")
                    {
                        char c = chars[ints[2]];
                        chars[ints[2]] = chars[ints[5]];
                        chars[ints[5]] = c;
                    }
                    else if (split[1] == "letter")
                    {
                        char l1 = split[2][0];
                        char l2 = split[5][0];

                        chars = chars.Select(c => c == l1 ? l2 : c == l2 ? l1 : c).ToList();
                    }
                }
                else if (split[0] == "reverse")
                {
                    int min = ints[2];
                    int max = ints[4];
                    for (int i = min, j = max; i < j; i++, j--)
                    {
                        char c = chars[i];
                        chars[i] = chars[j];
                        chars[j] = c;
                    }
                }
                else if (split[0] == "rotate")
                {
                    if (split[1] == "right")
                    {
                        chars = chars.Select((c, i) => chars[(i + ints[2]) % chars.Count]).ToList();
                    }
                    else if (split[1] == "left")
                    {
                        RotateRight(ints[2]);
                    }
                    else if (split[1] == "based")
                    {
                        char l = split[6][0];
                        var after = chars.ToList();
                        for (int i = 0; i < chars.Count - 1; i++)
                        {
                            var before = after.Select((c, j) => chars[(j + ints[2]) % chars.Count]).ToList();
                            chars = before.ToList();
                            int index = chars.FindIndex(c => c == l);
                            RotateRight(1 + index + (index >= 4 ? 1 : 0));
                            if (Enumerable.SequenceEqual(chars, after))
                            {
                                chars = before;
                                break;
                            }
                        }
                    }
                }
                else if (split[0] == "move")
                {
                    char c = chars[ints[5]];
                    chars.RemoveAt(ints[5]);
                    chars.Insert(ints[2], c);
                }

                Console.WriteLine("After '{0}': {1}", line, new string(chars.ToArray()));
            }

            Console.WriteLine("Result: {0}", new string(chars.ToArray()));

            void RotateRight(int positions)
            {
                chars = chars.Select((c, i) =>
                {
                    int index = (i - positions) % chars.Count;
                    if (index < 0)
                        index += chars.Count;
                    return chars[index];
                }).ToList();
            }
        }
    }
}
