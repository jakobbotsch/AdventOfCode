using System;

namespace AdventOfCode
{
    internal static class Util
    {
        public static long? BinarySearchForLargest(Func<long, bool> pred, long min = long.MinValue, long max = long.MaxValue)
        {
            // Result is in [min, max]
            while (min < max)
            {
                long mid = (long)((ulong)max - (ulong)(max - min) / 2);
                if (pred(mid))
                    min = mid;
                else
                    max = mid - 1;
            }

            return pred(min) ? (long?)min : null;
        }

        public static long? BinarySearchForSmallest(Func<long, bool> pred, long min = long.MinValue, long max = long.MaxValue)
        {
            // Result is in [min, max]
            while (min < max)
            {
                long mid = (long)((ulong)min + (ulong)(max - min) / 2);
                if (pred(mid))
                    max = mid;
                else
                    min = mid + 1;
            }

            return pred(max) ? (long?)max : null;
        }
    }
}
