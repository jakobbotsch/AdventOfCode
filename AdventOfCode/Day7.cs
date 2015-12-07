using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode
{
	internal static class Day7
	{
		internal static ushort Part1(string[] lines)
		{
			_memoized.Clear();
			return FindValue("a", lines);
		}

		private static readonly Dictionary<string, ushort> _memoized = new Dictionary<string, ushort>();
		private static ushort FindMemoized(string wire, string[] input)
		{
			if (_memoized.ContainsKey(wire))
				return _memoized[wire];

			ushort val = FindValue(wire, input);
			_memoized[wire] = val;
			return val;
		}

		private static ushort FindValue(string wire, string[] input)
		{
			for (int i = input.Length - 1; i >= 0; i--)
			{
				string line = input[i];
				if (!line.EndsWith("-> " + wire))
					continue;

				string leftSide = line.Remove(line.IndexOf(" -> "));
				string[] split = leftSide.Split(new[] {" "}, StringSplitOptions.None);

				Func<string, ushort> parseOrFind = text =>
				                                   {
					                                   ushort val;
					                                   if (ushort.TryParse(text, out val))
						                                   return val;

					                                   return FindMemoized(text, input);
				                                   };

				if (split.Length == 1)
					return parseOrFind(split[0]);

				if (split.Length == 2)
				{
					if (split[0] == "NOT")
						return (ushort)~parseOrFind(split[1]);

					throw new Exception();
				}

				if (split.Length == 3)
				{
					switch (split[1])
					{
						case "AND":
							return (ushort)(parseOrFind(split[0]) & parseOrFind(split[2]));
						case "OR":
							return (ushort)(parseOrFind(split[0]) | parseOrFind(split[2]));
						case "LSHIFT":
							return (ushort)(parseOrFind(split[0]) << parseOrFind(split[2]));
						case "RSHIFT":
							return (ushort)(parseOrFind(split[0]) >> parseOrFind(split[2]));
					}
				}

				throw new Exception();
			}

			throw new Exception("Not found");
		}

		internal static ushort Part2(string[] lines)
		{
			ushort part1Val = Part1(lines);
			_memoized.Clear();
			_memoized["b"] = part1Val;

			return FindValue("a", lines);
		}
	}
}