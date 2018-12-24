using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string input = File.ReadAllText("day24.txt");
            List<Group> originalGroups = new List<Group>();
            bool isImmuneSystem = true;

            foreach (string line in input.Split(Environment.NewLine))
            {
                if (line.Contains("Infection"))
                {
                    isImmuneSystem = false;
                    continue;
                }

                const string pattern =
                    "(?<nu>\\d+) units each with (?<hp>\\d+) hit points " +
                    "(\\(.*\\) )?with an attack that does (?<dmg>\\d+) " +
                    "(?<dt>[a-z]+) damage at initiative (?<init>\\d+)";
                Match m = Regex.Match(line, pattern);
                if (!m.Success)
                    continue;

                Group group = new Group();
                group.IsImmuneSystem = isImmuneSystem;
                group.HealthPerUnit = int.Parse(m.Groups["hp"].Value);
                group.NumUnits = int.Parse(m.Groups["nu"].Value);
                group.AttackDamage = int.Parse(m.Groups["dmg"].Value);
                group.AttackType = Enum.Parse<DamageType>(m.Groups["dt"].Value, true);
                group.Initiative = int.Parse(m.Groups["init"].Value);

                void ParseDamageTypes(HashSet<DamageType> types, string regex)
                {
                    Match weak = Regex.Match(line, regex);
                    if (!weak.Success)
                        return;

                    foreach (string t in weak.Groups[1].Value.Split(", "))
                        types.Add(Enum.Parse<DamageType>(t, true));
                }

                ParseDamageTypes(group.Weaknesses, "weak to (([a-z]+(, )?)+)");
                ParseDamageTypes(group.Immunities, "immune to (([a-z]+(, )?)+)");
                originalGroups.Add(group);
            }

            int CalculateDamage(Group attacker, Group defender)
            {
                if (defender.Immunities.Contains(attacker.AttackType))
                    return 0;

                int dmg = attacker.EffectivePower;
                if (defender.Weaknesses.Contains(attacker.AttackType))
                    dmg *= 2;

                return dmg;
            }

            int KilledUnits(Group attacker, Group defender)
                => CalculateDamage(attacker, defender) / defender.HealthPerUnit;

            for (int boost = 0;; boost++)
            {
                List<Group> groups =
                    originalGroups
                    .Select(g => g.WithBoost(g.IsImmuneSystem ? boost : 0))
                    .ToList();

                while (groups.Any(g => g.IsImmuneSystem) &&
                       groups.Any(g => !g.IsImmuneSystem))
                {
                    groups =
                        groups
                        .OrderByDescending(g => g.EffectivePower)
                        .ThenByDescending(g => g.Initiative)
                        .ToList();

                    Dictionary<Group, Group> selected = new Dictionary<Group, Group>();
                    foreach (Group attacker in groups)
                    {
                        List<Group> targets =
                            groups
                            .Where(g => g.IsImmuneSystem != attacker.IsImmuneSystem)
                            .Where(g => !selected.Values.Contains(g))
                            .Where(g => CalculateDamage(attacker, g) > 0)
                            .OrderByDescending(g => CalculateDamage(attacker, g))
                            .ThenByDescending(g => g.EffectivePower)
                            .ThenByDescending(g => g.Initiative)
                            .ToList();
                        if (targets.Any())
                            selected.Add(attacker, targets.First());
                    }

                    if (selected.All(kvp => KilledUnits(kvp.Key, kvp.Value) <= 0))
                        goto NextBoost;

                    groups = groups.OrderByDescending(g => g.Initiative).ToList();
                    for (int i = 0; i < groups.Count; i++)
                    {
                        Group attacker = groups[i];
                        if (!selected.TryGetValue(attacker, out Group defender))
                            continue;

                        defender.NumUnits -= KilledUnits(attacker, defender);
                        if (defender.NumUnits > 0)
                            continue;

                        int idx = groups.IndexOf(defender);
                        groups.RemoveAt(idx);
                        if (idx < i)
                            i--;
                    }
                }

                if (boost == 0 || groups[0].IsImmuneSystem)
                    Console.WriteLine(groups.Sum(g => g.NumUnits));

                if (groups[0].IsImmuneSystem)
                    break;

            NextBoost:;
            }
        }
    }

    internal class Group
    {
        public bool IsImmuneSystem;
        public int HealthPerUnit;
        public int AttackDamage;
        public DamageType AttackType;
        public int Initiative;
        public int NumUnits;
        public int EffectivePower => NumUnits * AttackDamage;

        public HashSet<DamageType> Weaknesses = new HashSet<DamageType>();
        public HashSet<DamageType> Immunities = new HashSet<DamageType>();

        public Group WithBoost(int boost)
        {
            Group g = (Group)MemberwiseClone();
            g.AttackDamage += boost;
            return g;
        }
    }

    internal enum DamageType
    {
        Bludgeoning,
        Slashing,
        Radiation,
        Cold,
        Fire,
    }
}
