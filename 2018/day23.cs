using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public static class Program
    {
        private static int[] ParseNumbers(string line)
            => Regex.Matches(line, "-?\\d+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();

        public static void Main()
        {
            string input = File.ReadAllText("day23.txt");
            (int x, int y, int z, int r)[] bots =
                    input
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .Select(ParseNumbers)
                    .Select(a => (a[0], a[1], a[2], a[3])).ToArray();

            int CountInRange((int x, int y, int z) pos, int range)
                => bots.Count(t => Math.Abs(pos.x - t.x) + Math.Abs(pos.y - t.y) + Math.Abs(pos.z - t.z) <= range);

            var sorted = bots.OrderByDescending(t => t.r).ToArray();
            int inRangeOfBest = CountInRange((sorted[0].x, sorted[0].y, sorted[0].z), sorted[0].r);
            Console.WriteLine(inRangeOfBest);

            using (Context ctx = new Context(new Dictionary<string, string> { ["model"] = "true" }))
            {
                ArithExpr Z3Abs(ArithExpr expr)
                    => (ArithExpr)ctx.MkITE(ctx.MkLt(expr, ctx.MkInt(0)), ctx.MkUnaryMinus(expr), expr);

                IntExpr xvar = ctx.MkIntConst("x");
                IntExpr yvar = ctx.MkIntConst("y");
                IntExpr zvar = ctx.MkIntConst("z");
                IntExpr[] inRangeBots = new IntExpr[bots.Length];
                List<BoolExpr> constraints = new List<BoolExpr>();
                for (int i = 0; i < bots.Length; i++)
                {
                    (int x, int y, int z, int r) = bots[i];

                    inRangeBots[i] = ctx.MkIntConst($"inRange_{i}");
                    ArithExpr xd = ctx.MkSub(xvar, ctx.MkInt(x));
                    ArithExpr yd = ctx.MkSub(yvar, ctx.MkInt(y));
                    ArithExpr zd = ctx.MkSub(zvar, ctx.MkInt(z));
                    ArithExpr dist = ctx.MkAdd(Z3Abs(xd), Z3Abs(yd), Z3Abs(zd));
                    BoolExpr inRangeQ = ctx.MkNot(ctx.MkGt(dist, ctx.MkInt(r)));
                    constraints.Add(ctx.MkEq(inRangeBots[i], ctx.MkITE(inRangeQ, ctx.MkInt(1), ctx.MkInt(0))));
                }

                IntExpr inRange = ctx.MkIntConst("inRange");
                constraints.Add(ctx.MkEq(inRange, ctx.MkAdd(inRangeBots)));
                IntExpr distTo0 = ctx.MkIntConst("distTo0");
                constraints.Add(ctx.MkEq(distTo0, ctx.MkAdd(Z3Abs(xvar), Z3Abs(yvar), Z3Abs(zvar))));

                Optimize opt = ctx.MkOptimize();
                opt.Assert(constraints);
                opt.MkMaximize(inRange);
                opt.MkMinimize(distTo0);
                Status s = opt.Check();

                Console.WriteLine(opt.Model.Evaluate(distTo0));
            }
        }

        private static int Distance((int x, int y, int z) t1, (int x, int y, int z) t2)
        {
            int d = Math.Abs(t1.x - t2.x) + Math.Abs(t1.y - t2.y) + Math.Abs(t1.z - t2.z);
            return d;
        }
    }
}
