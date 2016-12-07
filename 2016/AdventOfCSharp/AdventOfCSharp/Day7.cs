using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCSharp
{
    internal static class Day7
    {
        internal static void Solve1(string input)
        {
            bool SupportsTLS(string line)
            {
                bool inside = false;
                bool found = false;
                for (int i = 0; i < line.Length - 3; i++)
                {
                    switch (line[i])
                    {
                        case '[': inside = true; continue;
                        case ']': inside = false; continue;
                        default:
                            if (line[i] == line[i + 3] && line[i + 1] == line[i + 2] && line[i] != line[i + 1])
                            {
                                if (inside)
                                    return false;

                                found = true;
                            }

                            break;
                    }
                }

                return found;
            }

            Console.WriteLine("Count: {0}", input.Split(new[] { "\r\n" }, StringSplitOptions.None).Where(SupportsTLS).Count());
        }

        internal static void Solve2(string input)
        {
            bool SupportsSSL(string line)
            {
                string inside = "";
                string outside = "";

                bool isInside = false;
                foreach (char c in line)
                {
                    if (c == '[')
                    {
                        inside += "  ";
                        isInside = true;
                    }
                    else if (c == ']')
                    {
                        outside += "  ";
                        isInside = false;
                    }
                    else
                    {
                        if (isInside)
                            inside += c;
                        else
                            outside += c;
                    }
                }

                for (int i = 0; i < outside.Length - 2; i++)
                {
                    if (outside[i] == outside[i + 2] && outside[i] != outside[i+1])
                    {
                        string bab = "" + outside[i + 1] + outside[i] + outside[i + 1];
                        if (inside.Contains(bab))
                            return true;
                    }
                }

                return false;
            }

            Console.WriteLine("Count: {0}", input.Split(new[] { "\r\n" }, StringSplitOptions.None).Where(SupportsSSL).Count());
        }
    }
}
