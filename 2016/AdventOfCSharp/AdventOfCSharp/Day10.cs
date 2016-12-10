using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCSharp
{
    internal static class Day10
    {
        internal static void Solve1(string input)
        {
            Dictionary<int, List<int>> state = new Dictionary<int, List<int>>();
            List<int> GetState(int bot)
            {
                if (!state.ContainsKey(bot))
                    state.Add(bot, new List<int>());

                return state[bot];
            }

            List<string> insts = new List<string>();
            foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None))
            {
                var match = Regex.Match(line, "value (\\d+) goes to bot (\\d+)");
                if (match.Success)
                    GetState(int.Parse(match.Groups[2].Value)).Add(int.Parse(match.Groups[1].Value));
                else
                    insts.Add(line);
            }

            while (true)
            {
                foreach (string line in insts)
                {
                    var match = Regex.Match(line, "bot (\\d+) gives low to ([a-z]+) (\\d+) and high to ([a-z]+) (\\d+)");
                    if (!match.Success)
                        throw new Exception();

                    int bot = int.Parse(match.Groups[1].Value);
                    List<int> botState = GetState(bot);
                    if (botState.Count < 2)
                        continue;

                    int min = botState.Min();
                    int max = botState.Max();
                    botState.Clear();

                    if (min == 17 && max == 61)
                    {
                        Console.WriteLine("Bot: {0}", bot);
                        return;
                    }

                    if (match.Groups[2].Value == "bot")
                        GetState(int.Parse(match.Groups[3].Value)).Add(min);

                    if (match.Groups[4].Value == "bot")
                        GetState(int.Parse(match.Groups[5].Value)).Add(max);
                }
            }
        }

        internal static void Solve2(string input)
        {
            Dictionary<int, List<int>> state = new Dictionary<int, List<int>>();
            List<int> GetState(int bot)
            {
                if (!state.ContainsKey(bot))
                    state.Add(bot, new List<int>());

                return state[bot];
            }

            List<string> insts = new List<string>();
            foreach (string line in input.Split(new[] { "\r\n"}, StringSplitOptions.None))
            {
                var match = Regex.Match(line, "value (\\d+) goes to bot (\\d+)");
                if (match.Success)
                    GetState(int.Parse(match.Groups[2].Value)).Add(int.Parse(match.Groups[1].Value));
                else
                    insts.Add(line);
            }

            Dictionary<int, int> output = new Dictionary<int, int>();

            bool more = true;
            while (more)
            {
                more = false;
                foreach (string line in insts)
                {
                    var match = Regex.Match(line, "bot (\\d+) gives low to ([a-z]+) (\\d+) and high to ([a-z]+) (\\d+)");
                    if (!match.Success)
                        throw new Exception();

                    int bot = int.Parse(match.Groups[1].Value);
                    List<int> botState = GetState(bot);
                    if (botState.Count < 2)
                        continue;

                    more = true;
                    int min = botState.Min();
                    int max = botState.Max();
                    botState.Clear();
                    int lowGoesTo = int.Parse(match.Groups[3].Value);
                    int highGoesTo = int.Parse(match.Groups[5].Value);

                    if (match.Groups[2].Value == "bot")
                        GetState(lowGoesTo).Add(min);
                    else
                        output[lowGoesTo] = min;

                    if (match.Groups[4].Value == "bot")
                        GetState(highGoesTo).Add(max);
                    else
                        output[highGoesTo] = max;
                }
            }

            Console.WriteLine("Answer: {0}", output[0] * output[1] * output[2]);
        }
    }
}
