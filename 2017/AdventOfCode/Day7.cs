using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal static class Day7
    {
        public static void Solve(string input)
        {
            Dictionary<string, (double weight, List<string> children)> nodes = new Dictionary<string, (double weight, List<string> children)>();
            foreach (string line in Util.GetLines(input))
            {
                var match = Regex.Match(line, "(?<name>[a-z]+) \\((?<weight>[0-9]+)\\)( -> (?<children>.*))?");
                Trace.Assert(match.Success);
                string name = match.Groups["name"].Value;
                double weight = double.Parse(match.Groups["weight"].Value);
                List<string> children;
                if (match.Groups["children"].Success)
                    children = match.Groups["children"].Value.Split(',').Select(s => s.Trim()).ToList();
                else
                    children = new List<string>();

                nodes.Add(name, (weight, children));
            }

            Dictionary<string, string> parents = new Dictionary<string, string>();
            foreach (var (name, (weight, children)) in nodes.Select(k => (k.Key, k.Value)))
            {
                foreach (string s in children)
                    parents[s] = name;
            }

            string root = nodes.Keys.Single(n => !parents.ContainsKey(n));
            Console.WriteLine(root);
            Solve2(root);

            double Solve2(string node)
            {
                double weight = nodes[node].weight;
                var children = nodes[node].children;
                if (children.Count <= 0)
                    return weight;

                List<double> childWeights = children.Select(Solve2).ToList();
                double correctWeight = childWeights.OrderByDescending(w => childWeights.Count(w2 => w2 == w)).First();

                if (childWeights.All(w => w == correctWeight))
                    return weight + childWeights[0] * childWeights.Count;

                Trace.Assert(childWeights.Count > 2);
                double newWeight = 0;
                foreach (var (child, childWeight) in children.Zip(childWeights, (a, b) => (a, b)))
                {
                    if (childWeight != correctWeight)
                    {
                        newWeight = nodes[child].weight + (correctWeight - childWeight);
                        break;
                    }
                }

                Console.WriteLine(newWeight);
                return correctWeight;
            }
        }
    }
}
