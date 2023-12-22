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
                    grid.Set(p, '.');

                    foreach (var n in p.NeighborCoords)
                    {
                        if (n.Value == '#')
                            continue;
                        if (next.Contains(n))
                            continue;

                        grid.Set(n, 'O');

                        next.Add(n);
                    }
                }

                open = next;
                next = new();
            }

            return (long)open.Count;
        }

        private Grid LoadConcat(string filename, int concat)
        {
            var lines = System.IO.File.ReadAllLines(filename).ToList();

            var data = new List<List<char>>();

            for (int i = 0; i < concat; ++i)
            {
                foreach (var line in lines)
                {
                    var sb = new StringBuilder();
                    for (int k = 0; k < concat; ++k)
                    {
                        if (i != concat / 2 || k != concat / 2)
                            sb.Append(line.Replace('S', '.'));
                        else
                            sb.Append(line);
                    }

                    data.Add(sb.ToString().ToList());
                }
            }

            return new Grid<char>(data);
        }

        protected override object Solve2(string filename)
        {
            int STEPS = filename.Contains("example") ? 5000 : 26501365;

            var W = GridHelper.Load(filename).Width;

            var grid = LoadConcat(filename, 17);
            var S = grid.AllCoordinates.Single(c => c.Value == 'S');

            var open = new HashSet<Coord>() { S };
            var next = new HashSet<Coord>();

            long prev = 0;
            long prevDelta = 0;
            long prevDeltaDelta = 0;

            int deltaDeltaUnchanged = 0;

            var lst = new List<(int,long)>();

            for (int i = 0; lst.Count < 1; i++)
            {
                foreach (var p in open)
                {
                    grid.Set(p, '.');

                    foreach (var n in p.NeighborCoords)
                    {
                        if (n.Value == '#')
                            continue;
                        if (next.Contains(n))
                            continue;

                        grid.Set(n, 'O');

                        next.Add(n);
                    }
                }

                open = next;
                next = new();

                if ((STEPS - i - 1) % W == 0)
                {
                    long delta = open.Count - prev;
                    long deltaDelta = delta - prevDelta;
                    Console.WriteLine($"step {i + 1} count {open.Count} ({delta}) ({deltaDelta})");

                    if( deltaDelta == prevDeltaDelta)
                    {
                        deltaDeltaUnchanged += 1;

                        if(deltaDeltaUnchanged > 0 )
                        {
                            lst.Add((i + 1, open.Count));
                        }
                    }
                    else
                    {
                        deltaDeltaUnchanged = 0;
                    }

                    prev = open.Count;
                    prevDeltaDelta = deltaDelta;
                    prevDelta = delta;
                }
            }

            var steps = lst.Select(t => t.Item1).ToList();
            var nums = lst.Select(t => t.Item2).ToList();

            long d = prevDelta;
            long result = nums.Last();

            for ( int i = steps.Last(); i < STEPS; i += W)
            {
                d += prevDeltaDelta;
                result += d;
            }

            return result;
        }
    }
}
