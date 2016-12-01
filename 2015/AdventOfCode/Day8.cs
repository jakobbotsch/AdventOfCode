using System;
using System.Linq;

namespace AdventOfCode
{
	internal static class Day8
	{
		internal static int Part1(string[] lines)
		{
			int chars = 0;
			int code = 0;

			foreach (string line in lines)
			{
				for (int i = 1; i < line.Length - 1;)
				{
					string ss = line.Substring(i);
					if (ss.StartsWith("\\x"))
					{
						chars++;
						i += 4;
						continue;
					}

					if (ss.StartsWith("\\") || ss.StartsWith("\""))
					{
						chars++;
						i += 2;
						continue;
					}

					chars++;
					i++;
				}

				code += line.Length;
			}

			return code - chars;
		}

		internal static int Part2(string[] lines)
		{
			return lines.Select(l => l.Replace("\\", "\\\\").Replace("\"", "\\\"").Length + 2).Sum() - lines.Sum(l => l.Length);
		}
	}
}