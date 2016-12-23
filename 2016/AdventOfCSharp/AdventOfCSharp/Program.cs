using System;

namespace AdventOfCSharp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Day23.Solve(@"cpy a b
dec b
mul b a // cpy a d
nop cpy 0 a
nop cpy b c
nop inc a
nop dec c
nop jnz c -2
nop dec d
nop jnz d -5
dec b
cpy b c
cpy c d
dec d
inc c
jnz d -2
tgl c
cpy -16 c
jnz 1 c
cpy 75 c
jnz 88 d
inc a
inc d
jnz d -2
inc c
jnz c -5");
            Console.ReadLine();
        }
    }
}
