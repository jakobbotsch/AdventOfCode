using System;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal static class Day1
    {
        public static void Solve()
        {
            int Part1(int i) => i / 3 - 2;
            Console.WriteLine(File.ReadAllLines("day1.txt").Select(int.Parse).Sum(Part1));
            int Part2(int i)
            {
                int totalFuel = 0;
                int fuel = i / 3 - 2;
                while (fuel > 0)
                {
                    totalFuel += fuel;
                    fuel = fuel / 3 - 2;
                }

                return totalFuel;
            }
            Console.WriteLine(File.ReadAllLines("day1.txt").Select(int.Parse).Sum(Part2));
        }
    }
}
