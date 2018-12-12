using System;
using System.IO;
using System.Linq;
using System.Text;

public class Program
{
    public static void Main()
    {
        string[] lines = File.ReadAllLines("day12.txt");
        const int iterations = 1000;
        int @base = iterations * 2;
        string pad = new string('.', @base);
        string state = pad + lines[0].Split(' ')[2] + pad;
        (string pattern, char replacement)[] rules = lines.Skip(2).Select(l => (l.Split(' ')[0], l.Split(' ')[2][0])).ToArray();
        int prevScore = 0;
        for (int i = 0; i < iterations; i++)
        {
            if (i == 20)
                Console.WriteLine(Score());
            if (i == iterations - 1)
                prevScore = Score();

            char[] newState = state.ToCharArray();
            for (int j = 0; j < newState.Length - 4; j++)
            {
                string subst = state.Substring(j, 5);
                (_, char replacement) = rules.First(t => subst == t.pattern);
                newState[j + 2] = replacement == 0 ? '.' : replacement;
            }

            state = new string(newState);
        }

        Console.WriteLine(Score() + (Score() - prevScore) * (50_000_000_000 - 1000));

        int Score()
            => state.ToString().Select((c, i) => (c, i)).Sum(t => t.c == '#' ? t.i - @base : 0);
    }
}
