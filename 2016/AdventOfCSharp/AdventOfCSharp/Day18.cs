using System;
using System.Linq;

namespace AdventOfCSharp
{
    internal static class Day18
    {
        internal static void Solve(string input)
        {
            const int numRows = 400000;
            bool[] trap = ("." + input + ".").Select(c => c == '^').ToArray();
            bool[][] map = new bool[numRows][];
            map[0] = trap;
            for (int i = 1; i < numRows; i++)
            {
                bool[] row = new bool[map[i - 1].Length];
                for (int j = 1; j < row.Length - 1; j++)
                {
                    bool left = map[i - 1][j - 1];
                    bool center = map[i - 1][j];
                    bool right = map[i - 1][j + 1];

                    row[j] = left && center && !right ||
                             center && right && !left ||
                             left && !center && !right ||
                             right && !center && !left;
                }

                map[i] = row;
            }

            Console.WriteLine("Count: {0}", map.Sum(m => m.Skip(1).Take(m.Length - 2).Count(t => !t)));
        }
    }
}
