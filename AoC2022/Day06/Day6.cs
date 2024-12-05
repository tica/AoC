namespace AoC2022
{
    public class Day6 : AoC.DayBase
    {
        private static int Solve(string line, int markerSize)
        {
            return Enumerable.Range(0, line.Length).First(index => line.Substring(index, markerSize).Distinct().Count() == markerSize) + markerSize;
        }

        protected override object Solve1(string filename)
        {
            var input = File.ReadAllText(filename);
            return Solve(input, 4);
        }

        protected override object Solve2(string filename)
        {
            var input = File.ReadAllText(filename);
            return Solve(input, 14);
        }

        public override object SolutionExample1 => 7;
        public override object SolutionPuzzle1 => 1531;
        public override object SolutionExample2 => 19;
        public override object SolutionPuzzle2 => 2518;
    }
}
