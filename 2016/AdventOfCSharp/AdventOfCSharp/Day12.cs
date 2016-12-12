using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCSharp
{
    internal static class Day12
    {
        internal static void Solve1(string input)
        {
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<string, int> values = new Dictionary<string, int>
            {
                ["a"] = 0,
                ["b"] = 0,
                ["c"] = 0,
                ["d"] = 0,
            };

            int GetVal(string s)
            {
                if (values.ContainsKey(s))
                    return values[s];

                return int.Parse(s);
            }

            for (int pc = 0; pc < lines.Length; pc++)
            {
                string[] split = lines[pc].Split(' ');
                if (split[0] == "cpy")
                {
                    values[split[2]] = GetVal(split[1]);
                }
                else if (split[0] == "inc")
                    values[split[1]]++;
                else if (split[0] == "dec")
                    values[split[1]]--;
                else if (split[0] == "jnz")
                {
                    if (GetVal(split[1]) != 0)
                    {
                        pc += int.Parse(split[2]);
                        pc--;
                    }
                }
            }

            Console.WriteLine(GetVal("a"));
        }

        internal static void Solve2(string input)
        {
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<string, int> values = new Dictionary<string, int>
            {
                ["a"] = 0,
                ["b"] = 0,
                ["c"] = 1,
                ["d"] = 0,
            };

            int GetVal(string s)
            {
                if (values.ContainsKey(s))
                    return values[s];

                return int.Parse(s);
            }

            for (int pc = 0; pc < lines.Length; pc++)
            {
                string[] split = lines[pc].Split(' ');
                if (split[0] == "cpy")
                {
                    values[split[2]] = GetVal(split[1]);
                }
                else if (split[0] == "inc")
                    values[split[1]]++;
                else if (split[0] == "dec")
                    values[split[1]]--;
                else if (split[0] == "jnz")
                {
                    if (GetVal(split[1]) != 0)
                    {
                        pc += int.Parse(split[2]);
                        pc--;
                    }
                }
            }

            Console.WriteLine(GetVal("a"));
        }
    }
}
