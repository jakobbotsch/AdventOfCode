using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCSharp
{
    internal static class Day19
    {
        internal static void Solve1(int input)
        {
            List<int> withPresents = Enumerable.Range(0, input).ToList();
            while (withPresents.Count > 1)
            {
                List<int> next = new List<int>(withPresents.Count / 2);

                for (int i = withPresents.Count % 2 == 0 ? 0 : 2; i < withPresents.Count; i += 2)
                    next.Add(withPresents[i]);

                withPresents = next;
            }

            Console.WriteLine("Elf: {0}", withPresents.Single() + 1);
        }

        internal static void Solve2(int input)
        {
            LinkedList<int> elves = new LinkedList<int>(Enumerable.Range(0, input));

            LinkedListNode<int> opposite = elves.First;
            for (int i = 0; i < elves.Count / 2; i++)
                opposite = opposite.Next;

            while (elves.Count > 1)
            {
                LinkedListNode<int> next = opposite.Next ?? elves.First;
                if (elves.Count % 2 != 0)
                    next = next.Next ?? elves.First;

                elves.Remove(opposite);

                opposite = next;
            }

            Console.WriteLine("Elf: {0}", elves.First.Value + 1);
        }
    }
}
