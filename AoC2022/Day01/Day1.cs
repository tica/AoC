namespace AoC2022
{
    public class Day1 : AoC.DayBase
    {
        private static List<int> BuildSums(string filename)
        {
            List<int> sums = new List<int>();
            int sum = 0;

            foreach (var line in File.ReadAllLines(filename))
            {
                if (string.IsNullOrEmpty(line))
                {
                    sums.Add(sum);
                    sum = 0;
                    continue;
                }

                sum += int.Parse(line);
            }

            return sums;
        }

        protected override object Solve1(string filename)
        {
            return BuildSums(filename).Max();
        }

        protected override object Solve2(string filename)
        {
            return BuildSums(filename).OrderByDescending(x => x).Take(3).Sum();
        }

        public override object SolutionExample1 => 24000;
        public override object SolutionPuzzle1 => 71502;
        public override object SolutionExample2 => 41000;
        public override object SolutionPuzzle2 => 208191;
    }
}
