using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AoC2023
{
    public class Day9 : DayBase
    {
        public Day9(): base(9) { }

        public override object SolutionExample1 => 114L;
        public override object SolutionPuzzle1 => 2098530125L;
        public override object SolutionExample2 => 2L;
        public override object SolutionPuzzle2 => 1016L;

        private static IEnumerable<int> CalcDifferences(IEnumerable<int> numbers)
        {
            int? prev = null;
            foreach (int number in numbers)
            {
                if (prev != null)
                {
                    yield return number - prev.Value;
                }

                prev = number;
            }
        }

        private static int Extrapolate(List<int> numbers)
        {
            var d = CalcDifferences(numbers).ToList();

            if( d.All(x => x == 0) )
            {
                return numbers.Last();
            }
            else
            {
                return Extrapolate(d) + numbers.Last();
            }
        }

        protected override object Solve1(string filename)
        {
            long sum = 0;

            foreach( var line in System.IO.File.ReadAllLines(filename))
            {
                var numbers = line.Split(' ').Select(int.Parse).ToList();

                sum += Extrapolate(numbers);
            }

            return sum;
        }

        private static int ExtrapolateLeft(List<int> numbers)
        {
            var d = CalcDifferences(numbers).ToList();

            if (d.All(x => x == 0))
            {
                return numbers.First();
            }
            else
            {
                return numbers.First() - ExtrapolateLeft(d);
            }
        }

        protected override object Solve2(string filename)
        {
            long sum = 0;

            foreach (var line in System.IO.File.ReadAllLines(filename))
            {
                var numbers = line.Split(' ').Select(int.Parse).ToList();

                sum += ExtrapolateLeft(numbers);
            }

            return sum;
        }
    }
}
