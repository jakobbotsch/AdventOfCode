using System;
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
        string[] input = File.ReadAllLines("day17.txt");
        (int xs, int ys, int xe, int ye)[] clay =
            input
            .Select(l => (l, n: ParseNumbers(l)))
            .Select(t => t.l.StartsWith("x") ? (t.n[0], t.n[1], t.n[0], t.n[2]) : (t.n[1], t.n[0], t.n[2], t.n[0]))
            .ToArray();

        int maxX = clay.Max(t => t.xe);
        int maxY = clay.Max(t => t.ye);
        char[][] map = Enumerable.Range(0, maxY + 1).Select(_ => new string('.', maxX + 3).ToCharArray()).ToArray();
        foreach ((int xs, int ys, int xe, int ye) in clay)
        {
            for (int y = ys; y <= ye; y++)
            {
                for (int x = xs; x <= xe; x++)
                    map[y][x + 1] = '#';
            }
        }

        fillRecursive(501, clay.Min(t => t.ys));
        Console.WriteLine(map.Sum(l => l.Count(c => c == '~' || c == '|')));
        Console.WriteLine(map.Sum(l => l.Count(c => c == '~')));

        void fillRecursive(int x, int y)
        {
            if (y >= map.Length)
                return;

            map[y][x] = '|';
            if (y + 1 >= map.Length)
                return;

            if (map[y + 1][x] == '.')
                fillRecursive(x, y + 1);

            bool more = true;
            while (more && canSettleAt(x, y))
            {
                more = false;

                int findFlowEnd(int dir)
                {
                    for (int i = x; ; i += dir)
                    {
                        if (map[y][i] == '#')
                            return i - dir;
                        if (!canSettleAt(i, y))
                            return i;
                    }

                    throw new Exception("Unreachable");
                }

                int spanBefore = findFlowEnd(-1);
                int spanAfter = findFlowEnd(1);

                bool canSettle = canSettleAt(spanBefore, y) && canSettleAt(spanAfter, y);

                for (int i = spanBefore; i <= spanAfter; i++)
                {
                    char c = canSettle ? '~' : '|';
                    if (map[y][i] != c)
                    {
                        map[y][i] = c;
                        more = true;
                    }
                }

                for (int i = spanBefore; i <= spanAfter; i++)
                {
                    if (map[y + 1][i] == '.')
                    {
                        Debug.Assert(!canSettle);
                        fillRecursive(i, y + 1);
                    }
                }
            }
        }

        bool canSettleAt(int x, int y) => map[y + 1][x] == '~' || map[y + 1][x] == '#';
    }
}
