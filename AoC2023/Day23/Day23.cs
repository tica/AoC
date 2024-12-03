using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;
using System.Numerics;

namespace AoC2023
{
    public class Day23 : AoC.DayBase
    {
        public override object SolutionExample1 => 94;
        public override object SolutionPuzzle1 => 2110;
        public override object SolutionExample2 => 154;
        public override object SolutionPuzzle2 => 6514;


        private int FindLongestPath(Grid grid, Coord from, Coord to, HashSet<Coord> seen)
        {
            if (from == to)
                return 0;

            seen.Add(from);

            int longest = 0;

            if( !seen.Contains(from.Left) && (from.Left.Value == '.' || from.Left.Value == '<') )
            {
                longest = Math.Max(longest, FindLongestPath(grid, from.Left, to, seen));
            }
            if (!seen.Contains(from.Right) && (from.Right.Value == '.' || from.Right.Value == '>'))
            {
                longest = Math.Max(longest, FindLongestPath(grid, from.Right, to, seen));
            }
            if (!seen.Contains(from.Bottom) && (from.Bottom.Value == '.' || from.Bottom.Value == 'v'))
            {
                longest = Math.Max(longest, FindLongestPath(grid, from.Bottom, to, seen));
            }
            if (!seen.Contains(from.Top) && from.Top.IsValid && (from.Top.Value == '.' || from.Top.Value == '^'))
            {
                longest = Math.Max(longest, FindLongestPath(grid, from.Top, to, seen));
            }

            seen.Remove(from);

            return 1 + longest;
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);

            var S = grid.Rows.First().Single(p => p.Value == '.');
            var G = grid.Rows.Last().Single(p => p.Value == '.');

            return FindLongestPath(grid, S, G, new HashSet<Coord>());
        }

        private int FindLongestPath2<TNode>(Dictionary<TNode, List<(TNode vertex, int dist)>> graph, TNode from, TNode to, HashSet<TNode> seen, int len) where TNode: notnull
        {
            if (from.Equals(to))
                return len;

            seen.Add(from);

            var longest = graph[from].Max(
                n => {
                    if( seen.Contains(n.vertex) )
                        return -1;
                    return FindLongestPath2(graph, n.vertex, to, seen, len + n.dist);                    
                }
            );

            seen.Remove(from);

            return longest;
        }

        
        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);
            var S = grid.Rows.First().Single(p => p.Value == '.');
            var G = grid.Rows.Last().Single(p => p.Value == '.');

            var graph = grid.AllCoordinates
                .Where(p => p.Value != '#')
                .ToDictionary(
                    p => p,
                    p => p.NeighborCoords.Where(c => c.Value != '#').Select(c => (vertex: c, dist: 1)).ToList()
                );

            while( true )
            {
                var n = graph.FirstOrDefault(kvp => kvp.Value.Count == 2);
                if (n.Key == null)
                {
                    break;
                }

                var center = n.Key;
                var left = n.Value.First();
                var right = n.Value.Last();
                var len = n.Value.Sum(v => v.dist);

                graph[left.vertex].RemoveAll(v => v.vertex == center);
                graph[right.vertex].RemoveAll(v => v.vertex == center);
                graph[left.vertex].Add((right.vertex, len));
                graph[right.vertex].Add((left.vertex, len));

                graph.Remove(center);
            }

            // Rebuild graph structure, replacing Coord objects with just numbers
            var mapToNumbers = graph.Select((node, index) => (node, index)).ToDictionary(p => p.node.Key, p => p.index);
            var numbersGraph = graph.ToDictionary(kvp => mapToNumbers[kvp.Key], kvp => kvp.Value.Select(lst => (mapToNumbers[lst.vertex], lst.dist)).ToList());

            return FindLongestPath2(numbersGraph, mapToNumbers[S], mapToNumbers[G], new(), 0);
        }
    }
}
