
using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2024
{
    public class Day16 : AoC.DayBase
    {
        private IEnumerable<((Coord, Direction), int)> EnumNextNodes((Coord c, Direction dir) n)
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
            var start = (grid.AllCoordinates.Single(c => c.Value == 'S'), Direction.Right);
            var end = grid.AllCoordinates.Single(c => c.Value == 'E');

            var path = AStar.FindPath(start,
                t => t.Item1.Value == 'E',
                EnumNextNodes,
                n => Math.Abs(n.Item1.X - end.X) + Math.Abs(n.Item1.Y - end.Y)
            );

            //grid.Print(c => path.Any(p => p.Item1 == c) ? ConsoleColor.Red : ConsoleColor.Gray);

            int turns = path.Window2().Count(t => t.Item1.Item2 != t.Item2.Item2);
            int steps = path.Window2().Count(t => t.Item1.Item1 != t.Item2.Item1);

            return turns * 1000 + steps;
        }

        protected override object Solve2(string filename)
        {
            return null!;
        }

        public override object SolutionExample1 => 7036;
        public override object SolutionPuzzle1 => 99488;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;
    }
}
