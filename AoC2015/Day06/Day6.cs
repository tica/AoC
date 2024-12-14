using System.Text.RegularExpressions;
using Grid = AoC.Util.Grid<int>;

namespace AoC2015
{
    public class Day6 : AoC.DayBase
    {
        enum Action
        {
            On,
            Off,
            Toggle
        }

        record class Instruction(Action Action, int FromX, int FromY, int ToX, int ToY)
        {
            public static Instruction Parse(string line)
            {
                var m = Regex.Match(line, @"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)");
                var action = m.Groups[1].Value == "turn on" ? Action.On : m.Groups[1].Value == "turn off" ? Action.Off : Action.Toggle;

                return new Instruction(
                    action,
                    int.Parse(m.Groups[2].Value),
                    int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value),
                    int.Parse(m.Groups[5].Value)
                );
            }

            public void ModifyGrid(Grid grid)
            {
                for (int y = FromY; y <= ToY; ++y)
                {
                    for (int x = FromX; x <= ToX; ++x)
                    {
                        switch( Action)
                        {
                            case Action.On: grid[y, x] = 1; break;
                            case Action.Off: grid[y, x] = 0; break;
                            case Action.Toggle: grid[y, x] = 1 - grid[y,x]; break;
                            default: throw new NotSupportedException();
                        }
                    }
                }
            }

            public void ModifyGrid2(Grid grid)
            {
                for (int y = FromY; y <= ToY; ++y)
                {
                    for (int x = FromX; x <= ToX; ++x)
                    {
                        switch( Action)
                        {
                            case Action.On: grid[y, x] += 1; break;
                            case Action.Off: grid[y, x] = Math.Max(0, grid[y,x]-1); break;
                            case Action.Toggle: grid[y, x] += 2; break;
                            default: throw new NotSupportedException();
                        }
                    }
                }
            }
        }

        protected override object Solve1(string filename)
        {
            var input = File.ReadAllLines(filename).Select(Instruction.Parse);
            var grid = new Grid(1000, 1000, 0);

            foreach (var i in input)
            {
                i.ModifyGrid(grid);
            }

            return grid.AllValues.Sum();
        }

        protected override object Solve2(string filename)
        {
            var input = File.ReadAllLines(filename).Select(Instruction.Parse);
            var grid = new Grid(1000, 1000, 0);

            foreach (var i in input)
            {
                i.ModifyGrid2(grid);
            }

            return grid.AllValues.Sum();
        }

        public override object SolutionExample1 => 998996;
        public override object SolutionPuzzle1 => 569999;
        public override object SolutionExample2 => 1001996;
        public override object SolutionPuzzle2 => 17836115;
    }
}
