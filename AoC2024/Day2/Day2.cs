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
    public class Day2 : DayBase
    {
        public Day2() : base(2)
        {
        }

        private static IEnumerable<List<int>> ParseInput(string filename)
        {
            return System.IO.File.ReadAllLines(filename)
                .Select(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
                .Select(l => l.ToList()
            );
        }

        private static IEnumerable<(T, T)> EnumPairs<T>(IEnumerable<T> source)
        {
            var prev = source.First();

            foreach (var next in source.Skip(1))
            {
                yield return (prev, next);

                prev = next;
            }
        }

        private static bool CheckLine(IEnumerable<int> line)
        {
            Func<int, bool> checkDiff = line.First() > line.Last() ? (d => d >= -3 && d <= -1) : (d => d >= 1 && d <= 3);

            return EnumPairs(line).Select(t => t.Item2 - t.Item1).All(checkDiff);
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
