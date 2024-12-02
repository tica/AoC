using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2024
{
    public class Day1 : AoC.DayBase
    {
        private (List<int>, List<int>) ParseInput(string filename)
        {
            var left = new List<int>();
            var right = new List<int>();

            foreach (var line in System.IO.File.ReadAllLines(filename))
            {
                var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                left.Add(numbers[0]);
                right.Add(numbers[1]);
            }

            return (left, right);
        }

        protected override object Solve1(string filename)
        {
            var (left, right) = ParseInput(filename);

            left.Sort();
            right.Sort();

            return left.Zip(right, (l, r) => Math.Abs(l - r)).Sum();
        }

        protected override object Solve2(string filename)
        {
            var (left, right) = ParseInput(filename);

            return left.Sum(l => right.Count(r => r == l) * l);
        }

        public override object SolutionExample1 => 11;
        public override object SolutionPuzzle1 => 1834060;
        public override object SolutionExample2 => 31;
        public override object SolutionPuzzle2 => 21607792;
    }
}
