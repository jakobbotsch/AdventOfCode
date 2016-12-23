using System;
using System.Collections.Generic;

namespace AdventOfCSharp
{
    internal static class Day23
    {
        internal static void Solve(string input)
        {
            //int a, b, c, d;
            //a = 12;
            //b = a;
            //b--;
            //do
            //{
            //    d = a;
            //    a = 0;
            //    do
            //    {
            //        c = b;
            //        do
            //        {
            //            a++;
            //            c--;
            //        } while (c != 0);
            //        d--;
            //    } while (d != 0);
            //    b--;
            //    c = b;
            //    d = c;
            //    do
            //    {
            //        d--;
            //        c++;
            //    } while (d != 0);
            //    Console.WriteLine("Toggle {0}", c);
            //    c = -16;
            //} while (true);
            //c = 75;
            //do
            //{
            //    do
            //    {
            //        a++;
            //        d++;
            //    } while (d != 0);
            //    c++;
            //} while (c != 0);
            //Console.WriteLine("A: {0}", a);

            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<string, int> values = new Dictionary<string, int>
            {
                ["a"] = 12,
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
                if (split[0] == "nop")
                    continue;
                else if (split[0] == "mul")
                {
                    if (values.ContainsKey(split[2]))
                        values[split[2]] *= GetVal(split[1]);
                }
                else if (split[0] == "cpy")
                {
                    if (values.ContainsKey(split[2]))
                        values[split[2]] = GetVal(split[1]);
                }
                else if (split[0] == "inc")
                {
                    if (values.ContainsKey(split[1]))
                        values[split[1]]++;
                }
                else if (split[0] == "dec")
                {
                    if (values.ContainsKey(split[1]))
                        values[split[1]]--;
                }
                else if (split[0] == "jnz")
                {
                    if (GetVal(split[1]) != 0)
                    {
                        pc += GetVal(split[2]);
                        pc--;
                    }
                }
                else if (split[0] == "tgl")
                {
                    int pos = GetVal(split[1]);
                    if (pc + pos < 0 || pc + pos >= lines.Length)
                        continue;

                    Console.WriteLine("Toggling {0}", pc + pos);

                    string line = lines[pc + pos];
                    if (line.StartsWith("cpy"))
                        line = line.Replace("cpy", "jnz");
                    else if (line.StartsWith("inc"))
                        line = line.Replace("inc", "dec");
                    else if (line.StartsWith("dec"))
                        line = line.Replace("dec", "inc");
                    else if (line.StartsWith("jnz"))
                        line = line.Replace("jnz", "cpy");
                    else if (line.StartsWith("tgl"))
                        line = line.Replace("tgl", "inc");

                    lines[pc + pos] = line;
                }
            }

            Console.WriteLine(GetVal("a"));
        }
    }
}
