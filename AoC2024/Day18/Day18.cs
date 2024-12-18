using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2024
{
    public class Day18 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            (int X, int Y, int N) Size = filename.Contains("example") ? (7, 7, 12) : (71, 71, 1024);
            var grid = new Grid(Size.X, Size.Y, '.');

            var input = File.ReadAllLines(filename).Select(line => line.Split(',').Select(int.Parse)).Select(e => new Coord(grid, e.First(), e.Last()));

            var start = grid.Row(0).First();
            var goal = grid.Rows.Last().Last();

            foreach (var p in input.Take(Size.N))
            {
                grid.Set(p, '#');
            }

            var path = AStar.FindPath(start, goal,
                n => n.NeighborCoords.Where(c => c.Value == '.').Select(c => (c, 1)),
                n => Math.Abs(n.X - goal.X) + Math.Abs(n.Y - goal.Y)
            );

            return path.Count - 1;
        }

        protected override object Solve2(string filename)
        {
            (int X, int Y) Size = filename.Contains("example") ? (7, 7) : (71, 71);
            var grid = new Grid(Size.X, Size.Y, '.');

            var input = File.ReadAllLines(filename).Select(line => line.Split(',').Select(int.Parse)).Select(e => new Coord(grid, e.First(), e.Last()));

            var start = grid.Row(0).First();
            var goal = grid.Rows.Last().Last();

            var path = AStar.FindPath(start, goal,
                n => n.NeighborCoords.Where(c => c.Value == '.').Select(c => (c, 1)),
                n => Math.Abs(n.X - goal.X) + Math.Abs(n.Y - goal.Y)
            );

            foreach (var p in input)
            {
                grid.Set(p, '#');

                if( path.Contains(p) )
                {
                    path = AStar.FindPath(start, goal,
                        n => n.NeighborCoords.Where(c => c.Value == '.').Select(c => (c, 1)),
                        n => Math.Abs(n.X - goal.X) + Math.Abs(n.Y - goal.Y)
                    );
                    if (path == null)
                    {
                        return $"{p.X},{p.Y}";
                    }
                }
            }

            return null!;
        }

        public override object SolutionExample1 => 22;
        public override object SolutionPuzzle1 => 290;
        public override object SolutionExample2 => "6,1";
        public override object SolutionPuzzle2 => "64,54";
    }
}
