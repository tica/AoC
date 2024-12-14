using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day21 : AoC.DayBase
    {
        abstract record Formula
        {
            public abstract long Eval(IDictionary<string, Formula> world);

            public record Placeholder(string Name) : Formula
            {
                public override long Eval(IDictionary<string, Formula> world)
                {
                    return world[Name].Eval(world);
                }
            }

            public record Number(int Value) : Formula
            {
                public long ? Override { get; set; }

                public override long Eval(IDictionary<string, Formula> world)
                {
                    return Override ?? Value;
                }
            }
            public record Addition(Formula Left, Formula Right) : Formula
            {
                public override long Eval(IDictionary<string, Formula> world)
                {
                    return Left.Eval(world) + Right.Eval(world);
                }
            }
            public record Subtraction(Formula Left, Formula Right) : Formula
            {
                public override long Eval(IDictionary<string, Formula> world)
                {
                    return Left.Eval(world) - Right.Eval(world);
                }
            }
            public record Multiplication(Formula Left, Formula Right) : Formula
            {
                public override long Eval(IDictionary<string, Formula> world)
                {
                    return Left.Eval(world) * Right.Eval(world);
                }
            }
            public record Division(Formula Left, Formula Right) : Formula
            {
                public override long Eval(IDictionary<string, Formula> world)
                {
                    return Left.Eval(world) / Right.Eval(world);
                }
            }
        }

        IEnumerable<(string, Formula)> ParseInput(string fileName)
        {
            foreach (var line in File.ReadAllLines(fileName))
            {
                var m = Regex.Match(line, @"([a-z][a-z][a-z][a-z]): (?:(\d+)|([a-z][a-z][a-z][a-z]) ([+-\/\*]) ([a-z][a-z][a-z][a-z]))");

                var name = m.Groups[1].Value;

                if (int.TryParse(m.Groups[2].Value, out int value))
                {
                    yield return (name, new Formula.Number(value));
                }
                else
                {
                    var left = new Formula.Placeholder(m.Groups[3].Value);
                    var right = new Formula.Placeholder(m.Groups[5].Value);
                    yield return m.Groups[4].Value[0] switch
                    {
                        '+' => (name, new Formula.Addition(left, right)),
                        '-' => (name, new Formula.Subtraction(left, right)),
                        '*' => (name, new Formula.Multiplication(left, right)),
                        '/' => (name, new Formula.Division(left, right)),
                        _ => throw new NotImplementedException()
                    };
                }
            }
        }

        protected override object Solve1(string filename)
        {
            var input = ParseInput(filename).ToDictionary(
                ((string name, Formula formula) entry) => entry.name,
                ((string name, Formula formula) entry) => entry.formula
            );

            return input["root"].Eval(input);
        }

        protected override object Solve2(string filename)
        {
            var input = ParseInput(filename).ToDictionary(
                ((string name, Formula formula) entry) => entry.name,
                ((string name, Formula formula) entry) => entry.formula
            );

            var root = (Formula.Addition)input["root"];
            var humn = (Formula.Number)input["humn"];            

            long lower = -80000000000000;
            long upper = 100000000000000;
            
            if( filename.Contains("example") )
            {
                lower = -10000;
                upper = 10000;
            }

            while ( lower <= upper)
            {
                long mid = lower + (upper - lower) / 2;
                humn.Override = mid;
                var delta = root.Left.Eval(input) - root.Right.Eval(input);

                if( !filename.Contains("example"))
                    delta = -delta;

                if( delta > 0 )
                {
                    upper = mid - 1;
                }
                else if( delta < 0 )
                {
                    lower = mid + 1;
                }
                else
                {
                    return mid;
                }
            }

            return upper;
        }

        public override object SolutionExample1 => 152L;
        public override object SolutionPuzzle1 => 158661812617812L;
        public override object SolutionExample2 => 302L;
        public override object SolutionPuzzle2 => 3352886133831L;
    }
}
