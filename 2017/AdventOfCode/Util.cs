using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal static class Util
    {
        public static string[] GetLines(string input)
        {
            return input.Replace(Environment.NewLine, "\n").Split('\n');
        }

        public static int[] GetInts(string line)
        {
            return Regex.Matches(line, "[0-9]+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
        }
    }
}
