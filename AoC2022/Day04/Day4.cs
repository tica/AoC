using System;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day4 : AoC.DayBase
    {
        record class Input(int X0, int X1, int Y0, int Y1)
        {
            public int SX => X1 - X0;
            public int SY => Y1 - Y0;

            public static Input Parse(string line)
            {
                var m = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)");

                return new Input(
                    int.Parse(m.Groups[1].Value),
                    int.Parse(m.Groups[2].Value),
                    int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value)
                );
            }
        }

        protected override object Solve1(string filename)
        {
            int num = 0;

            foreach( var line in File.ReadAllLines(filename).Select(Input.Parse))
            {
                if (line.SX > line.SY)
                {
                    if (line.Y0 >= line.X0 && line.Y1 <= line.X1)
                    {
                        num += 1;
                    }
                }
                else
                {
                    if (line.X0 >= line.Y0 && line.X1 <= line.Y1)
                    {
                        num += 1;
                    }
                }
            }

            return num;
        }

        protected override object Solve2(string filename)
        {
            int num = 0;

            foreach (var line in File.ReadAllLines(filename).Select(Input.Parse))
            {
                if (line.X1 < line.Y0)
                {
                }
                else if (line.Y1 < line.X0)
                {
                }
                else
                {
                    num += 1;
                }
            }

            return num;
        }

        public override object SolutionExample1 => 2;
        public override object SolutionPuzzle1 => 542;
        public override object SolutionExample2 => 4;
        public override object SolutionPuzzle2 => 900;
    }
}
