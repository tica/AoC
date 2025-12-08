using AoC.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day8 : AoC.DayBase
    {
        private (int, int, int) ParseLine(string line)
        {
            var a = line.Split(',').Select(int.Parse).ToArray();
            return (a[0], a[1], a[2]);
        }

        private double CalcDistance((int x, int y, int z) a, (int x, int y, int z) b)
        {
            return checked(Math.Sqrt(MathFunc.Sqr(a.x - b.x) + MathFunc.Sqr(a.y - b.y) + MathFunc.Sqr(a.z - b.z)));
        }

        class Counter<T> : IEnumerable<KeyValuePair<T, long>> where T : notnull
        {
            private readonly Dictionary<T, long> counts = new();

            public void Add(T val, long count)
            {
                if (counts.ContainsKey(val))
                {
                    counts[val] += count;
                }
                else
                {
                    counts[val] = count;
                }
            }

            public IEnumerator<KeyValuePair<T, long>> GetEnumerator()
            {
                return ((IEnumerable<KeyValuePair<T, long>>)counts).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)counts).GetEnumerator();
            }
        }

        protected override object Solve1(string filename)
        {
            int NUM_STEPS = filename.Contains("example") ? 10 : 1000;
            var all = File.ReadAllLines(filename).Select(ParseLine).ToList();

            var lookupGroup = Enumerable.Range(0, all.Count).ToDictionary(n => all[n], n => n);

            var distances = all.Pairwise().Select(p => (p.Item1, p.Item2, CalcDistance(p.Item1, p.Item2))).ToList();

            distances.Sort((a,b) => a.Item3 == b.Item3 ? 0 : (a.Item3 > b.Item3 ? 1 : -1));

            for (int i = 0; i < NUM_STEPS; ++i)
            {
                var a = distances[i].Item1;
                var b = distances[i].Item2;

                var ga = lookupGroup[a];
                var gb = lookupGroup[b];

                if (ga == gb)
                    continue;

                foreach( var x in all )
                {
                    if (lookupGroup[x] == gb)
                        lookupGroup[x] = ga;
                }
            }

            var counter = new Counter<int>();

            foreach( var x in all )
            {
                counter.Add(lookupGroup[x], 1);
            }

            var sizes = counter.Select(kvp => kvp.Value).OrderByDescending(x => x).ToList();

            return sizes[0] * sizes[1] * sizes[2];
        }

        protected override object Solve2(string filename)
        {
            var all = File.ReadAllLines(filename).Select(ParseLine).ToList();

            var lookupGroup = Enumerable.Range(0, all.Count).ToDictionary(n => all[n], n => n);

            var distances = all.Pairwise().Select(p => (p.Item1, p.Item2, CalcDistance(p.Item1, p.Item2))).ToList();

            distances.Sort((a, b) => a.Item3 == b.Item3 ? 0 : (a.Item3 > b.Item3 ? 1 : -1));

            int groupsLeft = all.Count;

            foreach( var i in distances )
            {
                var a = i.Item1;
                var b = i.Item2;

                var ga = lookupGroup[a];
                var gb = lookupGroup[b];

                if (ga == gb)
                    continue;

                foreach (var x in all)
                {
                    if (lookupGroup[x] == gb)
                        lookupGroup[x] = ga;
                }

                groupsLeft -= 1;

                if ( groupsLeft == 1 )
                {
                    return a.Item1 * b.Item1;
                }
            }

            throw new InvalidOperationException("oops?!");
        }

        public override object SolutionExample1 => 40L;
        public override object SolutionPuzzle1 => 131150L;
        public override object SolutionExample2 => 25272;
        public override object SolutionPuzzle2 => 2497445;
    }
}
