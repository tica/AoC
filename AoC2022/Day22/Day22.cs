using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.LoadMultiple(filename).First();
            var instructions = File.ReadAllLines(filename).SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).First();

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
                            else if(next.Value ==  ' ')
                            {
                                // Fallthrough
                            }
                            else
                            {
                                throw new InvalidOperationException();
                            }
                        }

                        next = dir switch
                        {
                            Direction.Right => grid.Row(pos.Y).First(c => c.Value != ' '),
                            Direction.Left => grid.Row(pos.Y).Last(c => c.Value != ' '),
                            Direction.Down => grid.Column(pos.X).First(c => c.Value != ' '),
                            Direction.Up => grid.Column(pos.X).Last(c => c.Value != ' '),
                            _ => throw new InvalidOperationException()
                        };

                        if (next.Value == '.')
                            pos = next;
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

                //grid.Print(c => c == pos ? ConsoleColor.Red : ConsoleColor.Gray);
                //Console.ReadLine();
            }

            int score = 0;
            score += (pos.Y + 1) * 1000;
            score += (pos.X + 1) * 4;
            score += dir switch { Direction.Right => 0, Direction.Down => 1, Direction.Left => 2, Direction.Up => 3, _ => throw new InvalidOperationException() };

            return score;
        }

        protected override object Solve2(string filename)
        {
            return null!;
        }

        public override object SolutionExample1 => null!;
        public override object SolutionPuzzle1 => null!;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;
    }
}
