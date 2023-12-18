using AoC2023.Util;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Grid = AoC2023.Util.Grid<char>;
using Range = AoC2023.Util.Range;

namespace AoC2023
{
    public class Day18 : DayBase
    {
        public Day18(): base(18) { }

        public override object SolutionExample1 => 62L;
        public override object SolutionPuzzle1 => 42317L;
        public override object SolutionExample2 => 952408144115L;
        public override object SolutionPuzzle2 => 83605563360288L;

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

        record struct Command(Direction Dir, int Distance)
        {
            public static Command Parse(string line)
            {
                var m = Regex.Match(line, @"([RLDU]) (\d+) \(#([a-f0-9]+)\)");
                var dir = ToDirection(m.Groups[1].Value);
                var dist = int.Parse(m.Groups[2].Value);
                return new Command { Dir = dir, Distance = dist };
            }

            public static Command Parse2(string line)
            {
                var m = Regex.Match(line, @"([RLDU]) (\d+) \(#([a-f0-9]+)\)");

                var dist = int.Parse(m.Groups[3].Value.Substring(0, 5), System.Globalization.NumberStyles.HexNumber);

                var dc = (m.Groups[3].Value[5]);
                Direction dir = (dc == '0') ? Direction.Right : ((dc == '1') ? Direction.Down : ((dc == '2') ? Direction.Left : Direction.Up));
                
                return new Command { Dir = dir, Distance = dist };
            }
        }

        private long Solve(List<Command> commands)
        {
            long px = 0;
            long py = 0;

            var bounds = new List<Wall>();

            foreach(var cmd in commands)
            {
                switch (cmd.Dir)
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

            IEnumerable<Wall> hwalls = bounds.OrderBy(w => w.Upper ? 0 : 1).OrderBy(w => w.Y).ToList();

            var minY = hwalls.Min(w => w.Y);
            var maxY = hwalls.Max(w => w.Y);

            long area = 0;

            var open = new RangeList();

            var enumerator = hwalls.GetEnumerator();
            enumerator.MoveNext();

            for (var y = minY; y <= maxY; y++)
            {
                while (enumerator.Current != null && enumerator.Current.Y == y && enumerator.Current.Upper)
                {
                    var w = enumerator.Current;
                    open.Add(new Range(w.X0, w.X1 - w.X0));
                    enumerator.MoveNext();
                }

                foreach (var r in open.Ranges)
                {
                    area += r.Length + 1;
                }

                while (enumerator.Current != null && enumerator.Current.Y == y)
                {
                    var w = enumerator.Current;
                    open.Subtract(new Range(w.X0, w.X1 - w.X0));
                    enumerator.MoveNext();
                }
            }

            return area;
        }

        protected override object Solve1(string filename)
        {
            var commands = System.IO.File.ReadAllLines(filename).Select(Command.Parse).ToList();

            return Solve(commands);
        }

        protected override object Solve2(string filename)
        {
            var commands = System.IO.File.ReadAllLines(filename).Select(Command.Parse2).ToList();

            return Solve(commands);
        }
    }
}
