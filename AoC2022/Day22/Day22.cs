using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;
using System.Diagnostics;

namespace AoC2022
{
    public class Day22 : AoC.DayBase
    {
        IEnumerable<(int Distance, char Turn)> EnumInstructions(string instructions)
        {
            int p = 0;
            int n = 0;

            while (p < instructions.Length)
            {
                switch(instructions[p])
                {
                    case 'L':
                    case 'R':
                        yield return (int.Parse(instructions.Substring(n, p - n)), '\0');
                        yield return (0, instructions[p]);
                        n = p + 1;
                        break;
                    default:
                        break;
                }
                p += 1;                
            }

            yield return (int.Parse(instructions.Substring(n, p - n)), '\0');
        }

        int WalkGrid(Grid grid, string instructions, Func<Coord, Direction, (Coord, Direction)> wrapAround, bool debug = false)
        {
            var dir = Direction.Right;
            var pos = grid.Row(0).First(c => c.Value == '.');

            foreach (var i in EnumInstructions(instructions))
            {
                if (i.Distance > 0)
                {
                    for (int j = 0; j < i.Distance; ++j)
                    {
                        var next = pos.Neighbor(dir);

                        if (next.IsValid)
                        {
                            if (next.Value == '.')
                            {
                                pos = next;
                                continue;
                            }
                            else if (next.Value == '#')
                            {
                                break;
                            }
                            else if (next.Value == ' ')
                            {
                                // Fallthrough
                            }
                            else
                            {
                                throw new InvalidOperationException();
                            }
                        }

                        (pos, dir) = wrapAround(pos, dir);
                    }
                }
                else
                {
                    dir = i.Turn switch
                    {
                        'R' => dir.TurnRight(),
                        'L' => dir.TurnLeft(),
                        _ => throw new InvalidOperationException()
                    };
                }

                if (debug)
                {
                    grid.Print(c => c == pos ? ConsoleColor.Red : ConsoleColor.Gray);
                    Console.ReadLine();
                }
            }

            int score = 0;
            score += (pos.Y + 1) * 1000;
            score += (pos.X + 1) * 4;
            score += dir switch { Direction.Right => 0, Direction.Down => 1, Direction.Left => 2, Direction.Up => 3, _ => throw new InvalidOperationException() };

            return score;
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.LoadMultiple(filename).First();
            var instructions = File.ReadAllLines(filename).SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).First();

            return WalkGrid(grid, instructions,
                (pos, dir) =>
                {
                    var next = dir switch
                    {
                        Direction.Right => grid.Row(pos.Y).First(c => c.Value != ' '),
                        Direction.Left => grid.Row(pos.Y).Last(c => c.Value != ' '),
                        Direction.Down => grid.Column(pos.X).First(c => c.Value != ' '),
                        Direction.Up => grid.Column(pos.X).Last(c => c.Value != ' '),
                        _ => throw new InvalidOperationException()
                    };

                    if (next.Value == '.')
                        pos = next;

                    return (pos, dir);
                }
            );
        }

        private (Coord, Direction) WrapExample(Coord pos, Direction dir)
        {
            var grid = pos.Parent;

            int tileSize = 4;
            int tileRow = pos.Y / (pos.Parent.Height / 3);
            int tileCol = pos.X / (pos.Parent.Width / 4);
            int offsetX = pos.X - tileCol * tileSize;
            int offsetY = pos.Y - tileRow * tileSize;

            Func<Coord, bool> notSpace = c => c.Value != ' ';

            var (next, ndir) = dir switch
            {
                Direction.Left => tileRow switch
                {
                    0 => (grid.Column(tileSize + offsetY).First(notSpace), Direction.Down),
                    1 => (grid.Column(tileSize * 3 + offsetY).Last(notSpace), Direction.Up),
                    2 => (grid.Column(tileSize * 2 - offsetY - 1).Last(notSpace), Direction.Up),
                    _ => throw new InvalidOperationException(),
                },
                Direction.Right => tileRow switch
                {
                    0 => (grid.Row(tileSize * 3 - offsetY - 1).Last(notSpace), Direction.Left),
                    1 => (grid.Column(tileSize * 4 - offsetY - 1).First(notSpace), Direction.Down),
                    2 => (grid.Row(tileSize - offsetY - 1).Last(notSpace), Direction.Left),
                    _ => throw new InvalidOperationException(),
                },
                Direction.Up => tileCol switch
                {
                    0 => (grid.Column(tileSize * 3 - offsetX - 1).First(notSpace), Direction.Down),
                    1 => (grid.Row(offsetX).First(notSpace), Direction.Right),
                    2 => (grid.Column(tileSize - offsetX - 1).First(notSpace), Direction.Down),
                    3 => (grid.Row(tileSize * 2 - offsetX - 1).Last(notSpace), Direction.Left),
                    _ => throw new InvalidOperationException(),
                },
                Direction.Down => tileCol switch
                {
                    0 => (grid.Column(tileSize * 3 - offsetX - 1).Last(notSpace), Direction.Up),
                    1 => (grid.Row(tileSize*3 - offsetX - 1).First(notSpace), Direction.Right),
                    2 => (grid.Column(tileSize - offsetX - 1).Last(notSpace), Direction.Up),
                    3 => (grid.Row(tileSize * 2 - offsetX - 1).First(notSpace), Direction.Left),
                    _ => throw new InvalidOperationException(),
                },
                _ => throw new InvalidOperationException(),
            };

            if (next.Value == '.')
                return (next, ndir);

            return (pos, dir);
        }

