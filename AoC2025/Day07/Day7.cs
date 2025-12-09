using AoC.Util;

using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2025
{
    public class Day7 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);
            var start = grid.WhereValue('S').Single();
            var beams = new HashSet<Coord> { start };
            var nextbeams = new HashSet<Coord>();

            int numsplits = 0;

            while (!beams.First().IsBottomBorder)
            {
                foreach (var b in beams)
                {
                    switch (b.Bottom.Value)
                    {
                        case '^':
                            nextbeams.Add(b.BottomLeft);
                            nextbeams.Add(b.BottomRight);
                            numsplits += 1;
                            break;
                        case '.':
                            nextbeams.Add(b.Bottom);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                beams = nextbeams;
                nextbeams = new();
            }

            return numsplits;
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);
            var start = grid.WhereValue('S').Single();
            var beams = new CountingSet<Coord>();
            var nextbeams = new CountingSet<Coord>();

            beams.Add(start, 1);

            while (!beams.First().Key.IsBottomBorder)
            {
                foreach (var (b, c) in beams)
                {
                    switch (b.Bottom.Value)
                    {
                        case '^':
                            nextbeams.Add(b.BottomLeft, c);
                            nextbeams.Add(b.BottomRight, c);
                            break;
                        case '.':
                            nextbeams.Add(b.Bottom, c);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                beams = nextbeams;
                nextbeams = new();
            }

            return beams.Values.Sum();
        }

        public override object SolutionExample1 => 21;
        public override object SolutionPuzzle1 => 1681;
        public override object SolutionExample2 => 40L;
        public override object SolutionPuzzle2 => 422102272495018L;
    }
}
