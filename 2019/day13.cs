using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode
{
    internal static class Day13
    {
        public static async Task SolveAsync()
        {
            long[] program = File.ReadAllText("day13.txt").Split(',').Select(long.Parse).ToArray();
            var input = new BufferBlock<long>();
            var output = new BufferBlock<long>();
            await IntCode.RunAsync(program, input, output);

            output.TryReceiveAll(out IList<long> vals);
            var screen = new Dictionary<(long, long), long>();
            for (int i = 0; i < vals.Count; i += 3)
            {
                screen[(vals[i + 0], vals[i + 1])] = vals[i + 2];
            }

            Console.WriteLine(screen.Values.Count(v => v == 2));

            program[0] = 2;

            input = new BufferBlock<long>();
            output = new BufferBlock<long>();
            Task game = IntCode.RunAsync(program, input, output);
            long score = 0;
            int lastMove = 1;
            while (true)
            {
                int ballx = -1;
                int padx = -1;
                while (ballx == -1 || (lastMove != 0 && padx == -1))
                {
                    Task<long> task = output.ReceiveAsync();
                    await Task.WhenAny(game, task);
                    if (game.IsCompleted)
                    {
                        Console.WriteLine(score);
                        return;
                    }

                    long x = task.Result;
                    long y = await output.ReceiveAsync();
                    long t = await output.ReceiveAsync();
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
                input.Post(lastMove);
            }
        }
    }
}
