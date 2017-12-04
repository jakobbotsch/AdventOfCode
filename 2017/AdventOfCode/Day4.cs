using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day4
    {
        public static void Solve1(string input)
        {
            Console.WriteLine(Util.GetLines(input).Count(s => s.Split().All(new HashSet<string>().Add)));
        }

        public static void Solve2(string input)
        {
            Console.WriteLine(Util.GetLines(input).Count(s =>
            {
                var hs = new HashSet<string>();
                return s.Split().All(w => hs.Add(string.Concat(w.OrderBy(c => c))));
            }));
        }
    }
}
