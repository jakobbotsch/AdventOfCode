using System;

namespace AdventOfCode
{
    internal static class Util
    {
        public static string[] GetLines(string input)
        {
            return input.Replace(Environment.NewLine, "\n").Split('\n');
        }
    }
}
