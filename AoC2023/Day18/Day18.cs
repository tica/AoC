using AoC2023.Util;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Grid = AoC2023.Util.Grid<char>;

namespace AoC2023
{
    public class Day18 : DayBase
    {
        public Day18(): base(18) { }

        public override object SolutionExample1 => throw new NotImplementedException();
        public override object SolutionPuzzle1 => throw new NotImplementedException();
        public override object SolutionExample2 => throw new NotImplementedException();
        public override object SolutionPuzzle2 => throw new NotImplementedException();

        private static Direction ToDirection(string dir)
        {
            switch(dir)
            {
                case "L": return Direction.Left;
                case "R": return Direction.Right;
                case "U": return Direction.Up;
                case "D": return Direction.Down;
            }
            throw new Exception("oops");
        }

        class Wall
        {
            public bool Upper;
            public long Y;
            public long X0;
            public long X1;

            public override string ToString()
            {
                return $"{(Upper ? 'U' : 'L')} X{X0}..{X1} Y{Y}";
            }
        }

        class Event
        {
            public bool Begin;
            public Wall Wall;

            public long X => Begin ? Wall.X0 : Wall.X1;

            public override string ToString()
            {
                return $"{X}: {(Begin ? "Begin" : "End")} {Wall}";
            }
        }

        private bool InsideActive(List<Wall> walls, long y)
        {
            for (int i = 0; i < walls.Count; i += 2)
            {
                if (y > walls[i].Y && y < walls[i + 1].Y)
                    return true;
            }
            return false;
        }

        record struct Command(Direction Dir, int Distance)
        {
            public static Command Parse(string line)
            {
                var m = Regex.Match(line, @"([RLDU]) (\d+) \(#([a-f0-9]+)\)");
                var dir = ToDirection(m.Groups[1].Value);
                var dist = int.Parse(m.Groups[2].Value);
                return new Command { Dir = dir, Distance = dist };
            }
        }

        record class Range(long Begin, long Length)
        {
            public long End => Begin + Length;

            public bool Contains(long val) => val >= Begin && val < End;

            public Range Intersect(Range other)
            {
                var begin = Math.Max(Begin, other.Begin);
                var end = Math.Min(End, other.End);
                if (begin >= end)
                    throw new Exception("No intersection");

                return new Range(begin, end - begin);
            }

            public bool Intersects(Range other)
            {
                var begin = Math.Max(Begin, other.Begin);
                var end = Math.Min(End, other.End);
                return (begin < end);
            }

            public Range Merge(Range other)
            {
                var begin = Math.Min(Begin, other.Begin);
                var end = Math.Max(End, other.End);

                return new Range(begin, end - begin);
            }

            public Range Subtract(Range other)
            {
                if (!Intersects(other))
                    throw new Exception("No intersection");

                if( other.Begin <= Begin )
                {
                    var begin = Math.Min(End, other.End);
                    return new Range(begin, End - begin);
                }
                else
                {
                    var end = Math.Min(End, other.Begin);
                    return new Range(Begin, end - Begin);
                }
            }
        }

        class RangeList
        {
            public List <Range> Ranges { get; private set; } = new List <Range>();

            public void Add(Range r)
            {
                Ranges.Add(r);
                Consolidate();
            }

            public void Subtract(Range r)
            {
                var result = new List<Range>();

                foreach (var x in Ranges)
                {
                    if (x.Intersects(r))
                    {
                        var xr = x.Subtract(r);
                        if( xr.Length > 0 )
                            result.Add(xr);
                    }
                    else
                    {
                        result.Add(x);
                    }
                }

                Ranges = result;
            }

            private void Consolidate()
            {
                Ranges = Ranges.OrderBy(r => r.Begin).ToList();

                var result = new List<Range>();

                var open = Ranges.First();
                foreach (var r in Ranges.Skip(1))
                {
                    if( r.Intersects(open) || r.Begin == open.End )
                    {
                        open = open.Merge(r);
                    }
                    else
                    {
                        result.Add(open);
                        open = r;
                    }
                }
                result.Add(open);

                Ranges = result;
            }
        }

        protected override object Solve1(string filename)
        {
            long px = 18;
            long py = 289;

            var template = System.IO.File.ReadAllLines("Day18/template.txt").ToList();

            var bounds = new List<Wall>();

            var commands = System.IO.File.ReadAllLines(filename).Select(Command.Parse).ToList();

            for( int i = 0; i < commands.Count; ++i)
            {
                var cmd = commands[i];

                switch (commands[i].Dir)
                {
                    case Direction.Up:
                        py -= cmd.Distance;
                        break;
                    case Direction.Down:
                        py += cmd.Distance;
                        break;
                    case Direction.Right:
                        px += cmd.Distance;
                        bounds.Add(new Wall { Upper = true, Y = py, X0 = px - cmd.Distance, X1 = px });
                        break;
                    case Direction.Left:
                        px -= cmd.Distance;
                        bounds.Add(new Wall { Upper = false, Y = py, X0 = px, X1 = px + cmd.Distance });
                        break;
                }
            }

            bounds = bounds.OrderBy(w => w.Y).OrderBy(w => w.X0).ToList();

            var hwalls = bounds.OrderBy(w => w.Upper ? 0 : 1).OrderBy(w => w.Y).ToList();

            var minX = hwalls.Min(w => w.X0);
            var maxX = hwalls.Max(w => w.X1);
            var minY = hwalls.Min(w => w.Y);
            var maxY = hwalls.Max(w => w.Y);

            long area = 0;

            var open = new RangeList();

            for( var y = minY; y <= maxY; y++ )
            {
                while (hwalls.Any() && hwalls.First().Y == y && hwalls.First().Upper)
                {
                    var w = hwalls.First();
                    var wr = new Range(w.X0, w.X1 - w.X0);
                    open.Add(wr);
                    hwalls = hwalls.Skip(1).ToList();
                }

                long rowArea = 0;
                Console.Write($"Line {y+1}: ");
                foreach (var r in open.Ranges)
                {
                    rowArea += r.Length + 1;
                    Console.Write($"{r.Begin}..{r.End} ");
                }
                Console.WriteLine($"-> {rowArea}");

                area += rowArea;

                if (filename.Contains("input"))
                {
                    var templateRowArea = template[(int)y].Count(c => c != '.');

                    if (rowArea != templateRowArea)
                    {
                        Debugger.Break();
                    }
                }

                while (hwalls.Any() && hwalls.First().Y == y)
                {
                    var w = hwalls.First();
                    var wr = new Range(w.X0, w.X1 - w.X0);
                    open.Subtract(wr);
                    hwalls = hwalls.Skip(1).ToList();
                }
            }

            return area;
        }

        protected override object Solve2(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
