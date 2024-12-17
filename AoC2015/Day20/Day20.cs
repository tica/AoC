namespace AoC2015
{
    public class Day20 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            int minPresents = int.Parse(File.ReadAllText(filename));

            var numHouses = minPresents / 10;
            var counts = new int[numHouses];

            int elflimit = numHouses;

            for (int elf = 1; elf < elflimit; ++elf)
            {
                for (int i = elf; i < elflimit; i += elf)
                {
                    counts[i] += elf * 10;
                    if (counts[i] >= minPresents)
                    {
                        elflimit = i;
                    }
                }
            }

            return counts.Select((c, i) => (c, i)).First(t => t.c >= minPresents).i;
        }

        protected override object Solve2(string filename)
        {
            int minPresents = int.Parse(File.ReadAllText(filename));

            var numHouses = minPresents / 10;
            var counts = new int[numHouses];

            int elflimit = numHouses;

            for (int elf = 1; elf < elflimit; ++elf)
            {
                for (int i = elf, j = 0; i < elflimit && j < 50; i += elf, ++j)
                {
                    counts[i] += elf * 11;
                    if (counts[i] >= minPresents)
                    {
                        elflimit = i;
                    }
                }
            }

            return counts.Select((c, i) => (c, i)).First(t => t.c >= minPresents).i;
        }

        public override object SolutionExample1 => 8;
        public override object SolutionPuzzle1 => 665280;
        public override object SolutionExample2 => 8;
        public override object SolutionPuzzle2 => 705600;
    }
}