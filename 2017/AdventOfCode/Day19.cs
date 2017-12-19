using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day19
    {
        public static void Solve(string input)
        {
            string[] map = Util.GetLines(input);
            (int x, int y) pos = (map[0].IndexOf('|'), 0);
            (int x, int y) dir = (0, 1);

            (int x, int y)[] dirs =
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1),
            };

            int count = 1;
            while (true)
            {
                if (char.IsLetter(map[pos.y][pos.x]))
                    Console.Write(map[pos.y][pos.x]);

                if (!HasPath(dir.x, dir.y))
                {
                    foreach ((int xo, int yo) in dirs)
                    {
                        if (xo == -dir.x || yo == -dir.y)
                            continue;

                        if (HasPath(xo, yo))
                        {
                            dir = (xo, yo);
                            goto FoundDir;
                        }
                    }

                    break;

                    FoundDir:;
                }

                pos = (pos.x + dir.x, pos.y + dir.y);
                count++;

                bool HasPath(int xo, int yo)
                {
                    int nx = pos.x + xo;
                    int ny = pos.y + yo;
                    return nx >= 0 && nx < map[0].Length &&
                           ny >= 0 && ny < map.Length &&
                           !char.IsWhiteSpace(map[ny][nx]);
                }
            }

            Console.WriteLine();
            Console.WriteLine(count);
        }
    }
}
