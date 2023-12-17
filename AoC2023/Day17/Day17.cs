using AoC2023.Util;
using System;
using Grid = AoC2023.Util.Grid<int>;
using Coord = AoC2023.Util.Grid<int>.Coord;

namespace AoC2023
{
    public class Day17 : DayBase
    {
        public Day17(): base(17) { }

        public override object SolutionExample1 => 102;

        public override object SolutionPuzzle1 => 1238;

        public override object SolutionExample2 => 94;

        public override object SolutionPuzzle2 => 1362;

        private static IEnumerable<Direction> AllowedDirections(Direction dir, int stepsInSameDirection)
        {
            switch(dir)
            {
                case Direction.Left:
                case Direction.Right:
                    yield return Direction.Up;
                    yield return Direction.Down;
                    break;
                case Direction.Up:
                case Direction.Down:
                    yield return Direction.Left;
                    yield return Direction.Right;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (stepsInSameDirection < 3)
                yield return dir;
        }

        private (List<Coord>?, int) FindMinimalPath(Grid grid, Coord p, Direction dir, int stepsInSameDirection, List<Coord> path)
        {
            var result = new List<Coord>() { p };
            var thisCost = grid[p];

            if (p.X == grid.Width - 1 && p.Y == grid.Height - 1)
            {
                return (result, thisCost);
            }

            var allowedDirections = AllowedDirections(dir, stepsInSameDirection);

            int minimalCost = int.MaxValue;
            List<Coord>? minimalPath = null;

            foreach (var (next, d, sd) in allowedDirections
                .Where(d => p.Neighbor(d).IsValid)
                .Select(d => (p.Neighbor(d), d, (d != dir) ? 0 : stepsInSameDirection + 1)))
            {
                if (path.Contains(next))
                    continue;

                var (sub, c) = FindMinimalPath(grid, next, d, sd, path.Concat(new[] { next }).ToList());
                if( sub != null && c < minimalCost )
                {
                    minimalCost = c;
                    minimalPath = sub;
                }
            }

            if (minimalPath == null)
                return (null, 0);

            return (result.Concat(minimalPath).ToList(), thisCost + minimalCost);
        }


        enum NodeDirection
        {
            Horizontal,
            Vertical,
            Any
        }

        class Node
        {
            public static int MinMove = 1;
            public static int MaxMove = 3;

            public Coord Position { get; init; }
            public NodeDirection Mode { get; init; }

            public Node(Coord position, NodeDirection mode)
            {
                Position = position;
                Mode = mode;
            }

            public static Node Any(Coord pos) => new Node(pos, NodeDirection.Any);

            public IEnumerable<(Node, int)> WeightedNeighbors
            {
                get
                {
                    if (Mode == NodeDirection.Horizontal || Mode == NodeDirection.Any)
                    {
                        var p = Position;
                        int cost = 0;
                        for (int i = 0; i < MaxMove && !p.IsLeftBorder; ++i)
                        {
                            p = p.Left;
                            cost += p.Value;
                            if (i >= MinMove - 1)
                                yield return (new Node(p, NodeDirection.Vertical), cost);
                        }

                        p = Position;
                        cost = 0;
                        for (int i = 0; i < MaxMove  && !p.IsRightBorder; ++i)
                        {
                            p = p.Right;
                            cost += p.Value;
                            if (i >= MinMove - 1)
                                yield return (new Node(p, NodeDirection.Vertical), cost);
                        }
                    }
                    if( Mode == NodeDirection.Vertical || Mode == NodeDirection.Any)
                    {
                        var p = Position;
                        int cost = 0;
                        for (int i = 0; i < MaxMove  && !p.IsTopBorder; ++i)
                        {
                            p = p.Top;
                            cost += p.Value;
                            if (i >= MinMove - 1)
                                yield return (new Node(p, NodeDirection.Horizontal), cost);
                        }

                        p = Position;
                        cost = 0;
                        for (int i = 0; i < MaxMove && !p.IsBottomBorder; ++i)
                        {
                            p = p.Bottom;
                            cost += p.Value;
                            if (i >= MinMove - 1)
                                yield return (new Node(p, NodeDirection.Horizontal), cost);
                        }
                    }
                }
            }

            public override bool Equals(object? obj)
            {
                if (obj == null) return false;
                if (!(obj is Node)) return false;

                Node other = (Node)obj;

                if( ReferenceEquals(this, obj) )
                    return true;

                if ( Position != other.Position ) return false;

                if (Mode == NodeDirection.Any || other.Mode == NodeDirection.Any)
                    return true;

                return Mode == other.Mode;
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode() + Mode.GetHashCode();
            }

            public override string ToString()
            {
                return $"{Position} ({Mode})";
            }
        }

        private int FindPath(string filename)
        {
            var grid = Util.GridHelper.Load(filename).Transform(x => int.Parse(x.ToString()));

            var nodePath = Util.AStar.FindPath(
                Node.Any(grid.Pos(0, 0)),
                Node.Any(grid.Pos(grid.Width - 1, grid.Height - 1)),
                n => n.WeightedNeighbors,
                n => (grid.Width - 1 - n.Position.X) + (grid.Height - 1 - n.Position.Y)
            );

            var path = new List<Coord>() { nodePath[0].Position };
            for (int i = 0; i < nodePath.Count - 1; ++i)
            {
                path.AddRange(grid.Between(nodePath[i].Position, nodePath[i + 1].Position));
                path.Add(nodePath[i + 1].Position);
            }

            //grid.Print(p => path.Contains(p) ? ConsoleColor.White : ConsoleColor.DarkGray);

            return path.Sum(p => p.Value) - grid.Pos(0, 0).Value;
        }

        protected override object Solve1(string filename)
        {
            Node.MinMove = 1;
            Node.MaxMove = 3;
            return FindPath(filename);
        }

        protected override object Solve2(string filename)
        {
            Node.MinMove = 4;
            Node.MaxMove = 10;
            return FindPath(filename);
        }
    }
}
