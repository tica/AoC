using System.Text;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day10 : AoC.DayBase
    {
        int? ParseOp(string line)
        {
            var m = Regex.Match(line, @"addx (-?\d+)");
            if (m.Success)
            {
                return int.Parse(m.Groups[1].Value);
            }
            else
            {
                return null;
            }
        }

        int CalcSignal(int cycle, int x, StringBuilder output)
        {
            int column = (cycle - 1) % 40;
            if (x >= (column - 1) && x <= (column + 1))
            {
                output.Append("#");
            }
            else
            {
                output.Append(".");
            }
            if (cycle % 40 == 0)
            {
                output.Append("\r\n");
            }

            if ((cycle - 20) % 40 == 0)
            {
                return cycle * x;
            }
            else
            {
                return 0;
            }
        }

        private (int Signal, string Output) AnalyzeSignal(string filename)
        {
            int cycle = 1;
            int X = 1;
            int signal = 0;

            var output = new StringBuilder();

            foreach (var op in File.ReadLines(filename).Select(ParseOp))
            {
                if (op == null)
                {
                    signal += CalcSignal(cycle++, X, output);
                }
                else
                {
                    signal += CalcSignal(cycle++, X, output);
                    signal += CalcSignal(cycle++, X, output);
                    X += op.Value;
                }
            }

            return (signal, output.ToString());
        }


        protected override object Solve1(string filename)
        {
            return AnalyzeSignal(filename).Signal;
        }

        protected override object Solve2(string filename)
        {
            return AnalyzeSignal(filename).Output;
        }

        public override object SolutionExample1 => 13140;
        public override object SolutionPuzzle1 => 12520;
        public override object SolutionExample2 =>
            "##..##..##..##..##..##..##..##..##..##..\r\n" +
            "###...###...###...###...###...###...###.\r\n" +
            "####....####....####....####....####....\r\n" +
            "#####.....#####.....#####.....#####.....\r\n" +
            "######......######......######......####\r\n" +
            "#######.......#######.......#######.....\r\n";
        public override object SolutionPuzzle2 =>
            "####.#..#.###..####.###....##..##..#....\r\n" +
            "#....#..#.#..#....#.#..#....#.#..#.#....\r\n" +
            "###..####.#..#...#..#..#....#.#....#....\r\n" +
            "#....#..#.###...#...###.....#.#.##.#....\r\n" +
            "#....#..#.#....#....#....#..#.#..#.#....\r\n" +
            "####.#..#.#....####.#.....##...###.####.\r\n";
    }
}
