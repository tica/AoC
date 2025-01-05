using AoC.Util;

namespace AoC2024
{
    public class Day25 : AoC.DayBase
    {
        List<int> CalcHeights(Grid<char> key)
        {
            return key.Columns.Select(col => col.Count(c => c.Value == '#') - 1).ToList();
        }

        protected override object Solve1(string filename)
        {
            var inputs = GridHelper.LoadMultiple(filename).ToList();

            var locks = inputs.Where(g => g.Row(0).First().Value == '#').Select(CalcHeights).ToList();
            var keys = inputs.Where(g => g.Row(0).First().Value == '.').Select(CalcHeights).ToList();

            int len = locks[0].Count;
            int height = inputs[0].Height;

            int count = 0;

            foreach (var key in keys)
            {
                foreach (var lck in locks)
                {
                    if( key.Zip(lck).All((t) => t.First + t.Second < height-1) )
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }

        protected override object Solve2(string filename)
        {
            return null!;
        }

        public override object SolutionExample1 => 3;
        public override object SolutionPuzzle1 => 2835;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;
    }
}
