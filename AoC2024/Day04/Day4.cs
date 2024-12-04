using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2024
{
    public class Day4 : AoC.DayBase
    {
        private static int CountInternal(IEnumerable<Coord> line)
        {
            int count = 0;
            int correct = 0;

            foreach (var c in line)
            {
                if (c.Value == 'X')
                {
                    correct = 1;
                }
                else if (c.Value == 'M' && correct == 1)
                {
                    correct = 2;
                }
                else if (c.Value == 'A' && correct == 2)
                {
                    correct = 3;
                }
                else if (c.Value == 'S' && correct == 3)
                {
                    correct = 0;
                    count += 1;
                }
                else
                {
                    correct = 0;
                }
            }

            return count;
        }

        private static int Count(IEnumerable<Coord> line)
        {
            return CountInternal(line) + CountInternal(line.Reverse());
        }

        protected override object Solve1(string filename)
        {
            var grid = AoC.Util.GridHelper.Load(filename);

            int count = 0;

            count += grid.Rows.Sum(Count);
            count += grid.Columns.Sum(Count);
            count += grid.Diagonals.Sum(Count);

            return count;
        }

        protected override object Solve2(string filename)
        {
            var grid = AoC.Util.GridHelper.Load(filename);

            int count = 0;

            foreach (var c in grid.AllCoordinates.Where(c => c.Value == 'A' && !c.IsAnyBorder))
            {
                bool downMAS = c.TopLeft.Value == 'M' && c.BottomRight.Value == 'S';
                bool downSAM = c.TopLeft.Value == 'S' && c.BottomRight.Value == 'M';
                bool upMAS = c.BottomLeft.Value == 'M' && c.TopRight.Value == 'S';
                bool upSAM = c.BottomLeft.Value == 'S' && c.TopRight.Value == 'M';

                if ((downMAS || downSAM) && (upMAS || upSAM))
                {
                    count += 1;
                }
            }

            return count;
        }

        public override object SolutionExample1 => 18;
        public override object SolutionPuzzle1 => 2575;
        public override object SolutionExample2 => 9;
        public override object SolutionPuzzle2 => 2041;
    }
}
