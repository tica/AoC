using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2024
{
    public class Day8 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);
            var antennaTypes = grid.AllCoordinates.Select(c => c.Value).Distinct().Where(v => v != '.');
            var locations = new HashSet<Coord>();

            foreach (var a in antennaTypes)
            {
                var coords = grid.WhereValue(a);

                foreach (var (p, q) in coords.Pairwise())
                {
                    var d = p - q;

                    locations.Add(p + d);
                    locations.Add(q - d);
                }
            }

            return locations.Where(p => p.IsValid).Count();
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);
            var antennaTypes = grid.AllCoordinates.Select(c => c.Value).Distinct().Where(v => v != '.');
            var locations = new HashSet<Coord>();

            foreach (var a in antennaTypes)
            {
                var coords = grid.WhereValue(a);

                foreach (var (p, q) in coords.Pairwise())
                {
                    var d = (p - q).Normalized;

                    for (var pp = p; pp.IsValid; pp += d)
                    {
                        locations.Add(pp);
                    }
                    for (var pp = p; pp.IsValid; pp -= d)
                    {
                        locations.Add(pp);
                    }
                }
            }

            return locations.Count;
        }

        public override object SolutionExample1 => 14;
        public override object SolutionPuzzle1 => 381;
        public override object SolutionExample2 => 34;
        public override object SolutionPuzzle2 => 1184;
    }
}
