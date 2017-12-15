using System;

namespace AdventOfCode
{
    internal static class Day15
    {
        public static void Solve(int a, int b)
        {
            long prevA = a;
            long prevB = b;

            int count = 0;
            for (int i = 0; i < 40000000; i++)
            {
                prevA = (prevA * 16807) % int.MaxValue;
                prevB = (prevB * 48271) % int.MaxValue;
                if ((prevA & 0xffff) == (prevB & 0xffff))
                    count++;
            }
            Console.WriteLine(count);

            prevA = a;
            prevB = b;
            count = 0;
            for (int i = 0; i < 5000000; i++)
            {
                do
                {
                    prevA = (prevA * 16807) % int.MaxValue;
                } while (prevA % 4 != 0);

                do
                {
                    prevB = (prevB * 48271) % int.MaxValue;
                } while (prevB % 8 != 0);

                if ((prevA & 0xffff) == (prevB & 0xffff))
                    count++;
            }

            Console.WriteLine(count);
        }
    }
}
