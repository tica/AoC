using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2015
{
    public class Day10 : AoC.DayBase
    {
        IEnumerable<char> Process(IEnumerable<char> input)
        {
            var e = input.GetEnumerator();

            e.MoveNext();

            for( bool done = false; !done; )
            {
                int count = 0;
                char ch = e.Current;

                while (!done && e.Current == ch)
                {
                    count += 1;
                    done = !e.MoveNext();
                }

                yield return (char)('0' + count);
                yield return ch;
            }
        }

        int CalcMutatedLength(IEnumerable<char> text, int mutations)
        {
            for (int i = 0; i < mutations; ++i)
            {
                text = Process(text);
            }

            return text.Count();
        }


        protected override object Solve1(string filename)
        {
            var text = File.ReadAllText(filename);

            return CalcMutatedLength(text, 40);
        }

        protected override object Solve2(string filename)
        {
            var text = File.ReadAllText(filename);

            return CalcMutatedLength(text, 50);
        }

        public override object SolutionExample1 => 237746;
        public override object SolutionPuzzle1 => 360154;
        public override object SolutionExample2 => 3369156;
        public override object SolutionPuzzle2 => 5103798;
    }
}
