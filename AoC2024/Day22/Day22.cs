using AoC.Util;
using System.Collections.Concurrent;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day22 : AoC.DayBase
    {
        void MixIntoSecret(ref long secret, long value)
        {
            secret ^= value;
        }

        void PruneSecret(ref long secret)
        {
            secret &= 0xFF_FFFF;
        }

        long Process(long secret)
        {
            MixIntoSecret(ref secret, secret * 64);
            PruneSecret(ref secret);

            MixIntoSecret(ref secret, secret / 32);
            PruneSecret(ref secret);

            MixIntoSecret(ref secret, secret * 2048);
            PruneSecret(ref secret);

            return secret;
       }

        protected override object Solve1(string filename)
        {
            var numbers = File.ReadAllLines(filename).Select(long.Parse).ToList();

            return numbers.Select(n => Enumerable.Repeat(0, 2000).Aggregate(n, (x, _) => Process(x))).Sum();
        }

        int IndexOfListInList<T>(List<T> haystack, List<T> needle) where T : IEquatable<T>
        {
            for( int i = 0; i < haystack.Count - needle.Count; i++ )
            {
                if (!haystack[i].Equals(needle[0]))
                    continue;

                bool match = true;
                for( int j = 1; j < needle.Count; ++j)
                {
                    if( !haystack[i+j].Equals(needle[j]))
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                    return i;
            }

            return -1;
        }

        int IndexOfListInList(string haystack, string needle)
        {
            for (int i = 0; i < haystack.Length - needle.Length; i++)
            {
                if (!haystack[i].Equals(needle[0]))
                    continue;

                bool match = true;
                for (int j = 1; j < needle.Length; ++j)
                {
                    if (!haystack[i + j].Equals(needle[j]))
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                    return i;
            }

            return -1;
        }


        IEnumerable<List<int>> GeneratePatterns()
        {
            for (int a = -9; a <= 9; ++a)
            {
                // -9 => [0..1]
                // -8 => [0..2]
                // -7 => [0..3]
                // -1 => [0..8]
                // 0 => [0..9]
                // 1 => [1..9]
                // 2 => [2..9]
                // 8 => [8..9]
                // 9 => [9..9]
                int minSeqA = Math.Max(0, a);
                int maxSeqA = Math.Min(9, a + 9);

                int minB = -maxSeqA;
                int maxB = 9 - minSeqA;

                for (int b = minB; b <= maxB; ++b)
                {
                    int minSeqB = minSeqA + b;
                    int maxSeqB = maxSeqA + b;

                    int minC = -maxSeqB;
                    int maxC = 9 - minSeqB;

                    for (int c = minC; c <= maxC; ++c)
                    {
                        int minSeqC = minSeqB + c;
                        int maxSeqC = maxSeqB + c;

                        int minD = -maxSeqC;
                        int maxD = 9 - minSeqC;

                        for (int d = minD; d <= maxD; ++d)
                        {
                            yield return [a, b, c, d];
                        }
                    }
                }
            }
        }

        string Stringify(IEnumerable<int> deltas)
        {
            return new string(deltas.Select(d => (char)(d + 'M')).ToArray());
        }

        int EvalPattern(List<List<int>> sequences, List<List<int>> deltas, List<int> pattern)
        {
            int sum = 0;

            for (int i = 0; i < sequences.Count; ++i)
            {
                int index = IndexOfListInList(deltas[i], pattern);
                if (index < 0)
                    continue;

                sum += sequences[i][index + pattern.Count];
            }

            return sum;
        }

        int EvalPattern(List<List<int>> sequences, List<string> deltas, string pattern)
        {
            int sum = 0;

            for (int i = 0; i < sequences.Count; ++i)
            {
                int index = IndexOfListInList(deltas[i], pattern);
                if (index < 0)
                    continue;

                sum += sequences[i][index + pattern.Length];
            }

            return sum;
        }


        protected override object Solve2(string filename)
        {
            var numbers = File.ReadAllLines(filename).Select(long.Parse).ToList();

            var sequences = numbers.Select(n => EnumerableEx.GenerateConsecutive(2000, n, Process).Select(n => (int)(n % 10)).ToList()).ToList();
            var deltas = sequences.Select(seq => seq.Window2().Select(t =>t.Right - t.Left)).Select(Stringify).ToList();

            var allPatterns = GeneratePatterns().Select(Stringify).ToList();
            //var allPatterns = new List<List<int>> { new List<int> { -2, 1, -1, 3 } }.Select(Stringify).ToList();

            int max = 0;
            for( int i = 0; i < allPatterns.Count; ++i)
            {
                int m = EvalPattern(sequences, deltas, allPatterns[i]);
                max = Math.Max(max, m);

                //Console.WriteLine(i);
            }

            return max;
        }

        public override object SolutionExample1 => 37327623L;
        public override object SolutionPuzzle1 => 17612566393L;
        public override object SolutionExample2 => 23;
        public override object SolutionPuzzle2 => 1968;
    }
}
