using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AoC2025
{
    public class Day10 : AoC.DayBase
    {
        record Joltages : IEquatable<Joltages>, IComparable<Joltages>
        {
            private List<int> Values { get; init; }
            public int Count { get; init; }

            public bool ContainsNegative { get; init; }
            public bool IsNull { get; init; }

            public Joltages(List<int> values, bool isNull = false, bool containsNegative = false)
            {
                Values = values;
                Count = values.Count;
                IsNull = isNull;
                ContainsNegative = containsNegative;
            }
            
            public override string ToString()
            {
                return $"Joltages( {String.Join(", ", Values)} )";
            }

            public static Joltages operator -(Joltages a, Joltages b)
            {
                var result = new List<int>(a.Count);

                bool allZero = true;
                for (int i = 0; i < a.Count; ++i)
                {
                    int x = a.Values[i] - b.Values[i];

                    if( x < 0 )
                    {
                        return new Joltages(result, false, true);
                    }
                    else if( x > 0 )
                    {
                        allZero = false;
                    }

                    result.Add(a.Values[i] - b.Values[i]);
                }

                return new Joltages(result, allZero, false);
            }

            public static bool operator<=(Joltages a, Joltages b)
            {
                for( int i = 0; i < a.Count ; ++i )
                {
                    if (a.Values[i] > b.Values[i])
                        return false;
                }

                return true;
            }

            public static bool operator >=(Joltages a, Joltages b)
            {
                for (int i = 0; i < a.Count; ++i)
                {
                    if (a.Values[i] < b.Values[i])
                        return false;
                }

                return true;
            }

            public virtual bool Equals(Joltages? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;

                return Values.SequenceEqual(other.Values);
            }

            public override int GetHashCode()
            {
                var hashCode = new HashCode();
                foreach (var val in Values)
                {
                    hashCode.Add(val);
                }
                return hashCode.ToHashCode();
            }

            public int CompareTo(Joltages? other)
            {
                if (ReferenceEquals(null, other)) return 1;
                if (ReferenceEquals(this, other)) return 0;

                for (int i = 0; i < Count; ++i)
                {
                    int d = Values[i] - other.Values[i];
                    if (d != 0)
                        return d;
                }

                return 0;
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

                var targetJoltages = new Joltages(arr.Last().Substring(1, arr.Last().Length - 2).Split(',').Select(int.Parse).ToList());

                var joltageIncrements = arr.Skip(1).Take(arr.Length - 2)
                    .Select(s => s.Substring(1, s.Length - 2)
                        .Split(',')
                        .Select(int.Parse)
                        .Aggregate(Enumerable.Repeat(0, targetJoltages.Count).ToList(), (acc, n) => { acc[n] = 1; return acc; }))
                    .Select(nn => new Joltages(nn))
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

        private int CountMinimumPressesRequiredFrom(Joltages state, Machine m, Dictionary<Joltages, int> dp)
        {
            if( dp.TryGetValue(state, out int steps) )
            {
                return steps;
            }

            int min = int.MaxValue / 2;

            var vv = new System.Numerics.Vector<byte>();

            foreach (var i in m.JoltageIncrements)
            {
                var next = state - i;

                if (next.IsNull)
                    return 1;

                if (next.ContainsNegative)
                    continue;

                min = Math.Min(min, 1 + CountMinimumPressesRequiredFrom(next, m, dp));
            }

            dp.Add(state, min);

            return min;
        }

        int CountMinimumPressesRequired(Machine m)
        {
            return CountMinimumPressesRequiredFrom(m.TargetJoltages, m, new());
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
