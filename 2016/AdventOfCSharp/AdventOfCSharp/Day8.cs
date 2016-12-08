using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCSharp
{
    internal static class Day8
    {
        internal static void Solve1(string input)
        {
            const int bwidth = 50;
            const int bheight = 6;
            bool[,] pixels = new bool[bheight, bwidth];
            foreach (string line in input.Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                var rect = Regex.Match(line, "rect (?<x>\\d+)x(?<y>\\d+)");
                var rotCol = Regex.Match(line, "rotate column x=(?<x>\\d+) by (?<amount>\\d+)");
                var rotRow = Regex.Match(line, "rotate row y=(?<y>\\d+) by (?<amount>\\d+)");

                if (rect.Success)
                {
                    int width = int.Parse(rect.Groups["x"].Value);
                    int height = int.Parse(rect.Groups["y"].Value);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                            pixels[y, x] = true;
                    }
                }
                else if (rotCol.Success)
                {
                    int x = int.Parse(rotCol.Groups["x"].Value);
                    int amount = int.Parse(rotCol.Groups["amount"].Value);
                    bool[] newCol = new bool[bheight];
                    for (int i = 0; i < bheight; i++)
                    {
                        int y = i - amount;
                        while (y < 0)
                            y += bheight;

                        newCol[i] = pixels[y, x];
                    }

                    for (int y = 0; y < bheight; y++)
                        pixels[y, x] = newCol[y];
                }
                else if (rotRow.Success)
                {
                    int y = int.Parse(rotRow.Groups["y"].Value);
                    int amount = int.Parse(rotRow.Groups["amount"].Value);
                    bool[] newRow = new bool[bwidth];
                    for (int i = 0; i < bwidth; i++)
                    {
                        int x = i - amount;
                        while (x < 0)
                            x += bwidth;

                        newRow[i] = pixels[y, x];
                    }

                    for (int x = 0; x < bwidth; x++)
                        pixels[y, x] = newRow[x];
                }
            }

            Console.WriteLine("{0}", pixels.OfType<bool>().Count(b => b));
        }

        internal static void Solve2(string input)
        {
            const int bwidth = 50;
            const int bheight = 6;
            bool[,] pixels = new bool[bheight, bwidth];
            foreach (string line in input.Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                var rect = Regex.Match(line, "rect (?<x>\\d+)x(?<y>\\d+)");
                var rotCol = Regex.Match(line, "rotate column x=(?<x>\\d+) by (?<amount>\\d+)");
                var rotRow = Regex.Match(line, "rotate row y=(?<y>\\d+) by (?<amount>\\d+)");

                if (rect.Success)
                {
                    int width = int.Parse(rect.Groups["x"].Value);
                    int height = int.Parse(rect.Groups["y"].Value);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                            pixels[y, x] = true;
                    }
                }
                else if (rotCol.Success)
                {
                    int x = int.Parse(rotCol.Groups["x"].Value);
                    int amount = int.Parse(rotCol.Groups["amount"].Value);
                    bool[] newCol = new bool[bheight];
                    for (int i = 0; i < bheight; i++)
                    {
                        int y = i - amount;
                        while (y < 0)
                            y += bheight;

                        newCol[i] = pixels[y, x];
                    }

                    for (int y = 0; y < bheight; y++)
                        pixels[y, x] = newCol[y];
                }
                else if (rotRow.Success)
                {
                    int y = int.Parse(rotRow.Groups["y"].Value);
                    int amount = int.Parse(rotRow.Groups["amount"].Value);
                    bool[] newRow = new bool[bwidth];
                    for (int i = 0; i < bwidth; i++)
                    {
                        int x = i - amount;
                        while (x < 0)
                            x += bwidth;

                        newRow[i] = pixels[y, x];
                    }

                    for (int x = 0; x < bwidth; x++)
                        pixels[y, x] = newRow[x];
                }
            }

            for (int y = 0; y < bheight; y++)
            {
                for (int x = 0; x < bwidth; x++)
                {
                    Console.Write(pixels[y, x] ? '#' : '.');
                }

                Console.WriteLine();
            }
        }
    }
}
