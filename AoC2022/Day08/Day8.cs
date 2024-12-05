using Coord = AoC.Util.Grid<int>.Coord;
using Direction = AoC.Util.Direction;

namespace AoC2022
{
    public class Day8 : AoC.DayBase
    {
        bool IsVisibleFromDirection(Coord p, Direction dir)
        {
            int h = p.Value;

            p = p.Neighbor(dir);

            while( p.IsValid )
            {
                if (p.Value >= h)
                    return false;

                p = p.Neighbor(dir);
            }

            return true;
        }

        bool IsVisibleFromOutside(Coord p)
        {
            return IsVisibleFromDirection(p, Direction.Up)
                || IsVisibleFromDirection(p, Direction.Left)
                || IsVisibleFromDirection(p, Direction.Down)
                || IsVisibleFromDirection(p, Direction.Right);
        }

        protected override object Solve1(string filename)
        {
            var grid = AoC.Util.GridHelper.Load(filename, ch => ch - '0');

            return grid.AllCoordinates.Count(IsVisibleFromOutside);
        }

        private int CalcViewingDistance(Coord p, Direction dir)
        {
            int initial = p.Value;
            int distance = 0;

            do
            {
                p = p.Neighbor(dir);

                if (!p.IsValid)
                    break;

                distance += 1;
            }
            while (p.Value < initial);

            return distance;
        }

        private int CalcScenicScore(Coord p)
        {
            var up = CalcViewingDistance(p, Direction.Up);
            var right = CalcViewingDistance(p, Direction.Right);
            var down = CalcViewingDistance(p, Direction.Down);
            var left = CalcViewingDistance(p, Direction.Left);

            return up * right * down * left;
        }

        protected override object Solve2(string filename)
        {
            var grid = AoC.Util.GridHelper.Load(filename, ch => ch - '0');

            return grid.AllCoordinates.Max(CalcScenicScore);
        }

        public override object SolutionExample1 => 21;
        public override object SolutionPuzzle1 => 1705;
        public override object SolutionExample2 => 8;
        public override object SolutionPuzzle2 => 371200;
    }
}
