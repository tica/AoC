using AoC.Util;
using static System.Net.Mime.MediaTypeNames;
using Grid = AoC.Util.Grid<char>;

namespace AoC2015
{
    public class Day18 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);

            for (int i = 0; i < 100; ++i)
            {
                var next = new Grid(grid.Width, grid.Height, '.');

                foreach (var c in grid.AllCoordinates)
                {
                    int n = c.AdjacentCoords.Count(cc => cc.Value == '#');
                    if (c.Value == '#' && (n == 2 || n == 3))
                    {
                        next.Set(c, '#');
                    }
                    else if (c.Value == '.' && n == 3)
                    {
                        next.Set(c, '#');
                    }
                    else
                    {
                        next.Set(c, '.');
                    }
                }

                grid = next;
            }

            return grid.AllValues.Count(v => v == '#');
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);

            grid.Set(grid.Rows.First().First(), '#');
            grid.Set(grid.Rows.First().Last(), '#');
            grid.Set(grid.Rows.Last().First(), '#');
            grid.Set(grid.Rows.Last().Last(), '#');

            for (int i = 0; i < 100; ++i)
            {
                var next = new Grid(grid.Width, grid.Height, '.');

                foreach (var c in grid.AllCoordinates)
                {
                    int n = c.AdjacentCoords.Count(cc => cc.Value == '#');
                    if (c.Value == '#' && (n == 2 || n == 3))
                    {
                        next.Set(c, '#');
                    }
                    else if (c.Value == '.' && n == 3)
                    {
                        next.Set(c, '#');
                    }
                    else
                    {
                        next.Set(c, '.');
                    }
                }

                next.Set(next.Rows.First().First(), '#');
                next.Set(next.Rows.First().Last(), '#');
                next.Set(next.Rows.Last().First(), '#');
                next.Set(next.Rows.Last().Last(), '#');

                grid = next;
            }

            return grid.AllValues.Count(v => v == '#');
        }

        public override object SolutionExample1 => 4;
        public override object SolutionPuzzle1 => 814;
        public override object SolutionExample2 => 7;
        public override object SolutionPuzzle2 => 924;
    }
}