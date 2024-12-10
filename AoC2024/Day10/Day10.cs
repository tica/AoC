using Grid = AoC.Util.Grid<int>;
using Coord = AoC.Util.Grid<int>.Coord;

namespace AoC2024
{
    public class Day10 : AoC.DayBase
    {
        void FindUphillDestinations(Coord from, HashSet<Coord> dest)
        {
            if (from.Value == 9)
            {
                dest.Add(from);
                return;
            }

            foreach (var c in from.NeighborCoords)
            {
                if (c.Value == from.Value + 1)
                {
                    FindUphillDestinations(c, dest);
                }
            }
        }

        protected override object Solve1(string filename)
        {
            var grid = AoC.Util.GridHelper.Load(filename, ch => ch == '.' ? -1 : ch - '0');

            return grid.WhereValue(0).Sum(
                c =>
                {
                    var dest = new HashSet<Coord>();
                    FindUphillDestinations(c, dest);
                    return dest.Count;
                }
            );
        }


        int CountUphillTrails(Coord from)
        {
            if (from.Value == 9)
                return 1;

            return from.NeighborCoords.Where(c => c.Value == from.Value + 1).Sum(CountUphillTrails);
        }

        protected override object Solve2(string filename)
        {
            var grid = AoC.Util.GridHelper.Load(filename, ch => ch - '0');

            return grid.WhereValue(0).Sum(CountUphillTrails);
        }

        public override object SolutionExample1 => 36;
        public override object SolutionPuzzle1 => 538;
        public override object SolutionExample2 => 81;
        public override object SolutionPuzzle2 => 1110;
    }
}
