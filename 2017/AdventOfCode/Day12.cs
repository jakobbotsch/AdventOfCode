using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day12
    {
        public static void Solve(string input)
        {
            Dictionary<int, List<int>> nodes = new Dictionary<int, List<int>>();
            foreach (string line in Util.GetLines(input))
            {
                int[] nums = line.Replace("<-> ", "").Replace(",", "").Split().Select(int.Parse).ToArray();
                foreach (int nei in nums.Skip(1))
                {
                    GetNeis(nums[0]).Add(nei);
                    GetNeis(nei).Add(nums[0]);
                }
            }

            HashSet<int> seen = new HashSet<int>();

            int groups = 0;
            foreach (int root in nodes.Keys)
            {
                bool containsBefore = seen.Contains(0);

                if (!seen.Add(root))
                    continue;

                groups++;
                Queue<int> next = new Queue<int>();
                next.Enqueue(root);

                while (next.Count > 0)
                {
                    int id = next.Dequeue();
                    foreach (int nei in GetNeis(id))
                    {
                        if (seen.Add(nei))
                            next.Enqueue(nei);
                    }
                }

                if (!containsBefore && seen.Contains(0))
                    Console.WriteLine("0: " + seen.Count);
            }

            Console.WriteLine(groups);

            List<int> GetNeis(int index)
            {
                if (!nodes.TryGetValue(index, out var list))
                    nodes.Add(index, list = new List<int>());

                return list;
            }
        }
    }
}
