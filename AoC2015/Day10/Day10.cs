using System.Diagnostics;

namespace AoC2015
{
    public class Day10 : AoC.DayBase
    {
        IEnumerable<char> ProcessInternal(string s)
        {
            for( int i = 0; i < s.Length; ++i)
            {
                int count = 1;
                char ch = s[i];

                while (i < s.Length - 1 && s[i + 1] == ch)
                {
                    count += 1;
                    i += 1;
                }

                yield return (char)('0' + count);
                yield return ch;
            }
        }

        string Process(string s)
        {
            return new string(ProcessInternal(s).ToArray());
        }

        protected override object Solve1(string filename)
        {
            var text = File.ReadAllText(filename);

            for (int i = 0; i < 40; ++i)
            {
                text = Process(text);
            }

            return text.Length;
        }

        protected override object Solve2(string filename)
        {
            var text = File.ReadAllText(filename);

            for (int i = 0; i < 50; ++i)
            {
                text = Process(text);
            }

            return text.Length;
        }

        public override object SolutionExample1 => null!;
        public override object SolutionPuzzle1 => null!;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;
    }
}
