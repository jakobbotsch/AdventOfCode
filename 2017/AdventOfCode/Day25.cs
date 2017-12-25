using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day25
    {
        public static void Solve(string input)
        {
            string[] lines = Util.GetLines(input);
            string startState = Regex.Match(lines[0], "Begin in state (?<start>.*)\\.").Groups["start"].Value;
            int numSteps = Util.GetInts(lines[1]).Single();
            var transitions = new Dictionary<(string, int), Transition>();

            for (int i = 3; i < lines.Length; i += 10)
            {
                string state = Regex.Match(lines[i], "In state (?<state>.*):").Groups["state"].Value;
                for (int j = i + 1; j < i + 6; j += 4)
                {
                    int curVal = Util.GetInts(lines[j]).Single();
                    int tapeValue = Util.GetInts(lines[j + 1]).Single();
                    int move = lines[j + 2].Contains("right") ? 1 : -1;
                    string nextState = Regex.Match(lines[j + 3], "Continue with state (?<state>.*)\\.").Groups["state"].Value;

                    transitions[(state, curVal)] = new Transition(tapeValue, move, nextState);
                }
            }

            string curState = startState;
            List<int> tape = new List<int> { 0 };
            int tapePos = 0;
            for (int i = 0; i < numSteps; i++)
            {
                int val = tape[tapePos];
                Transition t = transitions[(curState, val)];
                tape[tapePos] = t.TapeValue;
                tapePos += t.Move;
                if (tapePos >= tape.Count)
                    tape.Add(0);
                else if (tapePos < 0)
                {
                    tape.Insert(0, 0);
                    tapePos++;
                }
                curState = t.NextState;
            }

            Console.WriteLine(tape.Sum());
        }

        private class Transition
        {
            public Transition(int tapeValue, int move, string nextState)
            {
                TapeValue = tapeValue;
                Move = move;
                NextState = nextState;
            }
            public int TapeValue { get; set; }
            public int Move { get; set; }
            public string NextState { get; set; }
        }
    }
}
