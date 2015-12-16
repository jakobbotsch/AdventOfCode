using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	internal static class Day16
	{
		public static int Part1(string[] lines)
		{
			Dictionary<string, int> facts = new Dictionary<string, int>
			                                {
				                                ["children"] = 3,
				                                ["cats"] = 7,
				                                ["samoyeds"] = 2,
				                                ["pomeranians"] = 3,
				                                ["akitas"] = 0,
				                                ["vizslas"] = 0,
				                                ["goldfish"] = 5,
				                                ["trees"] = 3,
				                                ["cars"] = 2,
				                                ["perfumes"] = 1,
			                                };

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i].Replace(":", "").Replace(",", "");
				string[] split = line.Split(' ');

				if (IsMatch1(facts, split.Skip(2).ToArray()))
					return i + 1;
			}

			return -1;
		}

		private static bool IsMatch1(Dictionary<string, int> facts, string[] items)
		{
			for (int i = 0; i < items.Length; i += 2)
			{
				if (facts[items[i]] != int.Parse(items[i + 1]))
					return false;
			}

			return true;
		}

		public static int Part2(string[] lines)
		{
			Dictionary<string, int> facts = new Dictionary<string, int>
			                                {
				                                ["children"] = 3,
				                                ["cats"] = 7,
				                                ["samoyeds"] = 2,
				                                ["pomeranians"] = 3,
				                                ["akitas"] = 0,
				                                ["vizslas"] = 0,
				                                ["goldfish"] = 5,
				                                ["trees"] = 3,
				                                ["cars"] = 2,
				                                ["perfumes"] = 1,
			                                };

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i].Replace(":", "").Replace(",", "");
				string[] split = line.Split(' ');

				if (IsMatch2(facts, split.Skip(2).ToArray()))
					return i + 1;
			}

			return -1;
		}

		private static bool IsMatch2(Dictionary<string, int> facts, string[] items)
		{
			for (int i = 0; i < items.Length; i += 2)
			{
				string item = items[i];
				int amount = int.Parse(items[i + 1]);

				if (item == "cats" || item == "trees")
				{
					if (amount <= facts[item])
						return false;

					continue;
				}
				if (item == "pomeranians" || item == "goldfish")
				{
					if (amount >= facts[item])
						return false;

					continue;
				}

				if (facts[item] != amount)
					return false;
			}

			return true;
		}
	}
}