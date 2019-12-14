using System;

namespace AdventOfCode
{
    internal static class Util
    {
        public static long BinarySearchForLargest(Func<long, bool> sat, long min = long.MinValue, long max = long.MaxValue)
        {
            // Result is in [min, max]
            while (min < max)
            {
                long mid = (long)((ulong)max - (ulong)(max - min) / 2);
                if (sat(mid))
                    min = mid;
                else
                    max = mid - 1;
            }

            return min;
        }

        public static long BinarySearchForSmallest(Func<long, bool> sat, long min = long.MinValue, long max = long.MaxValue)
        {
            // Result is in [min, max]
            while (min < max)
            {
                long mid = (long)((ulong)min + (ulong)(max - min) / 2);
                if (sat(mid))
                    max = mid;
                else
                    min = mid + 1;
            }

            return max;
        }
    }
}
