using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class Program
{
    private static int[] ParseNumbers(string line)
        => Regex.Matches(line, "\\d+").Select(m => int.Parse(m.Value)).ToArray();

    public static void Main()
    {
        string[] lines = File.ReadAllLines("day16.txt");
        int index;
        List<(int[] before, int[] inst, int[] after)> stateUpdates = new List<(int[] before, int[] inst, int[] after)>();
        for (index = 0; lines[index].Contains("Before"); index += 4)
            stateUpdates.Add((ParseNumbers(lines[index]), ParseNumbers(lines[index + 1]), ParseNumbers(lines[index + 2])));

        index += 2;
        List<int[]> program = new List<int[]>();
        for (; index < lines.Length; index++)
            program.Add(ParseNumbers(lines[index]));

        var operations = new Action<int[], int[]>[]
        {
            (state, inst) => state[inst[3]] = state[inst[1]] + state[inst[2]], // addr
            (state, inst) => state[inst[3]] = state[inst[1]] + inst[2], // addi
            (state, inst) => state[inst[3]] = state[inst[1]] * state[inst[2]], // mulr
            (state, inst) => state[inst[3]] = state[inst[1]] * inst[2], // muli
            (state, inst) => state[inst[3]] = state[inst[1]] & state[inst[2]], // banr
            (state, inst) => state[inst[3]] = state[inst[1]] & inst[2], // bani
            (state, inst) => state[inst[3]] = state[inst[1]] | state[inst[2]], // borr
            (state, inst) => state[inst[3]] = state[inst[1]] | inst[2], // bori
            (state, inst) => state[inst[3]] = state[inst[1]], // setr
            (state, inst) => state[inst[3]] = inst[1], // seti
            (state, inst) => state[inst[3]] = inst[1] > state[inst[2]] ? 1 : 0, // gtir
            (state, inst) => state[inst[3]] = state[inst[1]] > inst[2] ? 1 : 0, // gtri
            (state, inst) => state[inst[3]] = state[inst[1]] > state[inst[2]] ? 1 : 0, // gtrr
            (state, inst) => state[inst[3]] = inst[1] == state[inst[2]] ? 1 : 0, // eqir
            (state, inst) => state[inst[3]] = state[inst[1]] == inst[2] ? 1 : 0, // eqri
            (state, inst) => state[inst[3]] = state[inst[1]] == state[inst[2]] ? 1 : 0, // eqrr
        };

        int numMoreThan3 = 0;
        Dictionary<int, HashSet<int>> opcodeToOpIndex = new Dictionary<int, HashSet<int>>();
        foreach ((int[] before, int[] inst, int[] after) in stateUpdates)
        {
            int matches = 0;
            for (int i = 0; i < operations.Length; i++)
            {
                int[] state = before.ToArray();
                operations[i](state, inst);
                if (state.SequenceEqual(after))
                {
                    if (!opcodeToOpIndex.TryGetValue(inst[0], out HashSet<int> opIndices))
                        opcodeToOpIndex[inst[0]] = opIndices = new HashSet<int>();
                    opIndices.Add(i);
                    matches++;
                }
            }

            if (matches >= 3)
                numMoreThan3++;
        }

        Console.WriteLine(numMoreThan3);

        int[] opMap = new int[16];
        while (opcodeToOpIndex.Any(kvp => kvp.Value.Count > 0))
        {
            var entry = opcodeToOpIndex.First(kvp => kvp.Value.Count == 1);
            int opIndex = entry.Value.First();
            opMap[entry.Key] = opIndex;
            foreach (HashSet<int> remaining in opcodeToOpIndex.Values)
                remaining.Remove(opIndex);
        }

        int[] programState = { 0, 0, 0, 0 };
        foreach (int[] inst in program)
            operations[opMap[inst[0]]](programState, inst);

        Console.WriteLine(programState[0]);
    }
}
