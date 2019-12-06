using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day6
    {
        public static void Solve()
        {
            Dictionary<string, string> directOrbits =
                    File.ReadAllLines("day6.txt")
                        .Select(s => s.Split(')'))
                        .ToDictionary(t => t[1], t => t[0]);

            Dictionary<string, HashSet<string>> orbits =
                    directOrbits.ToDictionary(t => t.Key, t => new HashSet<string>(new[] { t.Value }));
            orbits.Add("COM", new HashSet<string>());
            bool any = true;
            while (any)
            {
                any = false;
                foreach (var kvp in orbits)
                {
                    foreach (var mass in kvp.Value.ToList())
                    {
                        int precount = kvp.Value.Count;
                        kvp.Value.UnionWith(orbits[mass]);
                        any |= kvp.Value.Count > precount;
                    }
                }
            }

            Console.WriteLine(orbits.Sum(kvp => kvp.Value.Count));

            Dictionary<string, List<string>> directOrbiters =
                directOrbits.GroupBy(kvp => kvp.Value).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Key).ToList());

            Queue<(string mass, int steps)> queue = new Queue<(string mass, int steps)>();
            HashSet<string> seen = new HashSet<string>();
            queue.Enqueue((directOrbits["YOU"], 0));
            seen.Add(directOrbits["YOU"]);

            string target = directOrbits["SAN"];
            while (true)
            {
                (string cur, int steps) = queue.Dequeue();
                if (cur == target)
                {
                    Console.WriteLine(steps);
                    break;
                }

                IEnumerable<string> Neighbors()
                {
                    if (directOrbiters.TryGetValue(cur, out var orbiters))
                    {
                        foreach (string s in orbiters)
                            yield return s;
                    }

                    if (directOrbits.TryGetValue(cur, out string mass))
                        yield return mass;
                }

                foreach (string orbiter in Neighbors())
                {
                    if (!seen.Add(orbiter))
                        continue;

                    queue.Enqueue((orbiter, steps + 1));
                }
            }
        }
    }
}
