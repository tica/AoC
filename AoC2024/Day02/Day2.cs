﻿using AoC.Util;

namespace AoC2024
{
    public class Day2 : AoC.DayBase
    {
        private static IEnumerable<IEnumerable<int>> ParseInput(string filename)
        {
            return System.IO.File.ReadAllLines(filename)
                .Select(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
        }

        private static bool CheckLine(IEnumerable<int> line)
        {
            Func<int, bool> checkDiff = line.First() > line.Last() ? (d => d >= -3 && d <= -1) : (d => d >= 1 && d <= 3);

            return line.Window2().Select(t => t.Item2 - t.Item1).All(checkDiff);
        }

        protected override object Solve1(string filename)
        {
            return ParseInput(filename).Count(CheckLine);
        }

        private IEnumerable<IEnumerable<int>> Permute(IEnumerable<int> line)
        {
            return line.Select((_, i) => line.Take(i).Concat(line.Skip(i + 1)));
        }

        protected override object Solve2(string filename)
        {
            var lines = ParseInput(filename);

            return lines.Count(line => Permute(line).Any(CheckLine));
        }

        public override object SolutionExample1 => 2;
        public override object SolutionPuzzle1 => 680;
        public override object SolutionExample2 => 4;
        public override object SolutionPuzzle2 => 710;
    }
}
