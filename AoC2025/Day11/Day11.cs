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

        private long CountPathsToOut2(Dictionary<string, List<string>> graph, string from, Dictionary<(string, bool, bool), long> dp, bool visitedDac = false, bool visitedFft = false)
        {
            if (from == "out")
            {
                return (visitedDac && visitedFft) ? 1 : 0;
            }

            if (dp.TryGetValue((from, visitedDac, visitedFft), out long n))
            {
                return n;
            }

            visitedDac |= from == "dac";
            visitedFft |= from == "fft";

            long result = graph[from].Sum(n => CountPathsToOut2(graph, n, dp, visitedDac, visitedFft));

            dp[(from, visitedDac, visitedFft)] = result;

            return result;
        }

        protected override object Solve2(string filename)
        {
            var graph = ParseGraph(filename);

            return CountPathsToOut2(graph, "svr", new());
        }

        public override object SolutionExample1 => 5;
        public override object SolutionPuzzle1 => 431;
        public override object SolutionExample2 => 2L;
        public override object SolutionPuzzle2 => 358458157650450L;
    }
}
