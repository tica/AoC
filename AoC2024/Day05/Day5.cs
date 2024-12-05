
using System.Formats.Asn1;
using System.Net.Http.Headers;

namespace AoC2024
{
    public class Day5 : AoC.DayBase
    {
        class Input
        {
            public required List<(int, int)> Rules;
            public required List<List<int>> Updates;
        }

        private static Input ParseInput(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            var rules = lines.TakeWhile(l => !string.IsNullOrEmpty(l)).Select(line => line.Split('|').Select(int.Parse)).Select(a => (a.First(), a.Last())).ToList();
            var updates = lines.Skip(rules.Count + 1).Select(line => line.Split(',').Select(int.Parse).ToList()).ToList();

            return new Input
            {
                Rules = rules,
                Updates = updates
            };
        }

        private static bool Validate(List<int> update, List<(int, int)> rules)
        {
            for( int i = 0; i < update.Count - 1; ++i )
            {
                for( int j = i + 1; j < update.Count; ++j )
                {
                    var left = update[i];
                    var right = update[j];

                    bool fail = rules.Any( r => r.Item1 == right && r.Item2 == left );
                    if (fail) return false;
                }
            }

            return true;
        }

        protected override object Solve1(string filename)
        {
            var input = ParseInput(filename);

            return input.Updates.Where(u => Validate(u, input.Rules)).Sum(u => u[u.Count / 2]);
        }

        private Comparison<int> BuildCompare(List<(int, int)> rules)
        {
            return (a, b) =>
            {
                foreach (var r in rules)
                {
                    if (r.Item1 == a && r.Item2 == b)
                        return -1;
                    if (r.Item1 == b && r.Item2 == a)
                        return 1;
                }

                return 0;
            };
        }

        protected override object Solve2(string filename)
        {
            var input = ParseInput(filename);

            var fails = input.Updates.Where(u => !Validate(u, input.Rules));

            int sum = 0;

            foreach (var u in fails)
            {
                u.Sort(BuildCompare(input.Rules));

                sum += u[u.Count / 2];
            }

            return sum;
        }

        public override object SolutionExample1 => 143;
        public override object SolutionPuzzle1 => 5955;
        public override object SolutionExample2 => 123;
        public override object SolutionPuzzle2 => 4030;
    }
}
