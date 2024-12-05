namespace AoC2022
{
    public class Day3 : AoC.DayBase
    {
        private static int CalcPriority(char ch)
        {
            if (char.IsAsciiLetterLower(ch))
                return ch - 'a' + 1;
            else if( char.IsAsciiLetterUpper(ch))
                return ch - 'A' + 27;

            throw new NotImplementedException();
        }

        protected override object Solve1(string filename)
        {
            int sum = 0;

            foreach (var line in File.ReadAllLines(filename))
            {
                var left = line.Substring(0, line.Length / 2);
                var right = line.Substring(line.Length / 2);

                var dup = left.First(x => right.Contains(x));

                sum += CalcPriority(dup);
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            int sum = 0;

            foreach (var lines in File.ReadAllLines(filename).Chunk(3))
            {
                var tag = lines[0].First(c => lines[1].Contains(c) && lines[2].Contains(c));

                sum += CalcPriority(tag);
            }

            return sum;
        }

        public override object SolutionExample1 => 157;
        public override object SolutionPuzzle1 => 7903;
        public override object SolutionExample2 => 70;
        public override object SolutionPuzzle2 => 2548;
    }
}
