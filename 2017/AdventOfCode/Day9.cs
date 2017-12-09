using System;
using System.IO;

namespace AdventOfCode
{
    internal static class Day9
    {
        public static void Solve()
        {
            string input = File.ReadAllText("Day9.txt");
            int totalScore = 0;
            int curScore = 0;
            int garbageCount = 0;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                switch (c)
                {
                    case '{': curScore++; break;
                    case '<': ConsumeGarbage(); break;
                    case '}': totalScore += curScore; curScore--; break;
                }

                void ConsumeGarbage()
                {
                    i++;
                    while (true)
                    {
                        if (input[i] == '>')
                            break;

                        if (input[i] == '!')
                        {
                            i += 2;
                            continue;
                        }

                        garbageCount++;
                        i++;
                    }
                }
            }

            Console.WriteLine(totalScore);
            Console.WriteLine(garbageCount);
        }
    }
}
