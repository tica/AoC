using System.Text.RegularExpressions;

namespace AoC2015
{
    public class Day7 : AoC.DayBase
    {
        abstract record class ValueOrReference
        {
            public abstract UInt16 Eval(Dictionary<string, Equation> world);

            public record class Value(UInt16 Val) : ValueOrReference
            {
                public override UInt16 Eval(Dictionary<string, Equation> world)
                {
                    return Val;
                }
            }

            public record class Reference(string Target) : ValueOrReference
            {
                public override UInt16 Eval(Dictionary<string, Equation> world)
                {
                    return world[Target].Eval(world);
                }
            }

            public static ValueOrReference Parse(string text)
            {
                if (UInt16.TryParse(text, out var val))
                    return new Value(val);
                return new Reference(text);
            }
        }

        abstract record class Equation(string Name)
        {
            private UInt16? cache;

            public enum Operation
            {
                And, Or, Lshift, Rshift, Not
            }

            public abstract UInt16 Eval(Dictionary<string, Equation> world);

            public record class Assignment(string Name, ValueOrReference Arg) : Equation(Name)
            {
                public override UInt16 Eval(Dictionary<string, Equation> world)
                {
                    return cache ??= Arg.Eval(world);
                }
            }

            public record class BinaryOperation(string Name, ValueOrReference Left, Operation Op, ValueOrReference Right) : Equation(Name)
            {
                public override UInt16 Eval(Dictionary<string, Equation> world)
                {
                    return cache ??= Op switch
                    {
                        Operation.And => (UInt16)(Left.Eval(world) & Right.Eval(world)),
                        Operation.Or => (UInt16)(Left.Eval(world) | Right.Eval(world)),
                        Operation.Lshift => (UInt16)(Left.Eval(world) << Right.Eval(world)),
                        Operation.Rshift => (UInt16)(Left.Eval(world) >> Right.Eval(world)),
                        _ => throw new NotImplementedException()
                    };
                }
            }

            public record class UnaryOperation(string Name, ValueOrReference Arg, Operation Op) : Equation(Name)
            {
                public override UInt16 Eval(Dictionary<string, Equation> world)
                {
                    return cache ??= (UInt16)~Arg.Eval(world);
                }
            }

            public static Equation Parse(string line)
            {
                var m = Regex.Match(line, @"^(\d+|\w+) -> (\w+)$");
                if( m.Success )
                {
                    return new Assignment(m.Groups[2].Value, ValueOrReference.Parse(m.Groups[1].Value));
                }
                m = Regex.Match(line, @"^(\d+|\w+) (AND|OR|LSHIFT|RSHIFT) (\d+|\w+) -> (\w+)$");
                if( m.Success )
                {
                    var left = ValueOrReference.Parse(m.Groups[1].Value);
                    var op = m.Groups[2].Value[0] switch
                    {
                        'A' => Operation.And,
                        'O' => Operation.Or,
                        'L' => Operation.Lshift,
                        'R' => Operation.Rshift,
                        _ => throw new NotImplementedException()
                    };
                    var right = ValueOrReference.Parse(m.Groups[3].Value);
                    return new BinaryOperation(m.Groups[4].Value, left, op, right);
                }
                m = Regex.Match(line, @"^NOT (\d+|\w+) -> (\w+)$");
                if (m.Success)
                {
                    var arg = ValueOrReference.Parse(m.Groups[1].Value);
                    return new UnaryOperation(m.Groups[2].Value, arg, Operation.Not);
                }
                throw new NotImplementedException();
            }
        }

        protected override object Solve1(string filename)
        {
            var lookup = File.ReadAllLines(filename).Select(Equation.Parse).ToDictionary(e => e.Name, e => e);

            var q = filename.Contains("example") ? "h" : "a";

            return (int)lookup[q].Eval(lookup);
        }

        protected override object Solve2(string filename)
        {
            if (filename.Contains("example")) return 0;

            var lookup = File.ReadAllLines(filename).Select(Equation.Parse).ToDictionary(e => e.Name, e => e);
            var a = lookup["a"].Eval(lookup);

            lookup = File.ReadAllLines(filename).Select(Equation.Parse).ToDictionary(e => e.Name, e => e);
            lookup["b"] = new Equation.Assignment("b", new ValueOrReference.Value(a));

            return (int)lookup["a"].Eval(lookup);
        }

        public override object SolutionExample1 => 65412;
        public override object SolutionPuzzle1 => 956;
        public override object SolutionExample2 => 0;
        public override object SolutionPuzzle2 => 40149;
    }
}
