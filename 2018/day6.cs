using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main()
    {
        (int x, int y)[] coords =
            File.ReadAllLines(@"day6.txt")
            .Select(s => (int.Parse(s.Split(", ")[0]), int.Parse(s.Split(", ")[1]))).ToArray();

        int minX = coords.Select(t => t.x).Min();
        int minY = coords.Select(t => t.y).Min();
        int maxX = coords.Select(t => t.x).Max();
        int maxY = coords.Select(t => t.y).Max();

        Dictionary<(int x, int y), int> map = new Dictionary<(int, int), int>();
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                var distances = coords.Select((t, i) => (i, d: Math.Abs(t.x - x) + Math.Abs(t.y - y))).OrderBy(t => t.d).ToList();
                if (distances[0].d == distances[1].d)
                    map[(x, y)] = -1;
                else
                    map[(x, y)] = distances[0].i;
            }
        }

        List<int> badIds = map
            .Where(kvp => kvp.Key.x == minX || kvp.Key.x == maxX || kvp.Key.y == minY || kvp.Key.y == maxY)
            .Select(kvp => kvp.Value)
            .Distinct()
            .ToList();

        Console.WriteLine("{0}", map.Values.Except(badIds).Distinct().Select(id => map.Values.Count(v => v == id)).Max());

        int xBase = minX - 10000 / coords.Length - 1;
        int yBase = minY - 10000 / coords.Length - 1;
        int xEnd = maxX + 10000 / coords.Length + 1;
        int yEnd = maxY + 10000 / coords.Length + 1;

        int count = 0;
        for (int y = yBase; y <= yEnd; y++)
        {
            for (int x = xBase; x <= xEnd; x++)
            {
                int distance = 0;
                foreach ((int cx, int cy) in coords)
                {
                    distance += Math.Abs(cx - x) + Math.Abs(cy - y);
                    if (distance >= 10000)
                        goto NextCoord;
                }

                count++;

                NextCoord:
                ;
            }
        }

        Console.WriteLine("{0}", count);
    }
}
