namespace AoC2015
{
    public class Day1 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var input = File.ReadAllText(filename);

            return input.Select(ch => ch == ')' ? -1 : 1).Sum();
        }

        protected override object Solve2(string filename)
        {
            var input = File.ReadAllText(filename);

            int pos = 0;
            for (int i = 0; i < input.Length; ++i)
            {
                pos += (input[i] == ')') ? -1 : 1;
                if (pos < 0)
                    return i + 1;
            }

            return 0;
        }

        public override object SolutionExample1 => 3;
        public override object SolutionPuzzle1 => 138;
        public override object SolutionExample2 => 1;
        public override object SolutionPuzzle2 => 1771;
    }
}
