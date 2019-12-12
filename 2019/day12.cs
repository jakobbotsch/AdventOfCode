using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal static class Day12
    {
        public static async Task SolveAsync()
        {
            int[][] pos =
                File.ReadAllLines("day12.txt")
                .Select(l => Regex.Matches(l, "-?\\d+").Select(m => int.Parse(m.Value)).ToArray())
                .ToArray();
            int[][] vel = pos.Select(p => new int[p.Length]).ToArray();
            int[] cycleTimes = new int[pos[0].Length];
            HashSet<string> seen = new HashSet<string>();
            for (int step = 0;; step++)
            {
                if (step == 1000)
                {
                    Console.WriteLine(pos.Zip(vel, (p, v) => p.Sum(Math.Abs) * v.Sum(Math.Abs)).Sum());
                }

                for (int dim = 0; dim < pos[0].Length; dim++)
                {
                    if (cycleTimes[dim] != 0)
                        continue;

                    string id = dim + ":" + string.Join("|", pos.Zip(vel, (p, v) => p[dim] + "," + v[dim]));
                    if (!seen.Add(id))
                    {
                        cycleTimes[dim] = step;
                    }
                }

                if (cycleTimes.All(i => i != 0))
                {
                    Console.WriteLine(LCM(LCM(cycleTimes[0], cycleTimes[1]), cycleTimes[2]));
                    break;
                }

                int[][] newVel = vel.Select(v => v.ToArray()).ToArray();
                for (int i = 0; i < pos.Length; i++)
                {
                    int[] pi = pos[i];
                    int[] vi = newVel[i];
                    for (int j = i + 1; j < pos.Length; j++)
                    {
                        int[] pj = pos[j];
                        int[] vj = newVel[j];

                        for (int k = 0; k < pi.Length; k++)
                        {
                            if (pi[k] < pj[k])
                            {
                                vi[k]++;
                                vj[k]--;
                            }
                            else if (pi[k] > pj[k])
                            {
                                vi[k]--;
                                vj[k]++;
                            }
                        }
                    }
                }

                vel = newVel;

                for (int i = 0; i < pos.Length; i++)
                {
                    for (int j = 0; j < pos[i].Length; j++)
                    {
                        pos[i][j] += vel[i][j];
                    }
                }
            }
        }

        private static long LCM(long a, long b) => a * b / GCD(a, b);

        private static long GCD(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }
    }
}
