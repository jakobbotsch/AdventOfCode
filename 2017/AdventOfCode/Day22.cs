using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day22
    {
        public static void Solve(string input)
        {
            HashSet<(int x, int y)> infected = new HashSet<(int x, int y)>();
            Dictionary<(int x, int y), int> states = new Dictionary<(int x, int y), int>();
            string[] lines = Util.GetLines(input);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        infected.Add((x, y));
                        states.Add((x, y), 2);
                    }
                }
            }

            int curX = lines[0].Length / 2;
            int curY = lines.Length / 2;
            int dirX = 0;
            int dirY = -1;
            int numInf = 0;
            for (int i = 0; i < 10000; i++)
            {
                if (infected.Contains((curX, curY)))
                    (dirX, dirY) = (-dirY, dirX);
                else
                    (dirX, dirY) = (dirY, -dirX);

                if (infected.Contains((curX, curY)))
                    infected.Remove((curX, curY));
                else
                {
                    numInf++;
                    infected.Add((curX, curY));
                }

                curX += dirX;
                curY += dirY;
            }
            Console.WriteLine(numInf);

            curX = lines[0].Length / 2;
            curY = lines.Length / 2;
            dirX = 0;
            dirY = -1;
            numInf = 0;
            for (int i = 0; i < 10000000; i++)
            {
                states.TryGetValue((curX, curY), out int state);
                state %= 4;
                if (state == 0) // clean
                    (dirX, dirY) = (dirY, -dirX);
                else if (state == 2) // infected
                    (dirX, dirY) = (-dirY, dirX);
                else if (state == 3) // flagged
                    (dirX, dirY) = (-dirX, -dirY);

                states[(curX, curY)] = ++state;
                if (state == 2)
                    numInf++;

                curX += dirX;
                curY += dirY;
            }

            Console.WriteLine(numInf);
        }
    }
}
