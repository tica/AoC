using AoC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AoC2023
{
    public class Day11 : AoC.DayBase
    {
        public override object SolutionExample1 => 374L;
        public override object SolutionPuzzle1 => 9565386L;
        public override object SolutionExample2 => 1030L;
        public override object SolutionPuzzle2 => 857986849428L;

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);

            for ( int y = 0; y < grid.Height; ++y)
            {
                if( grid.Row(y).All(c => grid[c] == '.') )
                {
                    grid.InsertRow(y, '.');
                    y += 1;
                }
            }
            for (int x = 0; x < grid.Width; ++x)
            {
                if (grid.Column(x).All(c => grid[c] == '.'))
                {
                    grid.InsertColumn(x, '.');
                    x += 1;
                }
            }

            var galaxies = grid.AllCoordinates.Where(p => grid[p] == '#').ToList();

            long sum = 0;

            for( int i = 0; i < galaxies.Count; ++i)
            {
                for( int j = i + 1; j < galaxies.Count; ++j)
                {
                    var g = galaxies[i];
                    var h = galaxies[j];

                    int dist = Math.Abs(h.X - g.X) + Math.Abs(h.Y - g.Y);
                    sum += dist;
                }
            }

            return sum;
        }

        private (int,int) Order(int a, int b)
        {
            if (a < b) return (a, b);
            else return (b, a);
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);

            List<int> emptyRows = new();
            List<int> emptyColumns = new();

            for (int y = 0; y < grid.Height; ++y)
            {
                if (grid.Row(y).All(c => grid[c] == '.'))
                {
                    emptyRows.Add(y);
                }
            }
            for (int x = 0; x < grid.Width; ++x)
            {
                if (grid.Column(x).All(c => grid[c] == '.'))
                {
                    emptyColumns.Add(x);
                }
            }

            var galaxies = grid.AllCoordinates.Where(p => grid[p] == '#').ToList();

            long sum = 0;

            long factor = filename.Contains("example") ? 10 : 1000000;

            for (int i = 0; i < galaxies.Count; ++i)
            {
                for (int j = i + 1; j < galaxies.Count; ++j)
                {
                    var g = galaxies[i];
                    var h = galaxies[j];

                    var (x1, x2) = Order(g.X, h.X);
                    var (y1, y2) = Order(g.Y, h.Y);

                    long dx = x2 - x1;
                    long dy = y2 - y1;

                    int growX = emptyColumns.Count(x => (x > x1) && (x < x2));
                    int growY = emptyRows.Count(y => (y > y1) && (y < y2));

                    dx += (growX * (factor-1));
                    dy += (growY * (factor-1));

                    sum += (dx + dy);
                }
            }

            return sum;
        }
    }
}
