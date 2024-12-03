using AoC.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AoC2023
{
    public class Day12 : AoC.DayBase
    {
        public override object SolutionExample1 => 21L;
        public override object SolutionPuzzle1 => 7599L;
        public override object SolutionExample2 => 525152L;
        public override object SolutionPuzzle2 => 15454556629917L;

        enum Symbol
        {
            Operational,
            Damaged,
            Unknown
        }

        private static Symbol ToSymbol(char ch)
        {
            switch(ch)
            {
                case '.': return Symbol.Operational;
                case '#': return Symbol.Damaged;
                case '?': return Symbol.Unknown;
                default:
                    throw new Exception("oops");
            }
        }

        struct Pattern
        {
            public UInt128 DamagedMask;
            public UInt128 OperationalMask;
        }

        struct Rule
        {
            public int Length;
            public int FinalStart;
            public UInt128 Mask;
        }

        private IEnumerable<Rule> BuildRules(List<int> rules, int patternLength)
        {
            for( int i = 0; i < rules.Count; ++i)
            {
                yield return new Rule
                {
                    Length = rules[i],
                    FinalStart = patternLength - (rules.Skip(i).Sum() + (rules.Count - i - 1)),
                    Mask = (1ul << (rules[i])) - 1,
                };
            }
        }

        private Pattern BuildPattern(List<Symbol> pattern)
        {
            UInt128 damagedMask = 0;
            UInt128 operationalMask = 0;

            for (int i = 0; i < pattern.Count; ++i)
            {
                switch (pattern[i])
                {
                    case Symbol.Operational:
                        operationalMask |= (((UInt128)1) << i);
                        break;
                    case Symbol.Damaged:
                        damagedMask |= (((UInt128)1) << i);
                        break;
                    default:
                        break;
                }
            }

            return new Pattern
            {
                DamagedMask = damagedMask,
                OperationalMask = operationalMask
            };
        }

        private long MutateRules(Pattern pattern, List<Rule> rules, int ruleIndex, int startPosition, UInt128 covered, Dictionary<(int,int,UInt128), long> cache)
        {
            if( cache.TryGetValue((ruleIndex, startPosition, covered), out long cached))
            {
                return cached;
            }

            var r = rules[ruleIndex];

            long sum = 0;

            for (int i = startPosition; i <= r.FinalStart; ++i)
            {
                if (((r.Mask << i) & pattern.OperationalMask) != UInt128.Zero)
                    continue;

                var left = (((UInt128)1) << i) - 1;
                var uncovered = pattern.DamagedMask & ~covered;
                if ((left & uncovered) != UInt128.Zero)
                    continue;

                var newCovered = covered | ((r.Mask << i) & pattern.DamagedMask);

                if (ruleIndex == rules.Count - 1)
                {
                    if ((pattern.DamagedMask & ~newCovered) == UInt128.Zero)
                    {
                        sum += 1;
                    }
                }
                else
                {
                    sum += MutateRules(pattern, rules, ruleIndex + 1, i + r.Length + 1, newCovered, cache);
                }
            }

            cache.Add((ruleIndex, startPosition, covered), sum);

            return sum;
        }

        private long MutatePatternBits(List<Symbol> pattern, List<int> rules)
        {
            var pp = BuildPattern(pattern);
            var rr = BuildRules(rules, pattern.Count).ToList();

            return MutateRules(pp, rr, 0, 0, UInt128.Zero, new());
        }

        private long CountMutations(string patternString, string rulesString)
        {
            var pattern = patternString.Select(ToSymbol).ToList();
            var rules = rulesString.Split(',').Select(int.Parse).ToList();

            return MutatePatternBits(pattern, rules);
        }

        protected override object Solve1(string filename)
        {
            long sum = 0;

            foreach ( var line in System.IO.File.ReadAllLines(filename))
            {
                var m = Regex.Match(line, @"([\?\.#]+)\s([\d\,]+)");

                sum += CountMutations(m.Groups[1].Value, m.Groups[2].Value);
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename).ToList();

            long sum = 0;

            foreach ( var line in lines)
            {
                var m = Regex.Match(line, @"([\?\.#]+)\s([\d\,]+)");

                var patternString = string.Join('?', m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value);
                var rulesString = string.Join(',', m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value);

                sum += CountMutations(patternString, rulesString);
            }

            return sum;
        }
    }
}
