using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoC.Util;

namespace AoC2025
{
    public class Day5 : AoC.DayBase
    {
        AoC.Util.Range ParseRange(string line)
        {
            var parts = line.Split('-');

            return AoC.Util.Range.FromToInclusive(long.Parse(parts[0]), long.Parse(parts[1]));
        }

        (List<AoC.Util.Range>, List<long>) ParseInput(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename).ToList();
            var ranges = lines.TakeWhile(line => !String.IsNullOrEmpty(line)).Select(ParseRange).ToList();

            var numbers = lines.Skip(ranges.Count + 1).Select(long.Parse).ToList();

            return (ranges, numbers);
        }

        protected override object Solve1(string filename)
        {
            var (ranges, numbers) = ParseInput(filename);

            return numbers.Count(n => ranges.Any(r => r.Contains(n)));
        }

        protected override object Solve2(string filename)
        {
            var (ranges, _) = ParseInput(filename);

            return new RangeList(ranges).Ranges.Sum(r => r.Length);
        }

        public override object SolutionExample1 => 3;
        public override object SolutionPuzzle1 => 840;
        public override object SolutionExample2 => 14L;
        public override object SolutionPuzzle2 => 359913027576322L;
    }
}
