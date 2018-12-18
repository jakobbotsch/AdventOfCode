using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        string[] input = File.ReadAllLines("day18.txt");
        char[][] map = input.Select(l => l.ToCharArray()).ToArray();

        (int trees, int lumberyards) CountAround(int x, int y)
        {
            int trees = 0, lumberyards = 0;
            for (int yo = -1; yo <= 1; yo++)
            {
                for (int xo = -1; xo <= 1; xo += yo == 0 ? 2 : 1)
                {
                    int xc = x + xo;
                    int yc = y + yo;
                    if (yc < 0 || yc >= map.Length || xc < 0 || xc >= map[yc].Length)
                        continue;

                    trees += map[yc][xc] == '|' ? 1 : 0;
                    lumberyards += map[yc][xc] == '#' ? 1 : 0;
                }
            }

            return (trees, lumberyards);
        }

        void UpdateMap()
        {
            char[][] newMap = map.Select(l => l.ToArray()).ToArray();
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    (int trees, int lumberyards) = CountAround(x, y);
                    switch (map[y][x])
                    {
                        case '.': newMap[y][x] = trees >= 3 ? '|' : map[y][x]; break;
                        case '|': newMap[y][x] = lumberyards >= 3 ? '#' : map[y][x]; break;
                        case '#': newMap[y][x] = lumberyards >= 1 && trees >= 1 ? '#' : '.'; break;
                    }
                }
            }

            map = newMap;
        }

        Dictionary<string, int> stateToSeenIndex = new Dictionary<string, int>();
        for (int i = 0; ; i++)
        {
            string state = string.Join(Environment.NewLine, map.Select(l => new string(l)));
            if (stateToSeenIndex.TryGetValue(state, out int seenIndex))
            {
                int period = i - seenIndex;
                i += (1000000000 - i) / period * period;
                while (i++ != 1000000000)
                    UpdateMap();

                Console.WriteLine(map.Sum(m => m.Count(c => c == '|')) * map.Sum(m => m.Count(c => c == '#')));
                break;
            }

            stateToSeenIndex.Add(state, i);

            UpdateMap();

            if (i == 10)
                Console.WriteLine(map.Sum(m => m.Count(c => c == '|')) * map.Sum(m => m.Count(c => c == '#')));
        }
    }
}
