using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal static class Day4
    {
        public static void Solve()
        {
            int[] nums = File.ReadAllText("day4.txt").Split('-').Select(int.Parse).ToArray();
            int part1 = 0;
            int part2 = 0;
            for (int i = nums[0]; i <= nums[1]; i++)
            {
                string s = i.ToString();
                bool hasTwo = false;
                bool ascending = true;
                for (int j = 0; j < s.Length - 1; j++)
                {
                    ascending &= s[j] <= s[j + 1];
                    hasTwo |= s[j] == s[j + 1];
                }

                bool hasTwoAlone = false;
                for (int j = 0; j < s.Length - 1; j++)
                {
                    if (s[j] != s[j + 1])
                        continue;

                    if (j+2 < s.Length && s[j+2] == s[j])
                    {
                        // more than 2, skip entire group
                        char c = s[j];
                        while (j < s.Length && s[j] == c)
                            j++;
                        j--;
                        continue;
                    }

                    hasTwoAlone = true;
                }

                if (ascending && hasTwo)
                    part1++;
                if (ascending && hasTwoAlone)
                    part2++;
            }
            Console.WriteLine(part1);
            Console.WriteLine(part2);
        }
    }
}
