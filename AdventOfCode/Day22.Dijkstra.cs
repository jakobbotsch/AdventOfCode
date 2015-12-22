using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.XPath;

namespace AdventOfCode
{
	internal static partial class Day22
	{
		private static int BossHP = 58;
		private static int BossDamage = 9;

		private static int Part3()
		{
			// Note: This is part 1 but done with an exhaustive Dijkstra's search
			return DijkstraSearch(false);
		}

		private static int Part4()
		{
			// Part 2 with Dijkstra
			return DijkstraSearch(true);
		}

		private static int DijkstraSearch(bool part2)
		{
			PriorityQueue<SearchNode> toVisit =
				new PriorityQueue<SearchNode>(comparer: SearchNode.CostComparer);

			AddNeighbors(null, toVisit, part2);

			while (toVisit.Count > 0)
			{
				SearchNode node = toVisit.ExtractMax();
				if (node.Won)
					return node.Cost;

				AddNeighbors(node, toVisit, part2);
			}

			return -1;
		}

		private static void AddNeighbors(SearchNode parent, PriorityQueue<SearchNode> toVisit, bool part2)
		{
			int cost = parent?.Cost ?? 0;
			AddNeighbor(parent, Spell.Drain, cost + 73, toVisit, part2);
			AddNeighbor(parent, Spell.MagicMissile, cost + 53, toVisit, part2);
			AddNeighbor(parent, Spell.Poison, cost + 173, toVisit, part2);
			AddNeighbor(parent, Spell.Recharge, cost + 229, toVisit, part2);
			AddNeighbor(parent, Spell.Shield, cost + 113, toVisit, part2);
		}

		private static void AddNeighbor(SearchNode parent, Spell spell, int cost,
		                                PriorityQueue<SearchNode> toVisit, bool part2)
		{
			var node = new SearchNode(spell, cost, parent);
			var result = Evaluate(node, part2);
			if (result == EvaluationResult.MoreSpells)
				toVisit.Add(node);

			if (result == EvaluationResult.PlayerWon)
			{
				node.Won = true;
				toVisit.Add(node);
			}
		}

		private static EvaluationResult Evaluate(SearchNode lastNode, bool part2)
		{
			var parent = lastNode.Parent;
			if (parent != null)
			{
				lastNode.BossHP = parent.BossHP;
				lastNode.PlayerHP = parent.PlayerHP;
				lastNode.PlayerMana = parent.PlayerMana;
				lastNode.ShieldTurns = parent.ShieldTurns;
				lastNode.PoisonTurns = parent.PoisonTurns;
				lastNode.RechargeTurns = parent.RechargeTurns;
			}
			else
			{
				lastNode.BossHP = BossHP;
				lastNode.PlayerHP = 50;
				lastNode.PlayerMana = 500;
			}

			// Player turn
			if (part2)
			{
				lastNode.PlayerHP--;
				if (lastNode.PlayerHP <= 0)
					return EvaluationResult.PlayerLost;
			}

			ApplySpellEffects(lastNode);

			if (lastNode.BossHP <= 0)
				return EvaluationResult.PlayerWon;

			// Cast spell
			switch (lastNode.Cast)
			{
				case Spell.Drain:
					if (lastNode.PlayerMana < 73)
						return EvaluationResult.Illegal;

					lastNode.PlayerMana -= 73;
					lastNode.PlayerHP += 2;
					lastNode.BossHP -= 2;
					break;
				case Spell.MagicMissile:
					if (lastNode.PlayerMana < 53)
						return EvaluationResult.Illegal;

					lastNode.PlayerMana -= 53;
					lastNode.BossHP -= 4;
					break;
				case Spell.Poison:
					if (lastNode.PlayerMana < 173 || lastNode.PoisonTurns > 0)
						return EvaluationResult.Illegal;

					lastNode.PlayerMana -= 173;
					lastNode.PoisonTurns = 6;
					break;
				case Spell.Recharge:
					if (lastNode.PlayerMana < 229 || lastNode.RechargeTurns > 0)
						return EvaluationResult.Illegal;

					lastNode.PlayerMana -= 229;
					lastNode.RechargeTurns = 5;
					break;
				case Spell.Shield:
					if (lastNode.PlayerMana < 113 || lastNode.ShieldTurns > 0)
						return EvaluationResult.Illegal;

					lastNode.PlayerMana -= 113;
					lastNode.ShieldTurns = 6;
					break;
			}

			if (lastNode.BossHP <= 0)
				return EvaluationResult.PlayerWon;

			// Boss's turn
			ApplySpellEffects(lastNode);
			if (lastNode.BossHP <= 0)
				return EvaluationResult.PlayerWon;

			lastNode.PlayerHP -= Math.Max(1, BossDamage - lastNode.PlayerArmor);

			if (lastNode.PlayerHP <= 0)
				return EvaluationResult.PlayerLost;

			return EvaluationResult.MoreSpells;
		}

		private static void ApplySpellEffects(SearchNode lastNode)
		{
			if (lastNode.PoisonTurns > 0)
			{
				lastNode.PoisonTurns--;
				lastNode.BossHP -= 3;
			}

			if (lastNode.RechargeTurns > 0)
			{
				lastNode.RechargeTurns--;
				lastNode.PlayerMana += 101;
			}

			if (lastNode.ShieldTurns > 0)
			{
				lastNode.ShieldTurns--;
				lastNode.PlayerArmor = 7;
			}
			else
			{
				lastNode.PlayerArmor = 0;
			}
		}

		private enum EvaluationResult
		{
			PlayerLost,
			Illegal,
			MoreSpells,
			PlayerWon,
		}

		private class SearchNode
		{
			public SearchNode(Spell cast, int cost, SearchNode parent)
			{
				Cast = cast;
				Cost = cost;
				Parent = parent;
			}

			public Spell Cast { get; }
			public int Cost { get; }
			public SearchNode Parent { get; }
			public int BossHP { get; set; }
			public int PlayerHP { get; set; }
			public int PlayerMana { get; set; }
			public int PlayerArmor { get; set; }
			public int RechargeTurns { get; set; }
			public int PoisonTurns { get; set; }
			public int ShieldTurns { get; set; }
			public bool Won { get; set; }

			private sealed class CostComparerImpl : IComparer<SearchNode>
			{
				public int Compare(SearchNode x, SearchNode y)
				{
					return y.Cost.CompareTo(x.Cost);
				}
			}

			public static IComparer<SearchNode> CostComparer { get; } = new CostComparerImpl();
		}
	}
}