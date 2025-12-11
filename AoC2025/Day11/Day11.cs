using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day11 : AoC.DayBase
    {
        Dictionary<string, List<string>> ParseGraph(string filename)
        {
            return File.ReadAllLines(filename).ToDictionary(
                line => line.Substring(0, 3),
                line => line.Substring(5).Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
        }

        private int CountPathsToOut(Dictionary<string, List<string>> graph, string from)
        {
            if (from == "out")
                return 1;

            return graph[from].Sum(n => CountPathsToOut(graph, n));
        }

        protected override object Solve1(string filename)
        {
            var graph = ParseGraph(filename);

            return CountPathsToOut(graph, "you");
        }

        protected override object Solve2(string filename)
        {
            var graph = ParseGraph(filename);

            return 0;
        }

        public override object SolutionExample1 => 5;
        public override object SolutionPuzzle1 => 431;
        public override object SolutionExample2 => 0;
        public override object SolutionPuzzle2 => 0;
    }
}
