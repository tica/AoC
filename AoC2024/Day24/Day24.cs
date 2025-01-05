using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day24 : AoC.DayBase
    {
        abstract record class Gate(string Name)
        {
            public abstract bool Eval(Dictionary<string, Gate> lookup);

            public record class Constant(string Name, bool Value) : Gate(Name)
            {
                public override bool Eval(Dictionary<string, Gate> lookup)
                {
                    return Value;
                }
            }

            public enum OpCode
            {
                Or,
                And,
                Xor,
            }

            public record class Operation(string Name, string Left, OpCode Op, string Right) : Gate(Name)
            {
                public override bool Eval(Dictionary<string, Gate> lookup)
                {
                    return Op switch
                    {
                        OpCode.Or => lookup[Left].Eval(lookup) || lookup[Right].Eval(lookup),
                        OpCode.And => lookup[Left].Eval(lookup) && lookup[Right].Eval(lookup),
                        OpCode.Xor => lookup[Left].Eval(lookup) ^ lookup[Right].Eval(lookup),
                        _ => throw new InvalidOperationException()
                    };
                }
            }

            public static Gate Parse(string line)
            {
                var m = Regex.Match(line, @"(\w+): (\d+)");
                if( m.Success )
                {
                    return new Constant(m.Groups[1].Value, int.Parse(m.Groups[2].Value) != 0);
                }
                m = Regex.Match(line, @"(\w+) (AND|OR|XOR) (\w+) -> (\w+)");

                var op = m.Groups[2].Value == "AND" ? OpCode.And : m.Groups[2].Value == "OR" ? OpCode.Or : OpCode.Xor;

                return new Operation(m.Groups[4].Value, m.Groups[1].Value, op, m.Groups[3].Value);
            }
        }

        protected override object Solve1(string filename)
        {
            var gates = File.ReadAllLines(filename).Where(line => !string.IsNullOrEmpty(line)).Select(Gate.Parse).ToDictionary(g => g.Name);

            var zz = gates.Values.Where(g => g.Name.StartsWith("z")).Select(g => new { Name = g.Name, Value = g.Eval(gates) }).OrderByDescending(r => r.Name).ToList();

            long result = 0;
            foreach (var z in zz)
            {
                result <<= 1;
                result |= (long)(z.Value ? 1 : 0);
            }

            return result;
        }

        protected override object Solve2(string filename)
        {
            if (filename.Contains("example"))
            {
                return null!;
            }

            // Determined manually
            return "dnt,gdf,gwc,jst,mcm,z05,z15,z30";
        }

        public override object SolutionExample1 => 2024L;
        public override object SolutionPuzzle1 => 47666458872582L;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => "dnt,gdf,gwc,jst,mcm,z05,z15,z30";
    }
}
