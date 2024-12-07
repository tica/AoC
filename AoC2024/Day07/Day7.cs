using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day7 : AoC.DayBase
    {
        record class Equation(long Result, List<long> Inputs)
        {
            public static Equation Parse(string line)
            {
                var result = long.Parse(line.Split(':')[0]);
                var inputs = line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                return new Equation(result, inputs);
            }

            private bool IsSolvableFrom(IEnumerable<long> remaining, long accu, bool allowConcatenation)
            {
                if (!remaining.Any())
                {
                    return accu == Result;
                }

                long head = remaining.First();
                var tail = remaining.Skip(1);

                long add = accu + head;
                if ( add <= Result )
                {
                    if (IsSolvableFrom(tail, add, allowConcatenation))
                        return true;
                }

                long mult = accu * head;
                if (mult <= Result)
                {
                    if (IsSolvableFrom(tail, mult, allowConcatenation))
                        return true;
                }

                if (allowConcatenation)
                {
                    long concat = long.Parse(accu.ToString() + head.ToString());
                    if (concat <= Result)
                    {
                        if (IsSolvableFrom(tail, concat, allowConcatenation))
                            return true;
                    }
                }

                return false;
            }

            public bool IsSolvable(bool allowConcatenation)
            {
                return IsSolvableFrom(Inputs.Skip(1), Inputs.First(), allowConcatenation);
            }
        }

        protected override object Solve1(string filename)
        {
            return File.ReadAllLines(filename)
                .Select(Equation.Parse)
                .Where(eq => eq.IsSolvable(false))
                .Sum(eq => eq.Result);
        }

        protected override object Solve2(string filename)
        {
            return File.ReadAllLines(filename)
                .Select(Equation.Parse)
                .Where(eq => eq.IsSolvable(true))
                .Sum(eq => eq.Result);
        }

        public override object SolutionExample1 => 3749L;
        public override object SolutionPuzzle1 => 3312271365652L;
        public override object SolutionExample2 => 11387L;
        public override object SolutionPuzzle2 => 509463489296712L;
    }
}
