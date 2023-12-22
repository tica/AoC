using AoC2023.Util;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Grid = AoC2023.Util.Grid<char>;
using Coord = AoC2023.Util.Grid<char>.Coord;
using Range = AoC2023.Util.Range;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace AoC2023
{
    public class Day21 : DayBase
    {
        public Day21(): base(21) { }

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

            var open = new HashSet<(int X, int Y)>() { (S.X + (W << 10), S.Y + (H << 10)) };
            var next = new HashSet<(int X, int Y)>();

            long prev = 0;
            long prevDelta = 0;
            long prevDeltaDelta = 0;

            for (int i = 1; ; i++)
            {
                foreach (var (x, y) in open)
                {
                    foreach (var (dx, dy) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                    {
                        var xx = x + dx;
                        var yy = y + dy;

                        if (grid[xx % W, yy % H] == '#')
                            continue;

                        next.Add((xx, yy));
                    }
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
