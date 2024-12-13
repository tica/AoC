using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day19 : AoC.DayBase
    {
        record class Blueprint(int Id, int OreRobotOre, int ClayRobotOre, int ObsidianRobotOre, int ObsidianRobotClay, int GeodeRobotOre, int GeodeRobotObsidian)
        {
            private int? maxAnyOre = null;
            public int MaxAnyRobotOre => maxAnyOre ??= Math.Max(Math.Max(OreRobotOre, ClayRobotOre), Math.Max(ObsidianRobotOre, GeodeRobotOre));

            public static Blueprint Parse(string line)
            {
                var m = Regex.Match(line, @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");

                return new Blueprint(
                    int.Parse(m.Groups[1].Value),
                    int.Parse(m.Groups[2].Value),
                    int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value),
                    int.Parse(m.Groups[5].Value),
                    int.Parse(m.Groups[6].Value),
                    int.Parse(m.Groups[7].Value)
                );
            }
        }

        int limit = 24;
        int FindMaximumGeode(Blueprint rules, int time, int ore, int clay, int obsidian, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, Dictionary<(int, int, int, int, int, int, int, int), int> cache)
        {
            if (time == limit)
                return geodeRobots;

            if (cache.TryGetValue((time, ore, clay, obsidian, oreRobots, clayRobots, obsidianRobots, geodeRobots), out var cached))
                return cached;

            int max = 0;

            if (ore >= rules.GeodeRobotOre && obsidian >= rules.GeodeRobotObsidian)
            {
                int m = FindMaximumGeode(rules, time + 1, ore - rules.GeodeRobotOre + oreRobots, clay + clayRobots, obsidian - rules.GeodeRobotObsidian + obsidianRobots, oreRobots, clayRobots, obsidianRobots, geodeRobots + 1, cache) + geodeRobots;
                max = Math.Max(max, m);
            }
            else
            {
                if (ore >= rules.ObsidianRobotOre && clay >= rules.ObsidianRobotClay && obsidianRobots < rules.GeodeRobotObsidian)
                {
                    int m = FindMaximumGeode(rules, time + 1, ore - rules.ObsidianRobotOre + oreRobots, clay - rules.ObsidianRobotClay + clayRobots, obsidian + obsidianRobots, oreRobots, clayRobots, obsidianRobots + 1, geodeRobots, cache) + geodeRobots;
                    max = Math.Max(max, m);
                }
                if (ore >= rules.OreRobotOre && oreRobots < rules.MaxAnyRobotOre)
                {
                    int m = FindMaximumGeode(rules, time + 1, ore - rules.OreRobotOre + oreRobots, clay + clayRobots, obsidian + obsidianRobots, oreRobots + 1, clayRobots, obsidianRobots, geodeRobots, cache) + geodeRobots;
                    max = Math.Max(max, m);
                }
                if (ore >= rules.ClayRobotOre && clayRobots < rules.ObsidianRobotClay)
                {
                    int m = FindMaximumGeode(rules, time + 1, ore - rules.ClayRobotOre + oreRobots, clay + clayRobots, obsidian + obsidianRobots, oreRobots, clayRobots + 1, obsidianRobots, geodeRobots, cache) + geodeRobots;
                    max = Math.Max(max, m);
                }

                if (true)
                {
                    int m = FindMaximumGeode(rules, time + 1, ore + oreRobots, clay + clayRobots, obsidian + obsidianRobots, oreRobots, clayRobots, obsidianRobots, geodeRobots, cache) + geodeRobots;
                    max = Math.Max(max, m);
                }
            }

            cache.Add((time, ore, clay, obsidian, oreRobots, clayRobots, obsidianRobots, geodeRobots), max);

            return max;
        }

        protected override object Solve1(string filename)
        {
            int sum = 0;
            foreach( var bp in File.ReadLines(filename).Select(Blueprint.Parse))
            {
                int score = FindMaximumGeode(bp, 1, 0, 0, 0, 1, 0, 0, 0, new());
                sum += score * bp.Id;
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            int product = 1;
            limit = 32;
            foreach (var bp in File.ReadLines(filename).Select(Blueprint.Parse).Take(3))
            {
                var score = FindMaximumGeode(bp, 1, 0, 0, 0, 1, 0, 0, 0, new());
                product *= score;
            }

            return product;
        }

        public override object SolutionExample1 => 33;
        public override object SolutionPuzzle1 => 1528;
        public override object SolutionExample2 => 3472;
        public override object SolutionPuzzle2 => 16926;
    }
}
