using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day11 : AoC.DayBase
    {
        private class Globals
        {
            public int old = 0;
        };

        private class Monkey
        {
            public Queue<long> Items { get; set; } = new Queue<long>();

            public char Operation { get; set; } = ' ';
            public int? Arg { get; set; } = null;

            public int TestDivisible { get; set; }

            public int TargetTrue { get; set; }
            public int TargetFalse { get; set; }

            public long Activity { get; set; } = 0;

            public void DoTurn(Dictionary<int, Monkey> monkeys, int divider, int modulo)
            {
                while (Items.TryDequeue(out long value))
                {
                    var newValue = Operation switch
                    {
                        '+' => value + (Arg.HasValue ? Arg.Value : value),
                        _ => value * (Arg.HasValue ? Arg.Value : value)
                    };

                    newValue /= divider;
                    newValue %= modulo;

                    monkeys[(newValue % TestDivisible == 0) ? TargetTrue : TargetFalse].Items.Enqueue(newValue);

                    Activity += 1;
                }
            }
        }

        private Dictionary<int, Monkey> ParseInput(string filename)
        {
            Dictionary<int, Monkey> monkeys = new Dictionary<int, Monkey>();

            Monkey current = new();

            foreach (var line in File.ReadAllLines(filename))
            {
                var matchMonkey = Regex.Match(line, @"Monkey (\d+):");
                if (matchMonkey.Success)
                {
                    current = new Monkey {};
                    monkeys.Add(int.Parse(matchMonkey.Groups[1].Value), current);
                    continue;
                }

                var matchItems = Regex.Match(line, @"Starting items: ([\d\s,]+)");
                if (matchItems.Success)
                {
                    current.Items = new Queue<long>(matchItems.Groups[1].Value.Split(", ").Select(long.Parse));
                    continue;
                }

                var matchOperation = Regex.Match(line, @"Operation: new = old ([\+\*]) (\d+|old)");
                if (matchOperation.Success)
                {
                    current.Operation = matchOperation.Groups[1].Value[0];

                    if (int.TryParse(matchOperation.Groups[2].Value, out int argument))
                    {
                        current.Arg = argument;
                    }

                    continue;
                }

                var matchTest = Regex.Match(line, @"Test: divisible by (\d+)");
                if (matchTest.Success)
                {
                    current.TestDivisible = int.Parse(matchTest.Groups[1].Value);
                    continue;
                }

                var matchTrue = Regex.Match(line, @"If true: throw to monkey (\d+)");
                if (matchTrue.Success)
                {
                    current.TargetTrue = int.Parse(matchTrue.Groups[1].Value);
                    continue;
                }

                var matchFalse = Regex.Match(line, @"If false: throw to monkey (\d+)");
                if (matchFalse.Success)
                {
                    current.TargetFalse = int.Parse(matchFalse.Groups[1].Value);
                    continue;
                }

                if (!string.IsNullOrEmpty(line))
                    throw new NotImplementedException();
            }

            return monkeys;
        }

        private long CalcMonkeyBusiness(Dictionary<int, Monkey> monkeys, int rounds, int divider = 1)
        {
            var modulo = monkeys.Values.Aggregate(1, (a, m) => a * m.TestDivisible);            

            var allMonkeys = monkeys.Values.ToList();

            for (int i = 0; i < rounds; ++i)
            {
                allMonkeys.ForEach(m => m.DoTurn(monkeys, divider, modulo));
            }

            return monkeys.Values.Select(m => m.Activity).OrderByDescending(a => a).Take(2).Aggregate(1L, (a, n) => a * n);
        }

        protected override object Solve1(string filename)
        {
            var monkeys = ParseInput(filename);

            return CalcMonkeyBusiness(monkeys, 20, 3);
        }

        protected override object Solve2(string filename)
        {
            var monkeys = ParseInput(filename);

            return CalcMonkeyBusiness(monkeys, 10000);
        }

        public override object SolutionExample1 => 10605L;
        public override object SolutionPuzzle1 => 99840L;
        public override object SolutionExample2 => 2713310158L;
        public override object SolutionPuzzle2 => 20683044837L;
    }
}
