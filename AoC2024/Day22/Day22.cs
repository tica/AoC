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

        string Stringify(IEnumerable<int> deltas)
        {
            return new string(deltas.Select(d => (char)(d + 'M')).ToArray());
        }

        Dictionary<string, int> BuildFirstIndexLookup(List<(string, int)> src)
        {
            var result = new Dictionary<string, int>();
            for (int i = 0; i < src.Count; ++i)
            {
                if( !result.ContainsKey(src[i].Item1))
                {
                    result.Add(src[i].Item1, src[i].Item2);
                }
            }

            return result;
        }

        int EvalPattern(List<Dictionary<string, int>> patternLookup, string pattern)
        {
            int sum = 0;

            foreach( var lookup in patternLookup)
            {
                if (lookup.TryGetValue(pattern, out int val))
                {
                    sum += val;
                }
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            var numbers = File.ReadAllLines(filename).Select(long.Parse).ToList();

            var sequences = numbers.Select(n => EnumerableEx.GenerateConsecutive(2000, n, Process).Select(n => (int)(n % 10)).ToList()).ToList();
            var deltas = sequences.Select(seq => seq.Window2().Select(t =>t.Right - t.Left)).Select(Stringify).ToList();

            var subDeltaResults = sequences.Zip(deltas).Select(t => Enumerable.Range(0, deltas[0].Length - 4).Select(i => (t.Second.Substring(i, 4), t.First[i+4])).ToList()).ToList();
            var patternLookup = subDeltaResults.Select(BuildFirstIndexLookup).ToList();

            var allPatterns = patternLookup.SelectMany(dict => dict.Keys).Distinct();

            return allPatterns.Max(p => EvalPattern(patternLookup, p));
        }

        public override object SolutionExample1 => 37327623L;
        public override object SolutionPuzzle1 => 17612566393L;
        public override object SolutionExample2 => 23;
        public override object SolutionPuzzle2 => 1968;
    }
}
