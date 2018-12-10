using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        int[][] input = File.ReadAllLines("day10.txt").Select(ParseNumbers).ToArray();
        (int x, int y)[] particlePosisions = input.Select(p => (p[0], p[1])).ToArray();
        for (int i = 0; ; i++)
        {
            int minX = particlePosisions.Select(p => p.x).Min();
            int minY = particlePosisions.Select(p => p.y).Min();
            int maxX = particlePosisions.Select(p => p.x).Max();
            int maxY = particlePosisions.Select(p => p.y).Max();

            if (maxX - minX < 200)
            {
                Console.WriteLine(i);
                string filler = new string('.', maxX - minX + 1);
                StringBuilder[] screen = Enumerable.Range(0, maxY - minY + 1).Select(_ => new StringBuilder(filler)).ToArray();
                foreach ((int x, int y) in particlePosisions)
                    screen[y - minY][x - minX] = '#';

                foreach (StringBuilder line in screen)
                    Console.WriteLine(line);
            }

            for (int j = 0; j < particlePosisions.Length; j++)
            {
                particlePosisions[j].x += input[j][2];
                particlePosisions[j].y += input[j][3];
            }
        }
    }

    private static int[] ParseNumbers(string line)
        => Regex.Matches(line, "(-)?\\d+").Select(m => int.Parse(m.Value)).ToArray();
}
