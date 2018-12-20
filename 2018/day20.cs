using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class Program
{
    private static int[] ParseNumbers(string line)
        => Regex.Matches(line, "\\d+").Select(m => int.Parse(m.Value)).ToArray();

    public static void Main()
    {
        string input = File.ReadAllText("day20.txt");
        input = input.Substring(1, input.Length - 2);

        Dictionary<(int x, int y), char> map = new Dictionary<(int x, int y), char>();
        HashSet<(int x, int y, string remaining)> seen = new HashSet<(int x, int y, string remaining)>();
        Queue<(int x, int y, string remaining)> queue = new Queue<(int x, int y, string remaining)>();
        queue.Enqueue((0, 0, input));
        while (queue.Count > 0)
        {
            (int x, int y, string remaining) = queue.Dequeue();
            for (int i = 0; i < remaining.Length; i++)
            {
                if (remaining[i] == '(')
                {
                    (List<string> alts, string suffix) = SplitRegex(remaining.Substring(i));
                    foreach (string alt in alts)
                    {
                        if (seen.Add((x, y, alt + suffix)))
                            queue.Enqueue((x, y, alt + suffix));
                    }
                    break;
                }

                (int dx, int dy) = (0, 0);

                switch (remaining[i])
                {
                    case 'N': dy--; break;
                    case 'W': dx--; break;
                    case 'S': dy++; break;
                    case 'E': dx++; break;
                    default: throw new Exception("Unreachable");
                }

                map[(x + dx, y + dy)] = dx != 0 ? '|' : '-';

                x += dx * 2;
                y += dy * 2;
                map[(x, y)] = '.';
            }
        }

        Bfs(map);
    }

    private static (int dx, int dy)[] s_neis = { (0, -1), (-1, 0), (0, 1), (1, 0) };
    private static void Bfs(Dictionary<(int x, int y), char> map)
    {
        int max = 0;
        (int x, int y) = (0, 0);
        Queue<(int x, int y, int numDoors)> queue = new Queue<(int x, int y, int numDoors)>();
        HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
        queue.Enqueue((0, 0, 0));
        visited.Add((0, 0));
        int count = 0;
        while (queue.Count > 0)
        {
            int numDoors;
            (x, y, numDoors) = queue.Dequeue();
            max = numDoors;
            if (numDoors >= 1000)
                count++;

            foreach ((int dx, int dy) in s_neis)
            {
                if (!map.ContainsKey((x + dx, y + dy)))
                    continue;

                if (!visited.Add((x + dx * 2, y + dy * 2)))
                    continue;

                queue.Enqueue((x + dx * 2, y + dy * 2, numDoors + 1));
            }
        }

        Console.WriteLine(max);
        Console.WriteLine(count);
    }

    private static (List<string> alternatives, string suffix) SplitRegex(string regex)
    {
        Debug.Assert(regex[0] == '(');
        int numParens = 0;
        List<int> splitLocs = new List<int> { 0 };
        int closePos;
        for (closePos = 0; ; closePos++)
        {
            if (regex[closePos] == '(')
                numParens++;
            if (regex[closePos] == ')')
            {
                numParens--;
                if (numParens == 0)
                    break;
            }
            if (regex[closePos] == '|' && numParens == 1)
                splitLocs.Add(closePos);
        }
        splitLocs.Add(closePos);

        List<string> alternatives = new List<string>();
        for (int i = 0; i < splitLocs.Count - 1; i++)
            alternatives.Add(regex.Substring(splitLocs[i] + 1, splitLocs[i + 1] - splitLocs[i] - 1));

        return (alternatives, regex.Substring(closePos + 1));
    }
}
