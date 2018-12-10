using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        const int numPlayers = 462;
        const int lastMarble = 71938;

        foreach (int multiplier in new[] { 1, 100 })
        {
            LinkedList<long> marbles = new LinkedList<long>();
            LinkedListNode<long> cur = marbles.AddFirst(0);
            long[] scores = new long[numPlayers];
            for (long i = 1; i < lastMarble * multiplier; i++)
            {
                if (i % 23 != 0)
                {
                    LinkedListNode<long> next = cur.Next ?? marbles.First;
                    cur = marbles.AddAfter(next, i);
                    continue;
                }

                LinkedListNode<long> toRemove = cur;
                for (long j = 0; j < 7; j++)
                    toRemove = toRemove.Previous ?? marbles.Last;

                long value = i + toRemove.Value;
                scores[(i - 1) % numPlayers] += value;
                cur = toRemove.Next ?? marbles.First;
                marbles.Remove(toRemove);
            }

            Console.WriteLine(scores.Max());
        }
    }
}
