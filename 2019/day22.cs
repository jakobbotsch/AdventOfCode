using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
    internal static class Day22
    {
        public static void Solve()
        {
            string[] lines = File.ReadAllLines("day22.txt");
//            string[] lines = @"deal into new stack
//cut -2
//deal with increment 7
//cut 8
//cut -4
//deal with increment 7
//cut 3
//deal with increment 9
//deal with increment 3
//cut -1".Split(Environment.NewLine);

            int[] deck = Enumerable.Range(0, 10007).ToArray();
            int[] newDeck = new int[deck.Length];
            foreach (string s in lines)
            {
                if (s == "deal into new stack")
                {
                    Array.Reverse(deck);
                    continue;
                }

                if (s.StartsWith("cut "))
                {
                    int cut = int.Parse(s.Substring(4));
                    if (cut < 0)
                        cut = deck.Length + cut;

                    Array.Copy(deck, 0, newDeck, newDeck.Length - cut, cut);
                    Array.Copy(deck, cut, newDeck, 0, deck.Length - cut);

                    Swap(ref deck, ref newDeck);
                    continue;
                }

                if (s.StartsWith("deal with increment "))
                {
                    int inc = int.Parse(s.Substring("deal with increment ".Length));
                    int putIndex = 0;
                    for (int i = 0; i < deck.Length; i++)
                    {
                        newDeck[putIndex] = deck[i];
                        putIndex += inc;
                        if (putIndex >= newDeck.Length)
                            putIndex -= newDeck.Length;
                    }

                    Swap(ref deck, ref newDeck);
                    continue;
                }

                throw new Exception("wtf");
            }

            if (deck.Length < 100)
                Console.WriteLine(string.Join(" ", deck));
            if (deck.Length > 2019)
                Console.WriteLine(Array.IndexOf(deck, 2019));

            long numCards = 119315717514047;
            List<Func<long, long>> ops = new List<Func<long, long>>();
            Array.Reverse(lines);
            string expr = "pos";
            foreach (string s in lines)
            {
                if (s == "deal into new stack")
                {
                    ops.Add(pos => numCards - 1 - pos);
                    expr = $"(({expr} + 1) * (-1))";
                    continue;
                }

                if (s.StartsWith("cut "))
                {
                    long num = long.Parse(s.Substring("cut ".Length));
                    expr = $"({expr} - ({num}))";
                    if (num < 0)
                        num = numCards + num;

                    ops.Add(pos => pos < num ? numCards - num + pos : pos - num);
                    continue;
                }

                if (s.StartsWith("deal with increment "))
                {
                    int inc = int.Parse(s.Substring("deal with increment ".Length));
                    // Solve prevPos * inc = pos (mod numCards)
                    ops.Add(pos => Invert(inc, pos, numCards));
                    expr = $"({expr} * {inc})";
                    continue;
                }

                throw new Exception("wtf");
            }

            long pos = 2020;
            Dictionary<long, long> posAtIndex = new Dictionary<long, long>();
            for (long i = 0; i < 101741582076661; i++)
            {
                if (posAtIndex.TryGetValue(pos, out long prev))
                {
                    Console.WriteLine("Found cycle: card is at {0} both in cycle {1} and {2}", pos, prev, i);
                    long skip = i - prev;
                    while (i + skip < 101741582076661)
                        i += skip;

                    Console.WriteLine("Skipped to {0}", i);
                }
                else
                {
                    posAtIndex.Add(pos, i);
                }

                foreach (var op in ops)
                    pos = op(pos);
            }

            Console.WriteLine(pos);
        }

        private static void Swap<T>(ref T v1, ref T v2)
        {
            T temp = v1;
            v1 = v2;
            v2 = temp;
        }

        // Given z * have1 = have2 (mod mod), compute z
        private static long Invert(long have1, long have2, long mod)
        {
            (long x, long y, long gcd) = GcdExtended(have1, mod);
            Trace.Assert(gcd == 1);
            // x*have1 + y*mod = 1
            Trace.Assert((BigInteger)x * (BigInteger)have1 + (BigInteger)y * (BigInteger)mod == 1);
            // so x*have1 = 1
            // so x is inverse
            long z = (long)(((BigInteger)have2 * (BigInteger)x) % mod);
            if (z < 0)
                z += mod;
            Trace.Assert(z >= 0 && z <= mod && (BigInteger)z * (BigInteger)have1 % (BigInteger)mod == (BigInteger)have2);

            return z;
        }

        // return x,y,gcd so that x*a + y*b = gcd
        private static (long x, long y, long gcd) GcdExtended(long a, long b)  
        {  
            // Base Case  
            if (a == 0)  
            {
                return (0, 1, b);
            }  
          
            (long x1, long y1, long gcd) = GcdExtended(b % a, a);
            return (y1 - b / a * x1, x1, gcd);
        }  
    }
}
