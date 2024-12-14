using System.Text.RegularExpressions;

namespace AoC2015
{
    public class Day2 : AoC.DayBase
    {
        record class Present(int Width, int Height, int Depth)
        {
            public static Present Parse(string line)
            {
                var m = Regex.Match(line, @"(\d+)x(\d+)x(\d+)");

                return new Present(
                    int.Parse(m.Groups[1].Value),
                    int.Parse(m.Groups[2].Value),
                    int.Parse(m.Groups[3].Value)
                );
            }
        }

        private int CalcRequiredPackaging(Present p)
        {
            int a = p.Width * p.Height;
            int b = p.Width * p.Depth;
            int c = p.Height * p.Depth;
            int extra = Math.Min(a, Math.Min(b, c));

            return 2 * a + 2 * b + 2 * c + extra;
        }

        private int CalcRequiredRibbon(Present p)
        {
            int longest = Math.Max(p.Width, Math.Max(p.Height, p.Depth));

            int around = p.Width * 2 + p.Height * 2 + p.Depth * 2 - longest * 2;
            int bow = p.Width * p.Height * p.Depth;

            return around + bow;
        }

        protected override object Solve1(string filename)
        {
            var input = File.ReadAllLines(filename).Select(Present.Parse);

            return input.Sum(CalcRequiredPackaging);
        }

        protected override object Solve2(string filename)
        {
            var input = File.ReadAllLines(filename).Select(Present.Parse);

            return input.Sum(CalcRequiredRibbon);
        }

        public override object SolutionExample1 => 58 + 43;
        public override object SolutionPuzzle1 => 1586300;
        public override object SolutionExample2 => 34 + 14;
        public override object SolutionPuzzle2 => 3737498;
    }
}
