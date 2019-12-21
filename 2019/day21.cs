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
    internal static class Day21
    {
        public static async Task SolveAsync()
        {
            long[] prog = File.ReadAllText("day21.txt").Split(',').Select(long.Parse).ToArray();
            var input = Channel.CreateUnbounded<long>();
            var output = Channel.CreateUnbounded<long>();
            async Task WriteLineAsync(string s)
            {
                foreach (char c in s)
                    await input.Writer.WriteAsync(c);
                await input.Writer.WriteAsync('\n');
            }

            await WriteLineAsync("NOT T T");
            await WriteLineAsync("AND A T");
            await WriteLineAsync("AND B T");
            await WriteLineAsync("AND C T");
            await WriteLineAsync("AND D T");
            await WriteLineAsync("NOT T J");
            await WriteLineAsync("AND D J");
            await WriteLineAsync("WALK");
            await IntCode.RunAsync(prog, input, output);

            await foreach (long l in output.Reader.ReadAllAsync())
            {
                if (l < 256)
                    Console.Write((char)l);
                else
                    Console.WriteLine(l);
            }

            input = Channel.CreateUnbounded<long>();
            output = Channel.CreateUnbounded<long>();
            // T = D is ground and there is a hole in A,B,C,D
            await WriteLineAsync("NOT T T");
            await WriteLineAsync("AND A T");
            await WriteLineAsync("AND B T");
            await WriteLineAsync("AND C T");
            await WriteLineAsync("AND D T");
            await WriteLineAsync("NOT T T");
            await WriteLineAsync("AND D T"); 

            // J = 5 and 9 are ground
            await WriteLineAsync("OR E J");
            await WriteLineAsync("AND I J");
            // Or if 8 is ground
            await WriteLineAsync("OR H J");
            // Jump if T and (8 is ground or 5 and 9 are round)
            await WriteLineAsync("AND T J"); 

            // Always jump if there is a hole in front of us
            await WriteLineAsync("NOT A T");

            await WriteLineAsync("OR T J");
            await WriteLineAsync("RUN");
            await IntCode.RunAsync(prog, input, output);
            await foreach (long l in output.Reader.ReadAllAsync())
            {
                if (l < 256)
                    Console.Write((char)l);
                else
                    Console.WriteLine(l);
            }
        }
    }
}
