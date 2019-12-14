using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day14
    {
        class Chemical
        {
            public Chemical(string name, long amount)
            {
                Name = name;
                Amount = amount;
            }

            public string Name;
            public long Amount;
            public override string ToString() => $"{Amount} {Name}";
        }

        class Production
        {
            public Production(Chemical[] input, Chemical output)
            {
                Input = input;
                Output = output;
            }

            public Chemical[] Input { get; }
            public Chemical Output { get; }

            public override string ToString()
                => $"{string.Join<Chemical>(", ", Input)} => {Output}";
        }

        public static void Solve()
        {
            string[] lines = File.ReadAllLines("day14.txt");

            Chemical ParseChem(string s)
            {
                s = s.Trim();
                string[] split = s.Split(' ');
                return new Chemical(split[1], int.Parse(split[0]));
            }

            List<Production> productions =
                lines
                .Select(l => l.Split("=>"))
                .Select(a => new Production(a[0].Split(',').Select(ParseChem).ToArray(), ParseChem(a[1])))
                .ToList();

            using (Context ctx = new Context())
            {
                Solver CreateInstance(out IntExpr ore, out IntExpr fuel)
                {
                    Solver sol = ctx.MkSolver();
                    Dictionary<string, IntExpr> made = new Dictionary<string, IntExpr>();
                    Dictionary<string, IntExpr> numProductions = new Dictionary<string, IntExpr>();
                    foreach (Production p in productions)
                    {
                        IntExpr numMade = ctx.MkIntConst(p.Output.Name);
                        IntExpr numProds = ctx.MkIntConst($"{p.Output.Name}Prod");
                        sol.Assert(ctx.MkEq(numMade, numProds * ctx.MkInt(p.Output.Amount)));
                        sol.Assert(numMade >= 0);
                        sol.Assert(numProds >= 0);

                        made.Add(p.Output.Name, numMade);
                        numProductions.Add(p.Output.Name, numProds);
                    }
                    made["ORE"] = ctx.MkIntConst("ORE");

                    foreach (var (name, expr) in made)
                    {
                        List<ArithExpr> consumedByProds = new List<ArithExpr>();
                        foreach (Production p in productions)
                        {
                            Chemical req = p.Input.SingleOrDefault(c => c.Name == name);
                            if (req == null)
                                continue;

                            consumedByProds.Add(numProductions[p.Output.Name] * ctx.MkInt(req.Amount));
                        }

                        if (consumedByProds.Count <= 0)
                            continue;
                        sol.Assert(expr >= ctx.MkAdd(consumedByProds));
                    }

                    ore = made["ORE"];
                    fuel = made["FUEL"];
                    return sol;
                }

                Console.WriteLine(
                    Util.BinarySearchForSmallest(
                        bound =>
                        {
                            Solver sol = CreateInstance(out var ore, out var fuel);
                            sol.Assert(ctx.MkEq(fuel, ctx.MkInt(1)));
                            sol.Assert(ore <= ctx.MkInt(bound));
                            return sol.Check() switch
                            {
                                Status.SATISFIABLE => true,
                                Status.UNSATISFIABLE => false,
                                _ => throw new Exception("Unk")
                            };
                        }));

                Console.WriteLine(
                    Util.BinarySearchForLargest(
                        bound =>
                        {
                            Solver sol = CreateInstance(out var ore, out var fuel);
                            sol.Assert(ctx.MkEq(ore, ctx.MkInt(1000000000000)));
                            sol.Assert(fuel >= ctx.MkInt(bound));
                            return sol.Check() switch
                            {
                                Status.SATISFIABLE => true,
                                Status.UNSATISFIABLE => false,
                                _ => throw new Exception("Unk")
                            };
                        }));
            }
        }
    }
}
