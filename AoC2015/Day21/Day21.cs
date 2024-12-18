using System.Numerics;

namespace AoC2015
{
    public class Day21 : AoC.DayBase
    {
        record class Boss(int HitPoints, int Damage, int Armor)
        {
            public static Boss Parse(string filename)
            {
                var lines = File.ReadAllLines(filename);

                return new Boss(
                    int.Parse(lines[0].Split(' ')[2]),
                    int.Parse(lines[1].Split(' ')[1]),
                    int.Parse(lines[2].Split(' ')[1])
                );
            }
        }

        record class Item(string Name, int Cost, int Damage, int Armor);

        Item?[] Weapons =
        {
            new Item("Dagger", 8, 4, 0),
            new Item("Shortsword", 10, 5, 0 ),
            new Item("Warhammer", 25, 6, 0 ),
            new Item("Longsword", 40, 7, 0 ),
            new Item("Greataxe", 74, 8, 0 )
        };
        Item?[] Armor =
        {
            new Item("Leather", 13, 0, 1 ),
            new Item("Chainmail", 31, 0, 2 ),
            new Item("Splintmail", 53, 0, 3 ),
            new Item("Bandedmail", 75, 0, 4 ),
            new Item("Platemail", 102, 0, 5 ),
            null
        };
        Item?[] Rings =
        {
            new Item("Damage +1", 25, 1, 0),
            new Item("Damage +2", 50, 2, 0),
            new Item("Damage +3", 100, 3, 0),
            new Item("Defense +1", 20, 0, 1),
            new Item("Defense +2", 40, 0, 2),
            new Item("Defense +3", 80, 0, 3),
            null
        };

        private bool SimulateBattle(Boss boss, List<Item> items)
        {
            int bossHp = boss.HitPoints;

            int hp = 100;
            int armor = items.Sum(i => i.Armor);
            int damage = items.Sum(i => i.Damage);

            int playerDamage = Math.Max(damage - boss.Armor, 1);
            int bossDamage = Math.Max(boss.Damage - armor, 1);

            var playerTurns = Math.Ceiling(boss.HitPoints / (decimal)Math.Max(damage - boss.Armor, 1));
            var bossTurns = Math.Ceiling(hp / (decimal)Math.Max(boss.Damage - armor, 1));

            return playerTurns <= bossTurns;
        }

        IEnumerable<List<Item>> BuildItemCombinations()
        {
            foreach (var weapon in Weapons)
            {
                foreach (var armor in Armor)
                {
                    foreach (var ring1 in Rings)
                    {
                        foreach (var ring2 in Rings.Where(r => r != ring1 || r == null))
                        {
                            var lst = new List<Item?> { weapon, armor, ring1, ring2 };
                            var notnull = lst.Where(i => i != null).Select(i => i!).ToList();
                            yield return notnull;
                        }
                    }
                }
            }
        }

        protected override object Solve1(string filename)
        {
            var boss = Boss.Parse(filename);

            var loadouts = BuildItemCombinations().OrderBy(lst => lst.Sum(w => w.Cost)).ToList();

            foreach (var loadout in loadouts)
            {
                if (SimulateBattle(boss, loadout))
                {
                    return loadout.Sum(w => w.Cost);
                }
            }

            return 0;
        }

        protected override object Solve2(string filename)
        {
            var boss = Boss.Parse(filename);

            var loadouts = BuildItemCombinations().OrderByDescending(lst => lst.Sum(w => w.Cost)).ToList();

            foreach (var loadout in loadouts)
            {
                var cost = loadout.Sum(w => w.Cost);

                if (!SimulateBattle(boss, loadout))
                {
                    return cost;
                }
            }

            return 0;
        }

        public override object SolutionExample1 => 23;
        public override object SolutionPuzzle1 => 111;
        public override object SolutionExample2 => 33;
        public override object SolutionPuzzle2 => 188;

    }
}