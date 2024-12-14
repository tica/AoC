namespace AoC2015
{
    public class Day8 : AoC.DayBase
    {
        int CalcDecodeDeflation(string line)
        {
            if (!line.StartsWith('\"'))
                throw new ArgumentException();
            if (!line.EndsWith('\"'))
                throw new ArgumentException();

            var contents = line.Substring(1, line.Length - 2);

            int numChars = 0;
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i] == '\\')
                {
                    if (contents[i + 1] == 'x')
                    {
                        numChars += 1;
                        i += 3;
                    }
                    else
                    {
                        numChars += 1;
                        i += 1;
                    }
                }
                else
                {
                    numChars += 1;
                }
            }

            return line.Length - numChars;
        }

        int CalcEncodeInflation(string line)
        {
            return 1 + line.Count(ch => "\\\"".Contains(ch)) + 1;
        }

        protected override object Solve1(string filename)
        {
            return File.ReadAllLines(filename).Sum(CalcDecodeDeflation);
        }

        protected override object Solve2(string filename)
        {
            return File.ReadAllLines(filename).Sum(CalcEncodeInflation);
        }

        public override object SolutionExample1 => 12;
        public override object SolutionPuzzle1 => 1371;
        public override object SolutionExample2 => 19;
        public override object SolutionPuzzle2 => 2117;
    }
}
