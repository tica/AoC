using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2024
{
    public class Day12 : AoC.DayBase
    {
        private IEnumerable<Coord> GrowRegion(Coord from, HashSet<Coord> visited)
        {
            if (visited.Contains(from))
                yield break;

            visited.Add(from);

            yield return from;

            foreach( var c in from.NeighborCoords.Where(c => c.Value == from.Value))
            { 
                foreach( var n in GrowRegion(c, visited))
                {
                    yield return n;
                }
            }
        }

        private List<HashSet<Coord>> FindPlots(Grid grid)
        {
            var result = new List<HashSet<Coord>>();
            var visited = new HashSet<Coord>();

            while (true)
            {
                var c = grid.AllCoordinates.FirstOrDefault(c => !visited.Contains(c));
                if (c == null)
                    break;

                result.Add(new(GrowRegion(c, visited)));
            }

            return result;
        }

        private bool IsFenceBetween(Coord a, Coord b)
        {
            if (!a.IsValid)
                return true;
            if (!b.IsValid)
                return true;

            return a.Value != b.Value;
        }

        private int CalcFenceLength(HashSet<Coord> plot)
        {
            int len = 0;
            foreach( var c in plot)
            {
                if (IsFenceBetween(c, c.Top))
                    len += 1;
                if (IsFenceBetween(c, c.Right))
                    len += 1;
                if (IsFenceBetween(c, c.Bottom))
                    len += 1;
                if (IsFenceBetween(c, c.Left))
                    len += 1;
            }

            return len;
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);
            var plots = FindPlots(grid);

            return plots.Sum(p => CalcFenceLength(p) * p.Count);
        }

        private int CountHorizontalFenceSections(HashSet<Coord> plot)
        {
            int sections = 0;

            foreach( var line in plot.GroupBy(c => c.Y) )
            {
                foreach( var c in line)
                {
                    // . A
                    // B X
                    if (!plot.Contains(c.Left) && !plot.Contains(c.Top))
                        sections += 1;
                    // B X
                    // . A
                    if (!plot.Contains(c.Left) && !plot.Contains(c.Bottom))
                        sections += 1;

                    // X A
                    // X X
                    if (plot.Contains(c.Left) && plot.Contains(c.TopLeft) && !plot.Contains(c.Top))
                        sections += 1;
                    // X X
                    // X A
                    if (plot.Contains(c.Left) && plot.Contains(c.BottomLeft) && !plot.Contains(c.Bottom))
                        sections += 1;
                }
            }

            return sections;
        }

        private int CountVerticalFenceSections(HashSet<Coord> plot)
        {
            int sections = 0;

            foreach (var column in plot.GroupBy(c => c.X))
            {
                foreach (var c in column)
                {
                    // . T
                    // L X
                    if (!plot.Contains(c.Top) && !plot.Contains(c.Left))
                        sections += 1;
                    // T .
                    // X R
                    if (!plot.Contains(c.Top) && !plot.Contains(c.Right))
                        sections += 1;

                    // X X
                    // L X
                    if (plot.Contains(c.Top) && plot.Contains(c.TopLeft) && !plot.Contains(c.Left))
                        sections += 1;
                    // X X
                    // X R
                    if (plot.Contains(c.Top) && plot.Contains(c.TopRight) && !plot.Contains(c.Right))
                        sections += 1;
                }
            }

            return sections;
        }

        private int CountFenceSections(HashSet<Coord> plot)
        {
            return CountHorizontalFenceSections(plot) + CountVerticalFenceSections(plot);
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);
            var plots = FindPlots(grid);

            return plots.Sum(p => CountFenceSections(p) * p.Count);
        }

        public override object SolutionExample1 => 1930;
        public override object SolutionPuzzle1 => 1467094;
        public override object SolutionExample2 => 1206;
        public override object SolutionPuzzle2 => 881182;
    }
}
