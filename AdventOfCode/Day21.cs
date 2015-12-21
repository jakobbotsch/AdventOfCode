using System;

namespace AdventOfCode
{
	internal static class Day21
	{
		public static int Part1()
		{
			int best = int.MaxValue;
			for (int i = 0; i < 1 << 16; i++)
			{
				int weapon = i & 0x1F;
				int armor = i & (0x1F << 5);
				int rings = i & (0x3F << 10);

				if (BitCount(weapon) != 1)
					continue;

				if (BitCount(armor) > 1)
					continue;

				if (BitCount(rings) > 2)
					continue;

				Trial(ref best, i, false);
			}

			return best;
		}

		private static readonly Item[] s_items =
		{
			new Item(8, 4, 0),
			new Item(10, 5, 0),
			new Item(25, 6, 0),
			new Item(40, 7, 0),
			new Item(74, 8, 0),

			new Item(13, 0, 1),
			new Item(31, 0, 2),
			new Item(53, 0, 3),
			new Item(75, 0, 4),
			new Item(102, 0, 5),

			new Item(25, 1, 0),
			new Item(50, 2, 0),
			new Item(100, 3, 0),
			new Item(20, 0, 1),
			new Item(40, 0, 2),
			new Item(80, 0, 3),
		};

		private static void Trial(ref int best, int config, bool part2)
		{
			int damageScore = 0;
			int armorScore = 0;

			int gold = 0;
			for (int i = 0; i < 16; i++)
			{
				if ((config & (1 << i)) != 0)
				{
					damageScore += s_items[i].Damage;
					armorScore += s_items[i].Armor;
					gold += s_items[i].Cost;
				}
			}
			int myHP = 100;
			int bossHP = 109;
			int bossDamage = 8;
			int bossArmor = 2;

			while (true)
			{
				bossHP -= Math.Max(damageScore - bossArmor, 1);
				if (bossHP <= 0)
				{
					if (part2)
						return;

					break;
				}

				myHP -= Math.Max(bossDamage - armorScore, 1);
				if (myHP <= 0)
				{
					if (part2)
						break;

					return;
				}
			}

			best = part2 ? Math.Max(best, gold) : Math.Min(best, gold);
		}

		private static int BitCount(int value)
		{
			int bits = 0;
			for (int i = 0; i < 32; i++)
			{
				if ((value & (1 << i)) != 0)
					bits++;
			}

			return bits;
		}

		private struct Item
		{
			public Item(int cost, int damage, int armor)
			{
				Cost = cost;
				Damage = damage;
				Armor = armor;
			}

			public int Cost { get; }
			public int Damage { get; }
			public int Armor { get; }
		}

		private static int Part2()
		{
			int best = int.MinValue;
			for (int i = 0; i < 1 << 16; i++)
			{
				int weapon = i & 0x1F;
				int armor = i & (0x1F << 5);
				int rings = i & (0x3F << 10);

				if (BitCount(weapon) != 1)
					continue;

				if (BitCount(armor) > 1)
					continue;

				if (BitCount(rings) > 2)
					continue;

				Trial(ref best, i, true);
			}

			return best;
		}
	}
}