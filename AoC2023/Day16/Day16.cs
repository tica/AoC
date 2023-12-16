using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using Direction = AoC2023.Util.Direction;
using Grid = AoC2023.Util.Grid<char>;
using Coord = AoC2023.Util.Grid<char>.Coord;
using System.Net;

namespace AoC2023
{
    public class Day16 : DayBase
    {
        public Day16(): base(16) { }

        public override object SolutionExample1 => 46;
        public override object SolutionPuzzle1 => 7307;
        public override object SolutionExample2 => 51;
        public override object SolutionPuzzle2 => 7635;

        IEnumerable<Direction> BounceRay(Coord p, Direction dir)
        {
            switch (p.Parent[p])
            {
                case '\\':
                    switch(dir)
                    {
                        case Direction.Right:
                            yield return Direction.Down;
                            yield break;
                        case Direction.Left:
                            yield return Direction.Up;
                            yield break;
                        case Direction.Up:
                            yield return Direction.Left;
                            yield break;
                        case Direction.Down:
                            yield return Direction.Right;
                            yield break;
                    }
                    throw new Exception("oops");
                case '/':
                    switch (dir)
                    {
                        case Direction.Right:
                            yield return Direction.Up;
                            yield break;
                        case Direction.Left:
                            yield return Direction.Down;
                            yield break;
                        case Direction.Up:
                            yield return Direction.Right;
                            yield break;
                        case Direction.Down:
                            yield return Direction.Left;
                            yield break;
                    }
                    throw new Exception("oops");
                case '-':
                    switch (dir)
                    {
                        case Direction.Right:
                        case Direction.Left:
                            yield return dir;
                            yield break;
                        case Direction.Up:
                        case Direction.Down:
                            yield return Direction.Right;
                            yield return Direction.Left;
                            yield break;
                    }
                    throw new Exception("oops");
                case '|':
                    switch (dir)
                    {
                        case Direction.Right:
                        case Direction.Left:
                            yield return Direction.Up;
                            yield return Direction.Down;
                            yield break;
                        case Direction.Up:
                        case Direction.Down:
                            yield return dir;
                            yield break;
                    }
                    throw new Exception("oops");
                case '.':
                    yield return dir;
                    yield break;
                default:
                    throw new Exception("oops");
            }
        }

        private int CountEnergized(Grid grid, Coord start, Direction dir)
        {
            var energized = new bool[grid.Width, grid.Height];

            var rays = new List<(Coord, Direction)>()
            {
                (start, dir)
            };

            energized[start.X, start.Y] = true;
            int numEnergized = 1;

            var newRays = new List<(Coord, Direction)>();

            var visited = new Dictionary<(Coord, Direction), bool>();

            while (rays.Count > 0)
            {
                foreach (var ray in rays)
                {
                    var p = ray.Item1;

                    if (visited.ContainsKey((p, ray.Item2)))
                        continue;

                    visited.Add((p, ray.Item2), true);

                    if (!energized[p.X, p.Y])
                    {
                        energized[p.X, p.Y] = true;
                        numEnergized += 1;
                    }

                    foreach (var newDir in BounceRay(ray.Item1, ray.Item2))
                    {
                        var newPos = ray.Item1.Neighbor(newDir);
                        if (newPos.IsValid)
                        {
                            newRays.Add((newPos, newDir));
                        }
                    }
                }

                rays = newRays;
                newRays = new();
            }

            return numEnergized;
        }

        protected override object Solve1(string filename)
        {
            var grid = Util.GridHelper.Load(filename);

            return CountEnergized(grid, grid.Pos(0, 0), Direction.Right);
        }

        protected override object Solve2(string filename)
        {
            var grid = Util.GridHelper.Load(filename);

            return new int[] {
                grid.FirstRow.Max(p => CountEnergized(grid, p, Direction.Down)),
                grid.LastRow.Max(p => CountEnergized(grid, p, Direction.Up)),
                grid.FirstColumn.Max(p => CountEnergized(grid, p, Direction.Right)),
                grid.LastColumn.Max(p => CountEnergized(grid, p, Direction.Left)),
            }.Max();
        }
    }
}
