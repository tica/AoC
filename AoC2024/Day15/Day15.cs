using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2024
{
    public class Day15 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var grid = AoC.Util.GridHelper.LoadMultiple(filename).First();
            var instructions = string.Join("", File.ReadAllLines(filename).SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1));
            var robot = grid.WhereValue('@').Single();

            foreach ( var dir in instructions.Select(DirectionHelper.Parse))
            {
                var n = robot.Neighbor(dir);
                while (n.Value == 'O')
                    n = n.Neighbor(dir);

                if( n.Value == '.')
                {
                    grid.Set(robot, '.');
                    robot = robot.Neighbor(dir);
                    grid.Set(n, robot.Value);
                    grid.Set(robot, '@');
                }
            }

            return grid.WhereValue('O').Sum(c => c.X + c.Y * 100);
        }

        protected override object Solve2(string filename)
        {
            var originalGrid = AoC.Util.GridHelper.LoadMultiple(filename).First();
            var grid = new Grid(originalGrid.Width * 2, originalGrid.Height, '.');
            foreach (var wall in originalGrid.WhereValue('#'))
            {
                grid.Set(new Coord(grid, wall.X * 2, wall.Y), '#');
                grid.Set(new Coord(grid, wall.X * 2 + 1, wall.Y), '#');
            }
            foreach (var box in originalGrid.WhereValue('O'))
            {
                grid.Set(new Coord(grid, box.X * 2, box.Y), '[');
                grid.Set(new Coord(grid, box.X * 2 + 1, box.Y), ']');
            }
            foreach (var r in originalGrid.WhereValue('@'))
            {
                grid.Set(new Coord(grid, r.X * 2, r.Y), '@');
                grid.Set(new Coord(grid, r.X * 2 + 1, r.Y), '.');
            }

            var instructions = string.Join("", File.ReadAllLines(filename).SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1));
            var robot = grid.WhereValue('@').Single();

            foreach (var dir in instructions.Select(DirectionHelper.Parse))
            {
                if( dir == Direction.Left || dir == Direction.Right )
                {
                    var n = robot.Neighbor(dir);
                    while (n.Value != '.' && n.Value != '#')
                        n = n.Neighbor(dir);

                    if (n.Value == '.')
                    {
                        while (n != robot)
                        {
                            var prev = n.Neighbor(dir.Reverse());
                            grid.Set(n, prev.Value);
                            n = prev;
                        }

                        grid.Set(robot, '.');
                        robot = robot.Neighbor(dir);
                    }
                }
                else
                {
                    bool blocked = false;

                    var moving = new List<Coord> { robot };
                    var current = new HashSet<Coord> { robot };

                    while (!blocked && current.Any())
                    {
                        var next = new HashSet<Coord>();

                        foreach (var c in current)
                        {
                            if (c.NeighborValue(dir) == '#')
                            {
                                blocked = true;
                                break;
                            }
                            else if (c.NeighborValue(dir) == '[')
                            {
                                next.Add(c.Neighbor(dir));
                                next.Add(c.Neighbor(dir).Neighbor(Direction.Right));
                            }
                            else if (c.NeighborValue(dir) == ']')
                            {
                                next.Add(c.Neighbor(dir));
                                next.Add(c.Neighbor(dir).Neighbor(Direction.Left));
                            }
                        }

                        moving.AddRange(next);
                        current = next;
                    }

                    if( !blocked )
                    {
                        foreach( var c in Enumerable.Reverse(moving) )
                        {
                            grid.Set(c.Neighbor(dir), c.Value);
                            grid.Set(c, '.');
                        }
                        robot = robot.Neighbor(dir);
                    }
                }
            }

            return grid.WhereValue('[').Sum(c => c.X + c.Y * 100);
        }

        public override object SolutionExample1 => 10092;
        public override object SolutionPuzzle1 => 1456590;
        public override object SolutionExample2 => 9021;
        public override object SolutionPuzzle2 => 1489116;
    }
}
