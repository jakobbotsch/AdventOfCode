using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day6
    {
        public static void Solve(string input)
        {
            int[] nums = input.Split().Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();

            int redists = 0;
            HashSet<string> seen = new HashSet<string>();
            while (seen.Add(string.Join("_", nums)))
            {
                Redistribute();
                redists++;
            }

            string start = string.Join("_", nums);
            int cycleLength = 0;
            do
            {
                Redistribute();
                cycleLength++;
            } while (string.Join("_", nums) != start);

            Console.WriteLine(redists);
            Console.WriteLine(cycleLength);

            void Redistribute()
            {
                int index = 0;
                for (int i = 1; i < nums.Length; i++)
                {
                    if (nums[i] > nums[index])
                        index = i;
                }

                int toDistribute = nums[index];
                nums[index] = 0;
                while (toDistribute > 0)
                {
                    index++;
                    if (index == nums.Length)
                        index = 0;
                    nums[index]++;
                    toDistribute--;
                }
            }
        }
    }
}
