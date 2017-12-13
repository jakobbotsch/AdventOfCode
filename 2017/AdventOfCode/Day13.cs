using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day13
    {
        public static void Solve(string input)
        {
            Dictionary<int, Wall> layers = Util.GetLines(input).ToDictionary(
                l => Util.GetInts(l)[0],
                l => new Wall
                {
                    Layer = Util.GetInts(l)[0],
                    Depth = Util.GetInts(l)[1],
                });

            int severity = 0;
            int end = layers.Keys.Max() + 1;
            for (int i = 0; i < end; i++)
            {
                if (layers.TryGetValue(i, out Wall w) && w.Cur == 0)
                    severity += w.Layer * w.Depth;

                ScanStep(layers);
            }

            Console.WriteLine(severity);

            foreach (var w in layers.Values)
                w.Cur = 0;

            for (int delay = 0; ; delay++)
            {
                var state = layers.ToDictionary(l => l.Key, l => new Wall { Layer = l.Value.Layer, Depth = l.Value.Depth, Cur = l.Value.Cur });

                for (int i = 0; i < end; i++)
                {
                    if (state.TryGetValue(i, out Wall w) && w.Cur == 0)
                        goto BAD;

                    ScanStep(state);
                }

                Console.WriteLine(delay);
                break;
                BAD:;

                if (delay % 1000 == 0)
                    Console.Title = $"{delay}";

                ScanStep(layers);
            }

            void ScanStep(Dictionary<int, Wall> state)
            {
                foreach (Wall w in state.Values)
                {
                    w.Cur++;
                    if (w.Cur == w.Depth) // was at top?
                        w.Cur = -(w.Depth - 2);
                }
            }
        }

        private class Wall
        {
            public int Layer { get; set; }
            public int Depth { get; set; }
            public int Cur { get; set; }
        }
    }
}
