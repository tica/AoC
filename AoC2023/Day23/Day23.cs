using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

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

            var newSeen = new HashSet<Coord>(seen);
            newSeen.Add(from);

            int longest = 0;

            if( !seen.Contains(from.Left) && (from.Left.Value == '.' || from.Left.Value == '<') )
            {
                longest = Math.Max(longest, FindLongestPath(grid, from.Left, to, newSeen));
            }
            if (!seen.Contains(from.Right) && (from.Right.Value == '.' || from.Right.Value == '>'))
            {
                longest = Math.Max(longest, FindLongestPath(grid, from.Right, to, newSeen));
            }
            if (!seen.Contains(from.Bottom) && (from.Bottom.Value == '.' || from.Bottom.Value == 'v'))
            {
                longest = Math.Max(longest, FindLongestPath(grid, from.Bottom, to, newSeen));
            }
            if (!seen.Contains(from.Top) && from.Top.IsValid && (from.Top.Value == '.' || from.Top.Value == '^'))
            {
                longest = Math.Max(longest, FindLongestPath(grid, from.Top, to, newSeen));
            }

            return 1 + longest;
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);

            var S = grid.Rows.First().Single(p => p.Value == '.');
            var G = grid.Rows.Last().Single(p => p.Value == '.');

            return FindLongestPath(grid, S, G, new HashSet<Coord>());
        }

        private int FindLongestPath2(Dictionary<Coord, List<(Coord vertex, int dist)>> graph, Coord from, Coord to, HashSet<Coord> seen, int len)
        {
            if (seen.Contains(from))
                return -1;

            if (from == to)
                return len;

            seen.Add(from);

            var longest = graph[from].Max(n => FindLongestPath2(graph, n.vertex, to, seen, len + n.dist));

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

            return FindLongestPath2(graph, S, G, new(), 0);
        }
    }
}
