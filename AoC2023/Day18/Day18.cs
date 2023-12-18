using AoC2023.Util;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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

        private static char FindCorner(Direction prev, Direction next)
        {
            switch(prev)
            {
                case Direction.Up:
                    switch(next)
                    {
                        case Direction.Up: return '|';
                        case Direction.Left: return '7';
                        case Direction.Right: return 'F';
                        default: throw new Exception("oops");
                    }
                case Direction.Down:
                    switch (next)
                    {
                        case Direction.Down: return '|';
                        case Direction.Left: return 'J';
                        case Direction.Right: return 'L';
                        default: throw new Exception("oops");
                    }
                case Direction.Right:
                    switch (next)
                    {
                        case Direction.Right: return '-';
                        case Direction.Down: return '7';
                        case Direction.Up: return 'J';
                        default: throw new Exception("oops");
                    }
                case Direction.Left:
                    switch (next)
                    {
                        case Direction.Left: return '-';
                        case Direction.Down: return 'F';
                        case Direction.Up: return 'L';
                        default: throw new Exception("oops");
                    }
                default: throw new Exception("oops");
            }
        }
        private static char FindWall(Direction dir)
        {
            switch(dir)
            {
                case Direction.Left:
                case Direction.Right:
                    return '-';
                case Direction.Up:
                case Direction.Down:
                    return '|';
                default: throw new Exception("oops");
            }
        }

        private static IEnumerable<Grid<char>.Coord> RightSideNeighbors(Grid.Coord p, Direction dir)
        {
            switch (p.Parent[p])
            {
                case '|':
                    switch (dir)
                    {
                        case Direction.Down:
                            yield return p.Left;
                            yield break;
                        case Direction.Up:
                            yield return p.Right;
                            yield break;
                        default:
                            throw new Exception("oops");
                    }
                case '-':
                    switch (dir)
                    {
                        case Direction.Left:
                            yield return p.Top;
                            yield break;
                        case Direction.Right:
                            yield return p.Bottom;
                            yield break;
                        default:
                            throw new Exception("oops");
                    }
                case '7':
                    switch (dir)
                    {
                        case Direction.Up:
                            yield return p.Right;
                            yield return p.TopRight;
                            yield return p.Top;
                            yield break;
                        case Direction.Right:
                            yield break;
                        default:
                            throw new Exception("oops");
                    }
                case 'L':
                    switch (dir)
                    {
                        case Direction.Down:
                            yield return p.Left;
                            yield return p.BottomLeft;
                            yield return p.Bottom;
                            yield break;
                        case Direction.Left:
                            yield break;
                        default:
                            throw new Exception("oops");
                    }
                case 'J':
                    switch (dir)
                    {
                        case Direction.Down:
                            yield break;
                        case Direction.Right:
                            yield return p.Bottom;
                            yield return p.BottomRight;
                            yield return p.Right;
                            yield break;
                        default:
                            throw new Exception("oops");
                    }
                case 'F':
                    switch (dir)
                    {
                        case Direction.Up:
                            yield break;
                        case Direction.Left:
                            yield return p.Top;
                            yield return p.TopLeft;
                            yield return p.Left;
                            yield break;
                        default:
                            throw new Exception("oops");
                    }
                default:
                    throw new Exception($"oops: {p.Parent[p]}");
            }
        }

        protected override object Solve1(string filename)
        {
            var grid = new Grid(265, 341, '.');

            var pos = grid.Pos(17, 289);

            Direction prevDir = Direction.Right;

            var path = new List<(Grid.Coord, Direction)>();

            foreach ( var line in System.IO.File.ReadAllLines(filename) )
            {
                var m = Regex.Match(line, @"([RLDU]) (\d+) \(#([a-f0-9]+)\)");
                var dir = ToDirection(m.Groups[1].Value);
                var dist = int.Parse(m.Groups[2].Value);

                var c = FindCorner(prevDir, dir);
                prevDir = dir;
                grid.Set(pos, c);

                for ( int i = 0; i < dist; i++ )
                {
                    pos = pos.Neighbor(dir);

                    var w = FindWall(dir);
                    grid.Set(pos, w);

                    path.Add((pos, dir));
                }
            }

            var inner = new List<Grid<char>.Coord>();

            foreach (var (p,d) in path)
            {

                foreach (var n in RightSideNeighbors(p, d))
                {
                    if (grid[n] == '.')
                    {
                        grid.Set(n, 'I');
                        inner.Add(n);
                    }
                }
            }

            while (true)
            {
                var inside = new List<Grid<char>.Coord>();
                foreach (var i in inner)
                {
                    foreach (var n in i.NeighborCoords)
                    {
                        if (grid[n] == '.')
                        {
                            grid.Set(n, 'I');
                            inside.Add(n);
                        }
                    }
                }

                if (!inside.Any())
                    break;

                inner.AddRange(inside);
            }

            //grid.Print();

            return inner.Count + path.Count;
        }

        protected override object Solve2(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
