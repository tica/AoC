
using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2024
{
    public class Day16 : AoC.DayBase
    {
        private IEnumerable<((Coord Pos, Direction Dir) Node, int Cost)> EnumNextNodes((Coord c, Direction dir) n)
        {
            if( n.c.Neighbor(n.dir).Value != '#' )
            {
                yield return ((n.c.Neighbor(n.dir), n.dir), 1);
            }
            if( n.c.Neighbor(n.dir.TurnRight()).Value != '#')
            {
                yield return ((n.c, n.dir.TurnRight()), 1000);
            }
            if (n.c.Neighbor(n.dir.TurnLeft()).Value != '#')
            {
                yield return ((n.c, n.dir.TurnLeft()), 1000);
            }
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);
            var start = (Position: grid.AllCoordinates.Single(c => c.Value == 'S'), Direction: Direction.Right);
            var end = grid.AllCoordinates.Single(c => c.Value == 'E');

            var path = AStar.FindPath(start,
                t => t.Item1.Value == 'E',
                EnumNextNodes,
                n => Math.Abs(n.Item1.X - end.X) + Math.Abs(n.Item1.Y - end.Y)
            );

            int turns = path.Window2().Count(t => t.Left.Direction != t.Right.Direction);
            int steps = path.Window2().Count(t => t.Left.Position != t.Right.Position);

            return turns * 1000 + steps;
        }

        void FindAllPaths((Coord Pos, Direction Dir) from, int curLength, int maxLength, IEnumerable<Coord> path, List<List<Coord>> output, Dictionary<(Coord, Direction), int> shortestFound)
        {
            if (curLength > maxLength)
                return;

            if (from.Pos.Value == 'E')
            {
                output.Add(path.ToList());
                return;
            }

            if( shortestFound.TryGetValue(from, out var bestLength) )
            {
                if( curLength > bestLength )
                {
                    return;
                }
            }

            shortestFound[from] = curLength;

            foreach (var n in EnumNextNodes(from))
            {
                FindAllPaths(n.Node, curLength + n.Cost, maxLength, path.Append(n.Node.Pos), output, shortestFound);
            }
        }

        protected override object Solve2(string filename)
        {
            var maxLength = (int)Solve1(filename);

            var grid = GridHelper.Load(filename);
            var start = (Position: grid.AllCoordinates.Single(c => c.Value == 'S'), Direction: Direction.Right);

            var result = new List<List<Coord>>();
            FindAllPaths(start, 0, maxLength, new List<Coord> { start.Position }, result, new());

            return result.SelectMany(x => x).Distinct().Count();
        }

        public override object SolutionExample1 => 7036;
        public override object SolutionPuzzle1 => 99488;
        public override object SolutionExample2 => 64;
        public override object SolutionPuzzle2 => 516;
    }
}
