using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day19
    {
        public static async Task SolveAsync()
        {
            long[] prog = File.ReadAllText("day19.txt").Split(',').Select(long.Parse).ToArray();
            async Task<bool> IsAttractedAsync(int x, int y)
            {
                long[] mem = new long[prog.Length + 50];
                Array.Copy(prog, 0, mem, 0, prog.Length);
                Channel<long> input = Channel.CreateUnbounded<long>();
                Channel<long> output = Channel.CreateUnbounded<long>();
                await input.Writer.WriteAsync(x);
                await input.Writer.WriteAsync(y);
                await IntCode.RunAsyncWithMem(mem, input, output);
                return await output.Reader.ReadAsync() != 0;
            }

            int sum = 0;
            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    sum += await IsAttractedAsync(x, y) ? 1 : 0;
                }
            }

            Console.WriteLine(sum);

            bool FitsStartingAt(int startX, int startY)
            {
                for (int y = 0; y < 100; y++)
                {
                    for (int x = 0; x < 100; x++)
                    {
                        if (!IsAttractedAsync(startX + x, startY + y).Result)
                            return false;
                    }
                }

                return true;
            }

            int? FittingX(int startY)
            {
                int curX = 0;
                while (!IsAttractedAsync(curX, startY).Result)
                    curX++;

                while (IsAttractedAsync(curX, startY).Result)
                {
                    if (FitsStartingAt(curX, startY))
                        return curX;

                    curX++;
                }

                return null;
            }

            long? bestY = Util.BinarySearchForSmallest(y => FittingX((int)y).HasValue, 0, 10000);
            long? bestX = FittingX((int)bestY.Value);
            Console.WriteLine(bestX.Value * 10000 + bestY.Value);
        }
    }
}
