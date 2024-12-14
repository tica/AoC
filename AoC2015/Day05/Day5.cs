using AoC.Util;

namespace AoC2015
{
    public class Day5 : AoC.DayBase
    {
        private bool IsNice(string s)
        {
            if (s.Count(ch => "aeiou".Contains(ch)) < 3)
                return false;

            if (!s.Window2().Any(t => t.Item1 == t.Item2))
                return false;

            if (s.Contains("ab") || s.Contains("cd") || s.Contains("pq") || s.Contains("xy"))
                return false;

            return true;
        }

        protected override object Solve1(string filename)
        {
            var input = File.ReadAllLines(filename);

            return input.Count(IsNice);
        }

        private bool IsReallyNice(string s)
        {
            bool foundPair = false;

            for( int i = 0; i < s.Length-1; ++i)
            {
                var ss = s.Substring(i, 2);

                if (s.LastIndexOf(ss) > i + 1)
                {
                    foundPair = true;
                    break;
                }
            }

            if (!foundPair)
                return false;            

            for( int i = 0; i < s.Length-2; ++i)
            {
                if (s[i] == s[i + 2])
                    return true;
            } 

            return false;
        }

        protected override object Solve2(string filename)
        {
            var input = File.ReadAllLines(filename);

            return input.Count(IsReallyNice);
        }

        public override object SolutionExample1 => 2;
        public override object SolutionPuzzle1 => 258;
        public override object SolutionExample2 => 2;
        public override object SolutionPuzzle2 => 53;
    }
}
