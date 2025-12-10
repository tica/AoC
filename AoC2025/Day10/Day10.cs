using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AoC2025
{
    public class Day10 : AoC.DayBase
    {
        record Joltages(Vector512<short> Values)
        {
            public bool IsNull => Values == Vector512<short>.Zero;

            public bool ContainsNegative => Vector512.LessThan(Values, Vector512<short>.Zero).ExtractMostSignificantBits() != 0;

            private static Vector512<short> ConvertIndices(IEnumerable<int> indices)
            {
                var values = new short[Vector512<short>.Count];

                foreach (var i in indices)
                {
                    values[i] = 1;
                }

                return Vector512.Create(values);
            }

            public static Joltages WithOnesSet(IEnumerable<int> onesSet)
            {
                return new Joltages(ConvertIndices(onesSet));
            }

            public static Joltages WithValues(IEnumerable<short> values)
            {
                var arr = new short[Vector512<short>.Count];
                Array.Copy(values.ToArray(), arr, values.Count());

                return new Joltages(Vector512.Create(arr));
            }

            public override string ToString()
            {
                return $"Joltages( {String.Join(", ", Values)} )";
            }

            public static Joltages operator -(Joltages a, Joltages b)
            {
                return new Joltages(a.Values - b.Values);
            }
        }

        record class Machine(int InitialPattern, List<int> Toggles, Joltages TargetJoltages, List<Joltages> JoltageIncrements)
        {
            public static Machine Parse(string line)
            {
                var arr = line.Split(' ');

                var pattern = arr[0].Substring(1, arr[0].Length - 2)
                    .Select((ch, n) => (ch, n))
                    .Aggregate(0, (acc, t) => acc |= (t.ch == '#' ? (1 << t.n) : 0));

                var toggles = arr.Skip(1).Take(arr.Length - 2)
                    .Select(s => s.Substring(1, s.Length - 2)
                        .Split(',')
                        .Select(int.Parse)
                        .Aggregate(0, (acc, n) => acc | (1 << n)))
                    .ToList();

                var targetJoltages = Joltages.WithValues(arr.Last().Substring(1, arr.Last().Length - 2).Split(',').Select(short.Parse));

                var joltageIncrements = arr.Skip(1).Take(arr.Length - 2)
                    .Select(s => s.Substring(1, s.Length - 2)
                        .Split(',')
                        .Select(int.Parse))
                    .Select(Joltages.WithOnesSet)
                    .ToList();

                return new Machine(pattern, toggles, targetJoltages, joltageIncrements);
            }
        }

        private int CountMinimumTogglesRequired(Machine m)
        {
            HashSet<int> states = new HashSet<int> { 0 };

            int result = 0;

            while( true )
            {
                result += 1;

                HashSet<int> newStates = new();

                foreach (var s in states)
                {
                    foreach (var t in m.Toggles)
                    {
                        var ns = s ^ t;
                        if (ns == m.InitialPattern)
                            return result;

                        newStates.Add(ns);
                    }
                }

                states = newStates;
            }
        }

        protected override object Solve1(string filename)
        {
            var machines = File.ReadAllLines(filename).Select(Machine.Parse).ToList();

            return machines.Sum(CountMinimumTogglesRequired);
        }

        int CountMinimumPressesRequiredSlow(Machine m)
        {
            Dictionary<Joltages, int> dp = new() {
                [m.TargetJoltages] = 0
            };

            HashSet<Joltages> states = new() { m.TargetJoltages };

            while( true )
            {
                var j = states.Order().First();

                states.Remove(j);

                int n = dp[j];

                foreach( var i in m.JoltageIncrements )
                {
                    var jj = j - i;

                    if (jj.IsNull)
                        return n + 1;

                    if (jj.ContainsNegative)
                        continue;

                    if (dp.ContainsKey(jj))
                        continue;

                    dp.Add(jj, n + 1);

                    states.Add(jj);
                }
            }
        }

        private int CountMinimumPressesRequiredFrom(Joltages state, Machine m, Dictionary<Joltages, int> dp, int depth, ref int best)
        {
            int min = int.MaxValue / 2;

            if ( depth >= best )
            {
                return min;
            }

            if( dp.TryGetValue(state, out int steps) )
            {
                return steps;
            }

            foreach (var i in m.JoltageIncrements)
            {
                var next = state - i;

                if (next.IsNull)
                {
                    if( depth < best )
                    {
                        best = depth;
                    }
                    min = 1;
                    break;
                }

                if (next.ContainsNegative)
                    continue;

                min = Math.Min(min, 1 + CountMinimumPressesRequiredFrom(next, m, dp, depth + 1, ref best));
            }

            dp.Add(state, min);

            return min;
        }

        int CountMinimumPressesRequired(Machine m)
        {
            int best = int.MaxValue;
            return CountMinimumPressesRequiredFrom(m.TargetJoltages, m, new(), 0, ref best);
        }

        protected override object Solve2(string filename)
        {
            var machines = File.ReadAllLines(filename).Select(Machine.Parse).ToList();

            return machines.Sum(CountMinimumPressesRequired);
        }

        public override object SolutionExample1 => 7;
        public override object SolutionPuzzle1 => 432;
        public override object SolutionExample2 => 0;
        public override object SolutionPuzzle2 => 0;
    }
}
