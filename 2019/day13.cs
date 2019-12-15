using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode
{
    internal static class Day13
    {
        public static async Task SolveAsync()
        {
            long[] program = File.ReadAllText("day13.txt").Split(',').Select(long.Parse).ToArray();
            Channel<long> input = Channel.CreateUnbounded<long>();
            Channel<long> output = Channel.CreateUnbounded<long>();
            await IntCode.RunAsync(program, input, output);

            var screen = new Dictionary<(long, long), long>();
            while (!output.Reader.Completion.IsCompleted)
            {
                long x = await output.Reader.ReadAsync();
                long y = await output.Reader.ReadAsync();
                long t = await output.Reader.ReadAsync();
                screen[(x, y)] = t;
            }

            Console.WriteLine(screen.Values.Count(v => v == 2));

            program[0] = 2;

            input = Channel.CreateUnbounded<long>();
            output = Channel.CreateUnbounded<long>();
            Task game = IntCode.RunAsync(program, input, output);
            long score = 0;
            int lastMove = 1;
            while (true)
            {
                int ballx = -1;
                int padx = -1;
                while (ballx == -1 || (lastMove != 0 && padx == -1))
                {
                    Task<long> task = output.Reader.ReadAsync().AsTask();
                    await Task.WhenAny(game, task);
                    if (game.IsCompleted)
                    {
                        Console.WriteLine(score);
                        return;
                    }

                    long x = task.Result;
                    long y = await output.Reader.ReadAsync();
                    long t = await output.Reader.ReadAsync();
                    if (x == -1)
                    {
                        score = t;
                    }
                    else
                    {
                        if (t == 3)
                            padx = (int)x;
                        else if (t == 4)
                            ballx = (int)x;
                    }
                }

                lastMove = Math.Clamp(ballx - padx, -1, 1);
                await input.Writer.WriteAsync(lastMove);
            }
        }
    }
}
