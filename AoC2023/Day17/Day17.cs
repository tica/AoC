using AoC.Util;
using Coord = AoC.Util.Grid<int>.Coord;

namespace AoC2023
{
    public class Day17 : AoC.DayBase
    {
        public override object SolutionExample1 => 102;
        public override object SolutionPuzzle1 => 1238;
        public override object SolutionExample2 => 94;
        public override object SolutionPuzzle2 => 1362;

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

            private static IEnumerable<(Node, int)> EnumSteps(Coord p, NodeDirection newMode, Direction dir)
            {
                int cost = 0;
                for (int i = 0; i < MaxMove; ++i)
                {
                    p = p.Neighbor(dir);
                    if (!p.IsValid)
                        yield break;

                    cost += p.Value;
                    if (i >= MinMove - 1)
                        yield return (new Node(p, newMode), cost);
                }
            }

            public IEnumerable<(Node, int)> WeightedNeighbors
            {
                get
                {
                    var result = Enumerable.Empty<(Node, int)>();

                    if (Mode == NodeDirection.Horizontal || Mode == NodeDirection.Any)
                    {
                        result = result.Concat(EnumSteps(Position, NodeDirection.Vertical, Direction.Left));
                        result = result.Concat(EnumSteps(Position, NodeDirection.Vertical, Direction.Right));
                    }
                    if (Mode == NodeDirection.Vertical || Mode == NodeDirection.Any)
                    {
                        result = result.Concat(EnumSteps(Position, NodeDirection.Horizontal, Direction.Up));
                        result = result.Concat(EnumSteps(Position, NodeDirection.Horizontal, Direction.Down));
                    }

                    return result;
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
            var grid = GridHelper.Load(filename).Transform(x => int.Parse(x.ToString()));

            var nodePath = AStar.FindPath(
                Node.Any(grid.TopLeft),
                Node.Any(grid.BottomRight),
                n => n.WeightedNeighbors,
                n => grid.Distance(n.Position, grid.BottomRight)
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
