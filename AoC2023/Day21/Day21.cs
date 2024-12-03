using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2023
{
    public class Day21 : AoC.DayBase
    {
        public override object SolutionExample1 => 16L;
        public override object SolutionPuzzle1 => 3666L;
        public override object SolutionExample2 => 16733044L;
        public override object SolutionPuzzle2 => 609298746763952L;

        protected override object Solve1(string filename)
        {
            int STEPS = filename.Contains("example") ? 6 : 64;

            var grid = GridHelper.Load(filename);
            var S = grid.AllCoordinates.Single(c => c.Value == 'S');

            var open = new HashSet<Coord>() { S };
            var next = new HashSet<Coord>();

            for ( int i = 0; i < STEPS; i++ )
            {
                foreach (var p in open)
                {
                    foreach (var n in p.NeighborCoords)
                    {
                        if (n.Value == '#')
                            continue;

                        next.Add(n);
                    }
                }

                open = next;
                next = new();
            }

            return (long)open.Count;
        }

        private (long step, long count, long delta, long deltadelta) ConvergeInfiniteSearch(Grid grid, int targetSteps)
        {
            var W = grid.Width;
            var H = grid.Height;

            var S = grid.AllCoordinates.Single(c => c.Value == 'S');

            var open = new HashSet<int>() { ((S.X + (W << 8)) << 16) | (S.Y + (H << 8)) };
            var next = new HashSet<int>();

            long prev = 0;
            long prevDelta = 0;
            long prevDeltaDelta = 0;

            for (int i = 1; ; i++)
            {
                foreach (var xy in open)
                {
                    int x = (xy >> 16) & 0xFFFF;
                    int y = xy & 0xFFFF;

                    if (grid[(x - 1) % W, y % H] != '#') next.Add(((x - 1) << 16) | y);
                    if (grid[(x + 1) % W, y % H] != '#') next.Add(((x + 1) << 16) | y);
                    if (grid[x % W, (y - 1) % H] != '#') next.Add(x << 16 | (y - 1));
                    if (grid[x % W, (y + 1) % H] != '#') next.Add(x << 16 | (y + 1));
                }

                open = next;
                next = new();

                if ((targetSteps - i) % W == 0)
                {
                    long delta = open.Count - prev;
                    long deltaDelta = delta - prevDelta;
                    Console.WriteLine($"step {i} count {open.Count} ({delta}) ({deltaDelta})");

                    if (deltaDelta == prevDeltaDelta)
                    {
                        return (i, open.Count, delta, deltaDelta);
                    }

                    prev = open.Count;
                    prevDeltaDelta = deltaDelta;
                    prevDelta = delta;
                }
            }
        }

        protected override object Solve2(string filename)
        {
            int STEPS = filename.Contains("example") ? 5000 : 26501365;

            var grid = GridHelper.Load(filename);

            var (step, count, delta, deltaDelta) = ConvergeInfiniteSearch(grid, STEPS);

            for (long i = step; i < STEPS; i += grid.Width)
            {
                delta += deltaDelta;
                count += delta;
            }

            return count;
        }
    }
}
