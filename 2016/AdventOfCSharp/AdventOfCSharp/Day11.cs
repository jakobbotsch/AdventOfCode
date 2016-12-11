using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCSharp
{
    internal static class Day11
    {
        internal static void Solve1()
        {
            HashSet<string>[] initialFloors =
            {
                new HashSet<string> {"PRG", "PRM" },
                new HashSet<string> {"COG", "CUG", "RUG", "PLG"},
                new HashSet<string> {"COM", "CUM", "RUM", "PLM"},
                new HashSet<string>(),
            };

            Queue<State> queue = new Queue<State>();
            HashSet<State> seen = new HashSet<State>();
            queue.Enqueue(new State(0, initialFloors, 0));
            seen.Add(queue.Peek());

            while (queue.Count > 0)
            {
                var curState = queue.Dequeue();
                if (curState.FloorItems.Take(3).All(f => f.Count <= 0))
                {
                    Console.WriteLine("Steps: {0}", curState.Steps);
                    break;
                }

                for (int idir = 0; idir < 2; idir++)
                {
                    int dir = idir == 0 ? 1 : -1;
                    if (curState.Elevator == 0 && dir == -1)
                        continue;
                    if (curState.Elevator == 3 && dir == 1)
                        continue;

                    foreach (string item1 in curState.FloorItems[curState.Elevator])
                    {
                        foreach (string item2 in curState.FloorItems[curState.Elevator].Concat(new [] {(string)null}))
                        {
                            if (item2 == item1)
                                continue;

                            State newState = curState.Move(dir, item1, item2);
                            if (newState != null && seen.Add(newState))
                                queue.Enqueue(newState);
                        }
                    }
                }
            }
        }

        private class State
        {
            public State(int elevator, HashSet<string>[] floorItems, int steps)
            {
                Elevator = elevator;
                FloorItems = floorItems;
                Steps = steps;
            }

            public int Elevator { get; }
            public HashSet<string>[] FloorItems { get; }
            public int Steps { get; }

            public State Move(int dir, string item1, string item2 = null)
            {
                if (item1 != null && item2 != null &&
                    (item1.EndsWith("G") && item2.EndsWith("M") ||
                     item1.EndsWith("M") && item2.EndsWith("G")))
                {
                    // One is microchip, other is generator. Cannot do move if they aren't the same kind.
                    if (item1.Remove(item1.Length - 1) != item2.Remove(item2.Length - 1))
                        return null;
                }

                var newFloorItems = FloorItems.Select(fi => new HashSet<string>(fi)).ToArray();
                int newElevator = Elevator + dir;
                newFloorItems[Elevator].Remove(item1);
                newFloorItems[Elevator].Remove(item2);

                if (item1 != null)
                    newFloorItems[newElevator].Add(item1);
                if (item2 != null)
                    newFloorItems[newElevator].Add(item2);

                foreach (var floor in newFloorItems)
                {
                    // If a floor contains a generator and a chip of different kinds,
                    // the chip must be protected to avoid frying.
                    foreach (string item in floor)
                    {
                        if (!item.EndsWith("M"))
                            continue;

                        bool isProtected = floor.Contains(item.Remove(item.Length - 1) + "G");
                        if (isProtected)
                            continue;

                        if (floor.Any(f => f.EndsWith("G")))
                            return null;
                    }
                }

                return new State(newElevator, newFloorItems, Steps + 1);
            }

            protected bool Equals(State other)
            {
                if (Elevator != other.Elevator)
                    return false;

                for (int i = 0; i < FloorItems.Length; i++)
                {
                    if (!FloorItems[i].SetEquals(other.FloorItems[i]))
                        return false;
                }

                return true;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != this.GetType())
                    return false;
                return Equals((State)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = (Elevator * 397) ^ FloorItems.Length;
                    foreach (var floor in FloorItems)
                    {
                        hashCode *= 397;
                        hashCode ^= floor.Count;
                        foreach (string s in floor.OrderBy(f => f))
                        {
                            hashCode *= 397;
                            hashCode ^= s.GetHashCode();
                        }
                    }

                    return hashCode;
                }
            }
        }
    }
}
