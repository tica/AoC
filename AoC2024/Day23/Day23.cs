using AoC.Util;
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
    public class Day23 : AoC.DayBase
    {
        Dictionary<string, List<string>> ParseInput(string filename)
        {
            var pairs = File.ReadAllLines(filename).Select(line => line.Split('-')).ToList();
            var graph = pairs.Select(p => p[0]).Concat(pairs.Select(p => p[1])).Distinct().ToDictionary(p => p, p => new List<string>());
            foreach (var p in pairs)
            {
                if (p[0].CompareTo(p[1]) < 0)
                    graph[p[0]].Add(p[1]);
                else
                    graph[p[1]].Add(p[0]);
            }
            return graph;
        }

        IEnumerable<string[]> FindConnectedSets(Dictionary<string, List<string>> graph)
        {
            foreach (var node in graph.Keys.Order())
            {
                var adjacent = graph[node];

                foreach( var other in adjacent)
                {
                    foreach( var third in graph[other].Where(adjacent.Contains))
                    {
                        yield return [node, other, third];
                    }
                }
            }
        }

        IEnumerable<string[]> TryGrowSets(IEnumerable<string[]> sets, Dictionary<string, List<string>> graph)
        {
            foreach( var set in sets )
            {
                foreach( var other in graph[set.Last()])
                {
                    if( set.All(n => graph[n].Contains(other)) )
                    {
                        yield return set.Append(other).ToArray();
                    }
                }
            }
        }

        protected override object Solve1(string filename)
        {
            var graph = ParseInput(filename);

            var sets = FindConnectedSets(graph);

            return sets.Count(s => s.Any(c => c.StartsWith('t')));
        }

        protected override object Solve2(string filename)
        {
            var graph = ParseInput(filename);

            var sets = FindConnectedSets(graph).ToList();

            do
            {
                var next = TryGrowSets(sets, graph).ToList();
                if (!next.Any())
                {
                    return string.Join(',', sets.Single());
                }

                sets = next;
            }
            while (true);
        }

        public override object SolutionExample1 => 7;
        public override object SolutionPuzzle1 => 1200;
        public override object SolutionExample2 => "co,de,ka,ta";
        public override object SolutionPuzzle2 => "ag,gh,hh,iv,jx,nq,oc,qm,rb,sm,vm,wu,zr";
    }
}
