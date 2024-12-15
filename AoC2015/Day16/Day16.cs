using System.Text.RegularExpressions;

namespace AoC2015
{
    public class Day16 : AoC.DayBase
    {
        record class Sue(int Number, Dictionary<string, int> Items)
        {
            public static Sue Parse(string line)
            {
                var m = Regex.Match(line, @"^Sue (\d+): (\w+): (\d+), (\w+): (\d+), (\w+): (\d+)$");

                var dict = new Dictionary<string, int>();

                for (int i = 0; i < 3; ++i)
                {
                    dict.Add(m.Groups[i * 2 + 2].Value, int.Parse(m.Groups[i * 2 + 3].Value));
                }

                return new Sue(int.Parse(m.Groups[1].Value), dict);
            }
        }

        protected override object Solve1(string filename)
        {
            if (filename.Contains("example"))
                return 0;

            var sues = File.ReadAllLines(filename).Select(Sue.Parse).ToList();

            var hints = new Dictionary<string, int>()
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

            foreach (var sue in sues)
            {
                bool accept = true;

                foreach (var kvp in sue.Items)
                {
                    if (hints[kvp.Key] != kvp.Value)
                        accept = false;
                }

                if (accept)
                {
                    return sue.Number;
                }
            }

            throw new InvalidOperationException();
        }

        protected override object Solve2(string filename)
        {
            if (filename.Contains("example"))
                return 0;

            var sues = File.ReadAllLines(filename).Select(Sue.Parse).ToList();

            var hints = new Dictionary<string, (int Val, Func<int, int, bool> RejectOp)>()
            {
                ["children"] = (3, (a, b) => a != b),
                ["cats"] = (7, (a, b) => a <= b),
                ["samoyeds"] = (2, (a, b) => a != b),
                ["pomeranians"] = (3, (a, b) => a >= b),
                ["akitas"] = (0, (a, b) => a != b),
                ["vizslas"] = (0, (a, b) => a != b),
                ["goldfish"] = (5, (a, b) => a >= b),
                ["trees"] = (3, (a, b) => a <= b),
                ["cars"] = (2, (a, b) => a != b),
                ["perfumes"] = (1, (a, b) => a != b),
            };

            foreach (var sue in sues)
            {
                bool accept = true;

                foreach (var kvp in sue.Items)
                {
                    if (hints[kvp.Key].RejectOp(kvp.Value, hints[kvp.Key].Val))
                        accept = false;
                }

                if (accept)
                {
                    return sue.Number;
                }
            }

            throw new InvalidOperationException();
        }

        public override object SolutionExample1 => 0;
        public override object SolutionPuzzle1 => 213;
        public override object SolutionExample2 => 0;
        public override object SolutionPuzzle2 => 323;
    }
}
