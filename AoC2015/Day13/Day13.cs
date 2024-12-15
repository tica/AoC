using AoC.Util;
using System.Text.RegularExpressions;

namespace AoC2015
{
    public class Day13 : AoC.DayBase
    {
        record class Rule(string Who, string NextTo, int Change)
        {
            public static Rule Parse(string line)
            {
                var m = Regex.Match(line, @"^(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+).$");

                int change = int.Parse(m.Groups[3].Value) * ((m.Groups[2].Value == "lose") ? -1 : 1);

                return new Rule(m.Groups[1].Value, m.Groups[4].Value, change);
            }
        }

        private Dictionary<string, Dictionary<string, int>> ParseInput(string filename)
        {
            var changes = File.ReadAllLines(filename).Select(Rule.Parse).ToList();

            var lookup = changes.ToLookup(r => r.Who, r => (r.NextTo, r.Change));
            var dict = lookup.ToDictionary(g => g.Key, g => g.ToDictionary(s => s.NextTo, s => s.Change));

            return dict;
        }

        int EvaluateHappiness(List<string> seating, Dictionary<string, Dictionary<string, int>> rules)
        {
            int h = 0;

            foreach( var (a, b) in seating.Window2() )
            {
                h += rules[a][b];
                h += rules[b][a];
            }

            h += rules[seating.First()][seating.Last()];
            h += rules[seating.Last()][seating.First()];

            return h;
        }

        int EvalTotalHappiness(Dictionary<string, Dictionary<string, int>> input)
        {
            var all = input.Keys.ToList();

            int max = 0;

            foreach (var p in all.GetPermutations(all.Count))
            {
                var h = EvaluateHappiness(p.ToList(), input);
                if (h > max)
                    max = h;
            }

            return max;
        }

        protected override object Solve1(string filename)
        {
            var input = ParseInput(filename);

            return EvalTotalHappiness(input);
        }

        protected override object Solve2(string filename)
        {
            var input = ParseInput(filename);

            foreach(var v in input.Values)
            {
                v.Add("You", 0);
            }
            input.Add("You", input.Keys.ToDictionary(k => k, k => 0));

            return EvalTotalHappiness(input);
        }

        public override object SolutionExample1 => 330;
        public override object SolutionPuzzle1 => 618;
        public override object SolutionExample2 => 286;
        public override object SolutionPuzzle2 => 601;
    }
}
