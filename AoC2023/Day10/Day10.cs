using AoC2023.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AoC2023
{
    public class Day10 : DayBase
    {
        public Day10(): base(10) { }

        public override object SolutionExample1 => 8;
        public override object SolutionPuzzle1 => 6768;
        public override object SolutionExample2 => 10;
        public override object SolutionPuzzle2 => 351;

        private enum Direction
        {
            Left, Right, Up, Down
        }

        private static (Grid.Coord, Direction) Next(Grid.Coord coord, Direction dir)
        {
            var v = coord.Parent[coord];
            switch (dir)
            {
                case Direction.Up:
                    // Next coord after moving into coord from below
                    switch (v)
                    {
                        case 'F': return (coord.Right, Direction.Right);
                        case '7': return (coord.Left, Direction.Left);
                        case '|': return (coord.Top, Direction.Up);
                        default: throw new Exception("oops");
                    }
                case Direction.Left:
                    // Next coord after moving into coord from the right
                    switch (v)
                    {
                        case 'F': return (coord.Bottom, Direction.Down);
                        case 'L': return (coord.Top, Direction.Up);
                        case '-': return (coord.Left, Direction.Left);
                        default: throw new Exception("oops");
                    }
                case Direction.Right:
                    // Next coord after moving into coord from the left
                    switch (v)
                    {
                        case '7': return (coord.Bottom, Direction.Down);
                        case 'J': return (coord.Top, Direction.Up);
                        case '-': return (coord.Right, Direction.Right);
                        default: throw new Exception("oops");
                    }
                case Direction.Down:
                    // Next coord after moving into coord from top
                    switch (v)
                    {
                        case 'L': return (coord.Right, Direction.Right);
                        case 'J': return (coord.Left, Direction.Left);
                        case '|': return (coord.Bottom, Direction.Down);
                        default: throw new Exception("oops");
                    }
                default:
                    throw new Exception("oops");
            }
        }

        private static List<(Grid.Coord, Direction)> FindLoop(Grid.Coord start, Direction dir)
        {
            var p = start;

            var list = new List<(Grid.Coord, Direction)>() { (start, dir) };
            do
            {
                (p, dir) = Next(p, dir);

                list.Add((p, dir));
            }
            while (p != start);

            return list;
        }

        protected override object Solve1(string filename)
        {
            var grid = new Util.Grid(filename);
            var start = grid.AllCoordinates.Single(c => grid[c] == 'S');

            Direction dir = Direction.Right;
            if (filename.Contains("example"))
            {
                grid.Set(start, 'F');
                dir = Direction.Up;
            }
            else
            {
                grid.Set(start, '7');
            }

            var loop = FindLoop(start, dir);
            return loop.Count / 2;
        }

        private void PrintGrid(Util.Grid grid)
        {
            for(int y = 0; y < grid.Height; ++y)
            {
                for( int x = 0; x < grid.Width; ++x)
                {
                    var c = new Grid.Coord(grid, x, y);
                    if (c.Parent[c] == 'I')
                        Console.ForegroundColor = ConsoleColor.White;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(grid[c]);
                }
                Console.WriteLine();
            }
        }

        private static IEnumerable<Grid.Coord> RightSideNeighbors(Grid.Coord p, Direction dir)
        {
            switch(p.Parent[p])
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

        protected override object Solve2(string filename)
        {
            var grid = new Util.Grid(filename);
            var start = grid.AllCoordinates.Single(c => grid[c] == 'S');

            grid.Set(start, '7');

            var loop = FindLoop(start, Direction.Right);

            var grid2 = new Util.Grid(filename);
            foreach( var p in grid2.AllCoordinates)
            {
                grid2.Set(p, '.');
            }
            foreach( var i in loop)
            {
                var p = i.Item1;
                grid2.Set(p, grid[p]);
            }
            grid = grid2;

            var inner = new List<Grid.Coord>();

            foreach( var i in loop)
            {
                var p = i.Item1;
                var dir = i.Item2;

                foreach ( var n in RightSideNeighbors(p, dir) )
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
                var inside = new List<Grid.Coord>();
                foreach (var i in inner)
                {
                    foreach( var n in i.NeighborCoords)
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

            PrintGrid(grid);

            return inner.Count;
        }
    }
}
