using System;

namespace AdventOfCode
{
    internal static class Day1
    {
        public static void Solve1(string input)
        {
            int sum = 0;
            for (int j = input.Length - 1, i = 0; i < input.Length; j = i++)
            {
                if (input[j] == input[i])
                    sum += input[j] - '0';
            }

            Console.WriteLine(sum);
        }

        public static void Solve2(string input)
        {
            int sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                char c1 = input[i];
                char c2 = input[(i + input.Length / 2) % input.Length];

                if (c1 == c2)
                    sum += c1 - '0';
            }

            Console.WriteLine(sum);
        }
    }
}
