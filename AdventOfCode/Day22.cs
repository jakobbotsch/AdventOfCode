using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	internal static partial class Day22
	{
		private static readonly Random s_rand = new Random();

		private static int Part1()
		{
			int best = int.MaxValue;
			for (int i = 0; i < 500000; i++)
			{
				int spent = Trial(false);
				if (spent != -1)
					best = Math.Min(best, spent);
			}

			return best;
		}

		private static int Part2()
		{
			int best = int.MaxValue;
			for (int i = 0; i < 500000; i++)
			{
				int spent = Trial(true);
				if (spent != -1)
					best = Math.Min(best, spent);
			}

			return best;
		}

		private static int Trial(bool part2)
		{
			int bossHP = 58;
			int bossDamage = 9;

			int playerHP = 50;
			int playerMana = 500;
			int playerArmor = 0;
			List<SpellEffect> effects = new List<SpellEffect>();

			Action applySpellEffects =
				() =>
				{
					playerArmor = 0;
					foreach (var t in effects)
					{
						switch (t.Spell)
						{
							case Spell.Recharge:
								playerMana += 101;
								break;
							case Spell.Poison:
								bossHP -= 3;
								break;
							case Spell.Shield:
								playerArmor += 7;
								break;
						}

						t.Turns--;
					}

					effects.RemoveAll(e => e.Turns <= 0);
				};

			int manaSpent = 0;
			while (true)
			{
				// Player turn
				if (part2)
				{
					playerHP -= 1;
					if (playerHP <= 0)
						return -1;
				}

				applySpellEffects();

				if (bossHP <= 0)
					return manaSpent;

				// Cast spell
				bool didCast = false;
				for (int i = 0; i < 25 && !didCast; i++)
				{
					Spell spell = (Spell)s_rand.Next(5);
					switch (spell)
					{
						case Spell.Drain:
							if (playerMana < 73)
								continue;

							playerMana -= 73;
							manaSpent += 73;
							playerHP += 2;
							bossHP -= 2;
							didCast = true;
							break;
						case Spell.MagicMissile:
							if (playerMana < 53)
								continue;

							playerMana -= 53;
							manaSpent += 53;
							bossHP -= 4;
							didCast = true;
							break;
						case Spell.Poison:
							if (playerMana < 173 || effects.Any(e => e.Spell == Spell.Poison))
								continue;

							playerMana -= 173;
							manaSpent += 173;
							effects.Add(new SpellEffect(Spell.Poison, 6));
							didCast = true;
							break;
						case Spell.Recharge:
							if (playerMana < 229 || effects.Any(e => e.Spell == Spell.Recharge))
								continue;

							playerMana -= 229;
							manaSpent += 229;
							effects.Add(new SpellEffect(Spell.Recharge, 5));
							didCast = true;
							break;
						case Spell.Shield:
							if (playerMana < 113 || effects.Any(e => e.Spell == Spell.Shield))
								continue;

							playerMana -= 113;
							manaSpent += 113;
							effects.Add(new SpellEffect(Spell.Shield, 6));
							didCast = true;
							break;
					}
				}

				if (!didCast)
					return -1; // Couldn't cast anything, player loses

				if (bossHP <= 0)
					return manaSpent;

				// Bosses turn
				applySpellEffects();
				if (bossHP <= 0)
					return manaSpent;

				playerHP -= Math.Max(1, bossDamage - playerArmor);

				if (playerHP <= 0)
					return -1;
			}
		}

		private enum Spell
		{
			MagicMissile,
			Drain,
			Shield,
			Poison,
			Recharge,
		}

		private class SpellEffect
		{
			public SpellEffect(Spell spell, int turns)
			{
				Spell = spell;
				Turns = turns;
			}

			public Spell Spell { get; }
			public int Turns { get; set; }
		}
	}
}