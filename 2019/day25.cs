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
    internal static class Day25
    {
        public static async Task SolveAsync()
        {
            long[] prog = File.ReadAllText("day25.txt").Split(',').Select(long.Parse).ToArray();
            Channel<long> inputs = Channel.CreateUnbounded<long>();
            Channel<long> outputs = Channel.CreateUnbounded<long>();
            Task droid = IntCode.RunAsync(prog, inputs, outputs);

            Task.Run(async () =>
                {
                    await foreach (long l in outputs.Reader.ReadAllAsync())
                    {
                        Console.Write((char)l);
                    }
                });

            while (true)
            {
                string s = Console.ReadLine();
                foreach (char c in s)
                    await inputs.Writer.WriteAsync((long)c);

                await inputs.Writer.WriteAsync((long)'\n');
            }

            await droid;
        }
    }
}
