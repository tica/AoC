using Grid = AoC.Util.Grid<int>;
using Coord = AoC.Util.Grid<int>.Coord;
using AoC.Util;
using System.Runtime.Intrinsics.Arm;

namespace AoC2024
{
    public class Day20 : AoC.DayBase
    {
        int Solve(string filename, int maxCheatLength, int improvementLimit)
        {
            var input = GridHelper.Load(filename);

            var start = input.WhereValue('S').Single();
            var end = input.WhereValue('E').Single();
            var originalPath = AStar.FindPath(start, end,
                n => n.NeighborCoords.Where(c => c.Value != '#').Select(c => (c, 1)),
                n => n.ManhattanDistance(end)
            );

            var grid = new Grid(input.Width, input.Height, -1);
            var path = originalPath.Select(c => new Coord(grid, c.X, c.Y)).ToList();
            int pathLength = path.Count - 1;

            for (int i = 0; i < originalPath.Count; ++i)
            {
                grid.Set(path[i], pathLength - i);
            }

            int count = 0;
            foreach (var p in path)
            {
                foreach (var pp in p.WithinRange(maxCheatLength))
                {
                    int save = pp.Value - p.Value - pp.ManhattanDistance(p);
                    if (save >= improvementLimit)
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }

        protected override object Solve1(string filename)
        {
            return Solve(filename, 2, filename.Contains("example") ? 10 : 100);
        }

        protected override object Solve2(string filename)
        {
            return Solve(filename, 20, filename.Contains("example") ? 50 : 100);
        }

        public override object SolutionExample1 => 10;
        public override object SolutionPuzzle1 => 1375;
        public override object SolutionExample2 => 285;
        public override object SolutionPuzzle2 => 983054;
    }
}
