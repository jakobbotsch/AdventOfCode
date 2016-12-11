using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCSharp
{
    internal static class Day11
    {
        internal static void Solve2()
        {
            // Promethium: 0
            // Cobalt: 1
            // Curium: 2
            // Ruthenium: 3
            // Plutonium: 4
            // Elerium: 5
            // Dilithium: 6
            // First byte is microchips on floor 0, second is generators on floor 0, third is microchips on floor 1, etc.
            // Initial state:
            // Floor 1: PRG, PRM, EG, EM, DG, DM
            // Floor 2: COG, CUG, RUG, PLG
            // Floor 3: COM, CUM, RUM, PLM
            ulong initialState = 0b00000000_00000000_00000000_00011110_00011110_00000000_01100001_01100001;

            int[] indices = { 0, 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, -1 };

            Queue<State> queue = new Queue<State>();
            HashSet<State> seen = new HashSet<State>();
            queue.Enqueue(new State(0, initialState, 0));
            seen.Add(queue.Peek());

            int prevSteps = 0;
            while (queue.Count > 0)
            {
                var curState = queue.Dequeue();

                for (int idir = 0; idir < 2; idir++)
                {
                    int dir = idir == 0 ? 1 : -1;
                    if (curState.Elevator == 0 && dir == -1)
                        continue;
                    if (curState.Elevator == 3 && dir == 1)
                        continue;

                    for (int i = 0; i < indices.Length - 1; i++)
                    {
                        for (int j = i + 1; j < indices.Length; j++)
                        {
                            State? newState = curState.Move(dir, indices[i], indices[j]);
                            if (newState.HasValue && seen.Add(newState.Value))
                            {
                                if ((newState.Value.FloorItems & 0b11111111_11111111_11111111_11111111_11111111_11111111) == 0)
                                {
                                    Console.WriteLine("Steps: {0}", newState.Value.Steps);
                                    return;
                                }

                                if (newState.Value.Steps > prevSteps)
                                {
                                    prevSteps = newState.Value.Steps;
                                    Console.WriteLine(prevSteps);
                                }

                                queue.Enqueue(newState.Value);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("No more items");
        }

        private struct State
        {
            public State(int elevator, ulong floorItems, int steps)
            {
                Elevator = elevator;
                FloorItems = floorItems;
                Steps = steps;
            }

            public int Elevator { get; }
            public ulong FloorItems { get; }
            public int Steps { get; }

            public State? Move(int dir, int item1, int item2 = -1)
            {
                if (item2 != -1 &&
                    ((item1 >= 8 && item2 < 8 && item2 != item1 - 8) ||
                     (item1 < 8 && item2 >= 8 && item1 != item2 - 8)))
                {
                    // One is microchip, other is generator, and they are not of same kind
                    return null;
                }

                int newElevator = Elevator + dir;

                ulong oldBit1 = 1ul << (Elevator * 16 + item1);
                ulong oldBit2 = item2 == -1 ? 0 : 1ul << (Elevator * 16 + item2);

                ulong newBit1 = 1ul << (newElevator * 16 + item1);
                ulong newBit2 = item2 == -1 ? 1 : 1ul << (newElevator * 16 + item2);

                if ((FloorItems & oldBit1) == 0 ||
                    item2 != -1 && (FloorItems & oldBit2) == 0)
                    return null; // Items not on floor

                ulong newFloorItems = FloorItems;
                newFloorItems &= ~oldBit1;
                if (item2 != -1)
                    newFloorItems &= ~oldBit2;

                newFloorItems |= newBit1;
                if (item2 != -1)
                    newFloorItems |= newBit2;

                for (int i = 0; i < 4; i++)
                {
                    int microchips = (int)((newFloorItems >> (i * 16)) & 0xFF);
                    int generators = (int)((newFloorItems >> (i * 16 + 8)) & 0xFF);

                    // If a floor contains a generator and a chip of different kinds,
                    // the chip must be protected to avoid frying.
                    // We can remove the microchips that are protected with & ~generators.
                    int unprotected = microchips & ~generators;
                    // Now if there are any generators too, then the unprotected ones would be fried.
                    if (unprotected != 0 && generators != 0)
                        return null;
                }

                return new State(newElevator, newFloorItems, Steps + 1);
            }

            public bool Equals(State other)
            {
                return Elevator == other.Elevator && FloorItems == other.FloorItems;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != GetType())
                    return false;
                return Equals((State)obj);
            }

            public override int GetHashCode()
            {
                return unchecked((Elevator * 397) ^ FloorItems.GetHashCode());
            }
        }
    }
}
