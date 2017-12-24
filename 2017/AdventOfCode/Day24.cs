using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day24
    {
        public static void Solve(string input)
        {
            Random rand = new Random();
            State initialState = new State
            {
                Last = 0,
                Length = 0,
                PortsLeft = Util.GetLines(input).Select(Util.GetInts).ToList(),
                Sum = 0,
            };
            var stack = new Stack<State>();
            stack.Push(initialState);
            State strongest = initialState;
            State longest = initialState;
            while (stack.Count > 0)
            {
                State curState = stack.Pop();

                if (curState.Sum > strongest.Sum)
                    strongest = curState;
                if (curState.Length > longest.Length || curState.Length == longest.Length && curState.Sum > longest.Sum)
                    longest = curState;

                var valid = curState.PortsLeft.Where(v => v[0] == curState.Last || v[1] == curState.Last);
                foreach (int[] port in valid)
                {
                    var newPortsLeft = curState.PortsLeft.ToList();
                    newPortsLeft.Remove(port);
                    var newState = new State
                    {
                        Last = port[0] == curState.Last ? port[1] : port[0],
                        Length = curState.Length + 1,
                        PortsLeft = newPortsLeft,
                        Sum = curState.Sum + port[0] + port[1]
                    };

                    stack.Push(newState);
                }
            }

            Console.WriteLine("Strongest: {0}", strongest.Sum);
            Console.WriteLine("Longest: {0}", longest.Sum);
        }

        private class State
        {
            public List<int[]> PortsLeft { get; set; }
            public int Last { get; set; }
            public int Length { get; set; }
            public int Sum { get; set; }
        }
    }
}
