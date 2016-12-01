using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
	internal static class Day3
	{
		internal static int Part1(string s)
		{
			HashSet<Point> gifts = new HashSet<Point>();
			gifts.Add(Point.Empty);
			Point curPointSanta = Point.Empty;

			for (int i = 0; i < s.Length; i++)
			{
				Func<Point, char, Point> getNext = (cur, c) =>
				                                   {
					                                   if (c == '^')
						                                   return new Point(cur.X, cur.Y + 1);
					                                   if (c == '>')
						                                   return new Point(cur.X + 1, cur.Y);
					                                   if (c == 'v')
						                                   return new Point(cur.X, cur.Y - 1);
					                                   if (c == '<')
						                                   return new Point(cur.X - 1, cur.Y);

					                                   throw new Exception();
				                                   };
				curPointSanta = getNext(curPointSanta, s[i]);
				gifts.Add(curPointSanta);
			}

			return gifts.Count;
		}

		internal static int Part2(string s)
		{
			HashSet<Point> gifts = new HashSet<Point>();
			gifts.Add(Point.Empty);
			Point curPointSanta = Point.Empty;
			Point curPointRobo = Point.Empty;

			for (int i = 0; i < s.Length; i++)
			{
				Func<Point, char, Point> getNext = (cur, c) =>
				                                   {
					                                   if (c == '^')
						                                   return new Point(cur.X, cur.Y + 1);
					                                   if (c == '>')
						                                   return new Point(cur.X + 1, cur.Y);
					                                   if (c == 'v')
						                                   return new Point(cur.X, cur.Y - 1);
					                                   if (c == '<')
						                                   return new Point(cur.X - 1, cur.Y);

					                                   throw new Exception();
				                                   };
				if (i%2 == 0)
				{
					curPointSanta = getNext(curPointSanta, s[i]);
					gifts.Add(curPointSanta);
				}
				else
				{
					curPointRobo = getNext(curPointRobo, s[i]);
					gifts.Add(curPointRobo);
				}
			}

			return gifts.Count;
		}
	}
}