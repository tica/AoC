namespace AoC2015
{
    public class Day17 : AoC.DayBase
    {
        int CountCombinations(List<int> containers, int p, int current, int total)
        {
            if (current == total)
                return 1;

            if (p >= containers.Count)
                return 0;

            int count = CountCombinations(containers, p + 1, current, total);

            if (total - current >= containers[p])
            {
                count += CountCombinations(containers, p + 1, current + containers[p], total);
            }

            return count;
        }

        protected override object Solve1(string filename)
        {
            var containers = File.ReadAllLines(filename).Select(int.Parse).ToList();

            int total = filename.Contains("example") ? 25 : 150;

            return CountCombinations(containers, 0, 0, total);
        }

        int FindMinimumContainerCount(List<int> containers, int p, int num, int current, int total)
        {
            if (current == total)
                return num;

            if (p >= containers.Count)
                return int.MaxValue;

            int min = FindMinimumContainerCount(containers, p + 1, num, current, total);

            if (total - current >= containers[p])
            {
                int m = FindMinimumContainerCount(containers, p + 1, num + 1, current + containers[p], total);
                min = Math.Min(min, m);
            }

            return min;
        }

        int CountCombinations(List<int> containers, int p, int num, int current, int total, int max)
        {
            if (num > max)
                return 0;

            if (current == total)
                return 1;

            if (p >= containers.Count)
                return 0;

            int count = CountCombinations(containers, p + 1, num, current, total, max);

            if (total - current >= containers[p])
            {
                count += CountCombinations(containers, p + 1, num + 1, current + containers[p], total, max);
            }

            return count;
        }

        protected override object Solve2(string filename)
        {
            var containers = File.ReadAllLines(filename).Select(int.Parse).ToList();

            int total = filename.Contains("example") ? 25 : 150;

            int min = FindMinimumContainerCount(containers, 0, 0, 0, total);

            return CountCombinations(containers, 0, 0, 0, total, min);
        }

        public override object SolutionExample1 => 4;
        public override object SolutionPuzzle1 => 1304;
        public override object SolutionExample2 => 3;
        public override object SolutionPuzzle2 => 18;
    }
}