        private (Coord, Direction) WrapPuzzle(Coord pos, Direction dir)
        {
            var grid = pos.Parent;

            int tileSize = 50;
            int tileRow = pos.Y / (pos.Parent.Height / 4);
            int tileCol = pos.X / (pos.Parent.Width / 3);
            int offsetX = pos.X - tileCol * tileSize;
            int offsetY = pos.Y - tileRow * tileSize;

            Func<Coord, bool> notSpace = c => c.Value != ' ';

            //   AABB
            //   AABB
            //   CC
            //   CC
            // DDEE
            // DDEE
            // FF
            // FF

            var (next, ndir) = dir switch
            {
                Direction.Left => tileRow switch
                {
                    // Left of A to left of D, invert
                    0 => (grid.Row(tileSize * 3 - offsetY - 1).First(notSpace), Direction.Right),
                    // Left of C to top of D
                    1 => (grid.Column(offsetY).First(notSpace), Direction.Down),
                    // Left of D to left of A, invert
                    2 => (grid.Row(tileSize - offsetY - 1).First(notSpace), Direction.Right),
                    // Left of F to top of A
                    3 => (grid.Column(tileSize + offsetY).First(notSpace), Direction.Down),
                    _ => throw new InvalidOperationException(),
                },
                Direction.Right => tileRow switch
                {
                    // Right of B to right of E, invert
                    0 => (grid.Row(tileSize * 3 - offsetY - 1).Last(notSpace), Direction.Left),
                    // Right of C to bottom of B
                    1 => (grid.Column(tileSize * 2 + offsetY).Last(notSpace), Direction.Up),
                    // Right of E to right of B, invert
                    2 => (grid.Row(tileSize - offsetY - 1).Last(notSpace), Direction.Left),
                    // Right of F to bottom of E
                    3 => (grid.Column(tileSize + offsetY).Last(notSpace), Direction.Up),
                    _ => throw new InvalidOperationException(),
                },
                Direction.Up => tileCol switch
                {
                    // Top of D to left of C
                    0 => (grid.Row(tileSize + offsetX).First(notSpace), Direction.Right),
                    // Top of A to left of F
                    1 => (grid.Row(tileSize * 3 + offsetX).First(notSpace), Direction.Right),
                    // Top of B to bottom of F
                    2 => (grid.Column(offsetX).Last(notSpace), Direction.Up),
                    _ => throw new InvalidOperationException(),
                },
                Direction.Down => tileCol switch
                {
                    // Bottom of F to top of B
                    0 => (grid.Column(tileSize * 2 + offsetX).First(notSpace), Direction.Down),
                    // Bottom of E to right of F
                    1 => (grid.Row(tileSize * 3 + offsetX).Last(notSpace), Direction.Left),
                    // Bottom of B to right of C
                    2 => (grid.Row(tileSize + offsetX).Last(notSpace), Direction.Left),
                    _ => throw new InvalidOperationException(),
                },
                _ => throw new InvalidOperationException(),
            };

            if (next.Value == '.')
                return (next, ndir);

            return (pos, dir);
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.LoadMultiple(filename).First();
            var instructions = File.ReadAllLines(filename).SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).First();

            Func<Coord, Direction, (Coord, Direction)> wrap = filename.Contains("example") ? WrapExample : WrapPuzzle;

            return WalkGrid(grid, instructions, wrap); //, !filename.Contains("example"));
        }

        public override object SolutionExample1 => 6032;
        public override object SolutionPuzzle1 => 95358;
        public override object SolutionExample2 => 5031;
        public override object SolutionPuzzle2 => 144361;
    }
}
