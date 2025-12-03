using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC.Util;

namespace AoC2025
{
    public class Day3 : AoC.DayBase
    {
        private List<int> ParseLine(string line)
        {
            return line.Select(ch => int.Parse(ch.ToString())).ToList();
        }

        private long MaximumJoltage(List<int> values, int offset, int remainingDigits)
        {
            if (remainingDigits == 0)
                return 0;

            var (n, p) = values.Skip(offset).Take(values.Count - offset - (remainingDigits - 1)).MaxIndex();

            return n * (long)Math.Pow(10, remainingDigits - 1) + MaximumJoltage(values, offset + p + 1, remainingDigits - 1);
        }

        protected override object Solve1(string filename)
        {
            Func<List<int>, long> maximumJoltage = lst => MaximumJoltage(lst, 0, 2);

            return File.ReadAllLines(filename).Select(ParseLine).Select(maximumJoltage).Sum();
        }

        protected override object Solve2(string filename)
        {
            Func<List<int>, long> maximumJoltage = lst => MaximumJoltage(lst, 0, 12);

            return File.ReadAllLines(filename).Select(ParseLine).Select(maximumJoltage).Sum();
        }

        public override object SolutionExample1 => 357L;
        public override object SolutionPuzzle1 => 17034L;
        public override object SolutionExample2 => 3121910778619L;
        public override object SolutionPuzzle2 => 168798209663590L;
    }
}
