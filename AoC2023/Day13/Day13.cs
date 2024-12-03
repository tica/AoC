using AoC.Util;

namespace AoC2023
{
    public class Day13 : AoC.DayBase
    {
        public override object SolutionExample1 => 405L;
        public override object SolutionPuzzle1 => 35232L;
        public override object SolutionExample2 => 400L;
        public override object SolutionPuzzle2 => 37982L;

        private int? FindReflection(List<ulong> arr)
        {
            int h = arr.Count;
            for (int y = 1; y < h; ++y)
            {
                bool isMirrored = true;
                for (int i = 0; i < y && i < (h - y); ++i)
                {
                    if (arr[y + i] != arr[y - 1 - i])
                    {
                        isMirrored = false;
                        break;
                    }
                }
                if (isMirrored)
                {
                    return y;
                }
            }
            return null;
        }

        private long FindReflections(Grid<char> grid)
        {
            int w = grid.Width;
            int h = grid.Height;

            var rows = Enumerable.Range(0, h).Select(y => grid.RowBits(y, c => c == '#')).ToList();
            var columns = Enumerable.Range(0, w).Select(x => grid.ColumnBits(x, c => c == '#')).ToList();

            int? ry = FindReflection(rows);
            int? rx = FindReflection(columns);

            return (ry ?? 0) * 100 + (rx ?? 0);
        }

        protected override object Solve1(string filename)
        {
            long sum = 0;
            foreach (var grid in GridHelper.LoadMultiple(filename))
            {
                var rc = FindReflections(grid);
                sum += rc;
            }
            return sum;
        }

        private int? FindDefectiveReflection(List<ulong> arr)
        {
            int h = arr.Count;
            for (int y = 1; y < h; ++y)
            {
                int numErrors = 0;
                for (int i = 0; i < y && i < (h - y); ++i)
                {
                    var xor = arr[y + i] ^ arr[y - 1 - i];
                    numErrors += System.Numerics.BitOperations.PopCount(xor);
                    if( numErrors > 1 )
                        break;
                }
                if (numErrors == 1)
                {
                    return y;
                }
            }
            return null;
        }

        private long FindDefectiveReflections(Grid<char> grid)
        {
            int w = grid.Width;
            int h = grid.Height;

            var rows = Enumerable.Range(0, h).Select(y => grid.RowBits(y, c => c == '#')).ToList();
            var columns = Enumerable.Range(0, w).Select(x => grid.ColumnBits(x, c => c == '#')).ToList();

            int? ry = FindDefectiveReflection(rows);
            int? rx = FindDefectiveReflection(columns);

            return (ry ?? 0) * 100 + (rx ?? 0);
        }

        protected override object Solve2(string filename)
        {
            long sum = 0;
            foreach (var grid in GridHelper.LoadMultiple(filename))
            {
                var rc = FindDefectiveReflections(grid);
                sum += rc;
            }
            return sum;
        }
    }
}
