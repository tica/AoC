using AoC2023.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
{
    public class Day3 : DayBase
    {
        public Day3() : base(3) { }

        private static int ExtractAndClearNumber(Grid.Coord p)
        {
            var grid = p.Parent;

            while (!p.IsLeftBorder && char.IsDigit(grid[p.Left]))
                --p;

            string s = "";

            while (p.IsValid && char.IsDigit(grid[p]))
            {
                s += grid.Exchange(p++, ' ');
            }

            return int.Parse(s);
        }

        protected override object Solve1(string filename)
        {
            var grid = new Grid(filename);

            int sum = 0;
            foreach ( var p in grid.AllCoordinates)
            {
                char c = grid[p];
                if (char.IsDigit(c))
                    continue;
                if (c == '.')
                    continue;
                if (c == ' ')
                   continue;


                foreach( var q in p.AdjacentCoords)
                {
                    if (char.IsDigit(grid[q]))
                        sum += ExtractAndClearNumber(q);
                }
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            var grid = new Grid(filename);

            long sum = 0;
            foreach (var p in grid.AllCoordinates)
            {
                if (grid[p] != '*')
                    continue;

                var gears = new List<int>();

                foreach (var q in p.AdjacentCoords)
                {
                    if (char.IsDigit(grid[q]))
                        gears.Add(ExtractAndClearNumber(q));
                }

                if (gears.Count == 2)
                    sum += gears[0] * gears[1];
            }

            return sum;
        }

        public override object SolutionExample1 => 4361;
        public override object SolutionPuzzle1 => 554003;
        public override object SolutionExample2 => 467835L;
        public override object SolutionPuzzle2 => 87263515L;
    }
}
