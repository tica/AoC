using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC.Util;

namespace AoC2025
{
    public class Day4 : AoC.DayBase
    {
        private bool IsFilledAndAccessible(Grid<char>.Coord c)
        {
            return c.Value == '@' && c.AdjacentCoords.Count(n => n.Value == '@') < 4;
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);

            return grid.AllCoordinates.Count(IsFilledAndAccessible);
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);

            int totalRemoved = 0;

            while ( true )
            {
                var allAccessible = grid.AllCoordinates.Where(IsFilledAndAccessible).ToList();
                if (!allAccessible.Any())
                {
                    return totalRemoved;
                }

                foreach (var c in allAccessible)
                {
                    grid.Set(c, '.');
                }

                totalRemoved += allAccessible.Count;
            }
        }

        public override object SolutionExample1 => 13;
        public override object SolutionPuzzle1 => 1441;
        public override object SolutionExample2 => 43;
        public override object SolutionPuzzle2 => 9050;
    }
}
