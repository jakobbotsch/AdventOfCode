using System;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main()
    {
        const int input = 6392;

        int bestX = 0, bestY = 0;
        for (int y = 1; y <= 300 - 2; y++)
        {
            for (int x = 1; x <= 300 - 2; x++)
            {
                if (RectSum(x, y, 3, 3) > RectSum(bestX, bestY, 3, 3))
                    (bestX, bestY) = (x, y);
            }
        }

        Console.WriteLine((bestX, bestY));

        bestX = 0;
        bestY = 0;
        int bestSize = 0;
        long bestScore = long.MinValue;
        for (int y = 1; y <= 300; y++)
        {
            for (int x = 1; x <= 300; x++)
            {
                long score = 0;
                for (int size = 1; x + (size - 1) <= 300 && y + (size - 1) <= 300; size++)
                {
                    score += RectSum(x + size - 1, y, 1, size);
                    score += RectSum(x, y + size - 1, size - 1, 1);
                    if (score > bestScore)
                        (bestX, bestY, bestSize, bestScore) = (x, y, size, score);
                }
            }
        }

        Console.WriteLine((bestX, bestY, bestSize));

        long RectSum(int sx, int sy, int width, int height)
        {
            long sum = 0;
            for (int ty = 0; ty < height; ty++)
            {
                for (int tx = 0; tx < width; tx++)
                    sum += PowerLevel(sx + tx, sy + ty);
            }

            return sum;
        }

        int PowerLevel(int x, int y)
        {
            int rackId = x + 10;
            int powerLevel = rackId * y;
            powerLevel += input;
            powerLevel *= rackId;
            powerLevel /= 100;
            powerLevel %= 10;
            return powerLevel - 5;
        }
    }
}
