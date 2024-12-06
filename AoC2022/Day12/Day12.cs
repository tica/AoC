using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2022
{
    public class Day12 : AoC.DayBase
    {
        private static bool CanWalkUp(Coord from, Coord to)
        {
            var toVal = (to.Value == 'E') ? 'z' : to.Value;
            var fromVal = (from.Value == 'S') ? 'a' : from.Value;

            return (toVal - fromVal) <= 1;
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);

            var start = grid.AllCoordinates.Single(c => c.Value == 'S');
            var end = grid.AllCoordinates.Single(c => c.Value == 'E');

            var path = AStar.FindPath(start, end,
                p => p.NeighborCoords.Where(q => CanWalkUp(p, q)).Select(q => (q, 1)),
                p => grid.Distance(p, end));
            
            return path.Count - 1;
        }

        public static List<TNode>? FindFirst<TNode>(TNode from, Func<TNode, IEnumerable<TNode>> enumNeighbors, Func<TNode, bool> isDestination) where TNode : notnull
        {
            Dictionary<TNode, TNode?> shortestTo = new() { [from] = default };
            Queue<TNode> open = new([from]);

            while (open.Count > 0)
            {
                var current = open.Dequeue();

                foreach (var node in enumNeighbors(current))
                {
                    if (isDestination(node))
                    {
                        List<TNode> path = new() { node };
                        while( true )
                        {
                            path.Add(current);
                            var pre = shortestTo[current];
                            if (pre == null)
                            {
                                return Enumerable.Reverse(path).ToList();
                            }
                            current = pre;
                        }
                    }

                    if (shortestTo.ContainsKey(node))
                    {
                        continue;
                    }

                    shortestTo[node] = current;
                    open.Enqueue(node);
                }
            }

            return null;
        }

        private static bool CanWalkDown(Coord from, Coord to)
        {
            var toVal = (to.Value == 'S') ? 'a' : to.Value;
            var fromVal = (from.Value == 'E') ? 'z' : from.Value;

            return (fromVal - toVal) <= 1;
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);

            var end = grid.AllCoordinates.Single(c => c.Value == 'E');

            var path = FindFirst(end,
                p => p.NeighborCoords.Where(q => CanWalkDown(p, q)),
                p => p.Value == 'a')!;

            return path.Count - 1;
        }

        public override object SolutionExample1 => 31;
        public override object SolutionPuzzle1 => 490;
        public override object SolutionExample2 => 29;
        public override object SolutionPuzzle2 => 488;
    }
}
