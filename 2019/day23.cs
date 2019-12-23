using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day23
    {
        public static async Task SolveAsync()
        {
            long[] prog = File.ReadAllText("day23.txt").Split(',').Select(long.Parse).ToArray();
            List<Task> progs = new List<Task>();
            Channel<long>[] inputs = Enumerable.Range(0, 50).Select(_ =>
                    Channel.CreateUnbounded<long>()).ToArray();
            Channel<long>[] outputs = Enumerable.Range(0, 50).Select(_ =>
                    Channel.CreateUnbounded<long>()).ToArray();
            for (int i = 0; i < 50; i++)
            {
                await inputs[i].Writer.WriteAsync(i);
                progs.Add(IntCode.RunAsync(prog, inputs[i], outputs[i]));
            }

            bool first = true;
            (long x, long y)? lastNat = null;
            long? deliveredNatY = null;
            int idle = 0;
            while (true)
            {
                // Collect all outputs from this round
                var packets = new List<(long addr, long x, long y)>();
                foreach (Channel<long> output in outputs)
                {
                    List<long> collect = new List<long>();
                    while (output.Reader.TryRead(out long l))
                        collect.Add(l);

                    for (int i = 0; i < collect.Count; i += 3)
                        packets.Add((collect[i], collect[i+1], collect[i+2]));
                }

                List<int> losers =
                    Enumerable.Range(0, 50).Except(
                        packets.Select(t => (int)t.addr)).ToList();

                if (losers.Count == 50)
                {
                    idle++;
                    if (idle > 1 && lastNat.HasValue)
                    {
                        packets.Add((0, lastNat.Value.x, lastNat.Value.y));
                        losers.Remove(0);

                        if (lastNat.Value.y == deliveredNatY)
                        {
                            Console.WriteLine(lastNat.Value.y);
                            return;
                        }

                        deliveredNatY = lastNat.Value.y;
                        idle = 0;
                    }
                }
                else
                    idle = 0;

                foreach (var (a, x, y) in packets)
                {
                    if (a == 255)
                    {
                        if (lastNat == null)
                            Console.WriteLine(y);

                        lastNat = (x, y);
                        continue;
                    }

                    await inputs[a].Writer.WriteAsync(x);
                    await inputs[a].Writer.WriteAsync(y);
                }

                foreach (var l in losers)
                    await inputs[l].Writer.WriteAsync(-1);

                await Task.Delay(1);
            }
        }
    }
}
