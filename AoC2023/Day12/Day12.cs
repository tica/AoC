using AoC2023.Util;
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
    public class Day12 : DayBase
    {
        public Day12(): base(12) { }

        public override object SolutionExample1 => throw new NotImplementedException();
        public override object SolutionPuzzle1 => throw new NotImplementedException();
        public override object SolutionExample2 => throw new NotImplementedException();
        public override object SolutionPuzzle2 => throw new NotImplementedException();

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

        enum ValidationResult
        {
            Match,
            Ok,
            Error
        }

        struct ValidationState
        {
            public ValidationResult Result;
            public byte Rule;
            public byte Pos;
            public byte Damaged;

            public static readonly ValidationState Start = new ValidationState { Rule = 0, Pos = 0, Damaged = 0 };
            public static readonly ValidationState Error = new ValidationState { Result = ValidationResult.Error };
            public static readonly ValidationState CompleteMatch = new ValidationState { Result = ValidationResult.Match };
        }

        private static ValidationState ValidateStep(List<Symbol> pattern, List<int> rules, List<int> finalRuleStart, ValidationState state)
        {            
            switch (pattern[state.Pos])
            {
                case Symbol.Operational:
                    if (state.Damaged > 0)
                    {
                        if (state.Rule == rules.Count)
                            return ValidationState.Error;

                        if (rules[state.Rule] == state.Damaged)
                        {
                            if (state.Pos == (pattern.Count - 1) && state.Rule == (rules.Count - 1))
                                return ValidationState.CompleteMatch;

                            if (state.Rule + 1 < finalRuleStart.Count && state.Pos + 1 > finalRuleStart[state.Rule + 1])
                                return ValidationState.Error;

                            return new ValidationState
                            {
                                Result = ValidationResult.Ok,
                                Rule = (byte)(state.Rule + 1),
                                Pos = (byte)(state.Pos + 1)
                            };
                        }
                        else
                        {
                            return ValidationState.Error;
                        }
                    }
                    else
                    {
                        if( state.Pos == (pattern.Count - 1) && state.Rule == rules.Count )
                            return ValidationState.CompleteMatch;

                        return new ValidationState
                        {
                            Result = ValidationResult.Ok,
                            Rule = state.Rule,
                            Pos = (byte)(state.Pos + 1)
                        };
                    }
                case Symbol.Damaged:
                    if (state.Rule == rules.Count)
                        return ValidationState.Error;
                    if ((state.Damaged + 1) > rules[state.Rule])
                        return ValidationState.Error;

                    if (state.Pos == pattern.Count - 1)
                    {
                        if (state.Rule == rules.Count - 1 && state.Damaged + 1 == rules[state.Rule])
                            return ValidationState.CompleteMatch;
                        else
                            return ValidationState.Error;
                    }

                    return new ValidationState
                    {
                        Result = ValidationResult.Ok,
                        Rule = state.Rule,
                        Pos = (byte)(state.Pos + 1),
                        Damaged = (byte)(state.Damaged + 1)
                    };
                default:
                    throw new Exception("oops");
            }
        }

        private static bool ValidatePattern(List<Symbol> pattern, List<int> rules)
        {
            int currentDamaged = 0;
            int currentRule = 0;

            foreach(Symbol symbol in pattern)
            {
                switch(symbol)
                {
                    case Symbol.Unknown: throw new Exception("oops");
                    case Symbol.Operational:
                        if ( currentDamaged > 0)
                        {
                            if (currentRule == rules.Count)
                                return false;

                            if (rules[currentRule] == currentDamaged)
                            {
                                currentRule += 1;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        currentDamaged = 0;
                        break;
                    case Symbol.Damaged:
                        currentDamaged += 1;
                        if (currentRule == rules.Count)
                            return false;
                        if (currentDamaged > rules[currentRule])
                            return false;
                        break;
                }    
            }

            if (currentRule < rules.Count)
            {
                if (currentDamaged > 0)
                {
                    if (rules[currentRule] == currentDamaged)
                    {
                        currentRule += 1;
                    }
                }

                return currentRule == rules.Count;
            }
            else
            {
                return currentDamaged == 0;
            }
        }

        private void PrintPattern(List<Symbol> pattern)
        {
            foreach( var s in pattern)
            {
                switch(s)
                {
                    case Symbol.Unknown:
                        Console.Write('?');
                        break;
                    case Symbol.Damaged:
                        Console.Write('#');
                        break;
                    case Symbol.Operational:
                        Console.Write('.');
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine();
        }

        private long MutatePattern(List<Symbol> pattern, List<int> rules, List<int> finalRuleStart, ValidationState state)
        {
            long result = 0;

            if (pattern[state.Pos] != Symbol.Unknown)
            {
                var nextState = ValidateStep(pattern, rules, finalRuleStart, state);
                if (nextState.Result == ValidationResult.Match)
                {
#if DEBUG
                    if (!ValidatePattern(pattern, rules))
                    {
                        System.Diagnostics.Debugger.Break();
                    }
#endif

                    result += 1;
                }
                else if (nextState.Result == ValidationResult.Ok)
                {
                    if (nextState.Pos < pattern.Count)
                    {
                        result += MutatePattern(pattern, rules, finalRuleStart, nextState);
                    }
                }
            }
            else
            {
                pattern[state.Pos] = Symbol.Operational;
                var nextState = ValidateStep(pattern, rules, finalRuleStart, state);
                if (nextState.Result == ValidationResult.Match)
                {
#if DEBUG
                    if (!ValidatePattern(pattern, rules))
                    {
                        System.Diagnostics.Debugger.Break();
                    }
#endif

                    result += 1;
                }
                else if (nextState.Result == ValidationResult.Ok)
                {
                    if (nextState.Pos < pattern.Count)
                        result += MutatePattern(pattern, rules, finalRuleStart, nextState);
                }

                pattern[state.Pos] = Symbol.Damaged;
                nextState = ValidateStep(pattern, rules, finalRuleStart, state);
                if (nextState.Result == ValidationResult.Match)
                {
#if DEBUG
                    if (!ValidatePattern(pattern, rules))
                    {
                        System.Diagnostics.Debugger.Break();
                    }
#endif

                    result += 1;
                }
                else if (nextState.Result == ValidationResult.Ok)
                {
                    if (nextState.Pos < pattern.Count)
                        result += MutatePattern(pattern, rules, finalRuleStart, nextState);
                }

                pattern[state.Pos] = Symbol.Unknown;
            }

            return result;
        }

        private static List<int> CalcLastRuleStarts(List<int> rules, int patternLength)
        {
            return Enumerable.Range(0, rules.Count)
                .Select(x => patternLength - (rules.Skip(x).Sum() + (rules.Count - x - 1)))
                .ToList();
        }

        struct Pattern
        {
            public UInt128 DamagedMask;
            public UInt128 OperationalMask;
            public UInt128 CoveredMask;
        }

        struct Rule
        {
            public int Length;
            public int FinalStart;
            public UInt128 Mask;
            public UInt128 InverseMaskShift1;
            public UInt128 InverseMask0;
        }

        private IEnumerable<Rule> BuildRules(List<int> rules, int patternLength)
        {
            for( int i = 0; i < rules.Count; ++i)
            {
                UInt128 mask = (1ul << (rules[i])) - 1;
                UInt128 inverseMaskShift1 = (mask | (mask << 2)) & ~(mask << 1);
                UInt128 inverseMask0 = (mask << 1) & ~mask;
                yield return new Rule
                {
                    Length = rules[i],
                    FinalStart = patternLength - (rules.Skip(i).Sum() + (rules.Count - i - 1)),
                    Mask = mask,
                    InverseMaskShift1 = inverseMaskShift1,
                    InverseMask0 = inverseMask0,
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

        private long MutateRules(Pattern pattern, List<Rule> rules, int ruleIndex, int startPosition, Dictionary<(UInt128,int,int), long> cache)
        {
            /*
            if( cache.TryGetValue((pattern.CoveredMask, ruleIndex, startPosition), out long cached))
            {
                return cached;
            }
            */

            var r = rules[ruleIndex];

            long sum = 0;

            for (int i = startPosition; i <= r.FinalStart; ++i)
            {
                /*
                if (ruleIndex <= 4)
                    Console.WriteLine($"{new string(' ', ruleIndex)}Rule {ruleIndex} -> {i}");
                */

                if (((r.Mask << i) & pattern.OperationalMask) != UInt128.Zero)
                    continue;

                if (i == 0)
                {
                    if ((r.InverseMask0 & (pattern.DamagedMask /*| pattern.CoveredMask*/)) != UInt128.Zero)
                        continue;
                }
                else
                {
                    if (((r.InverseMaskShift1 << (i - 1)) & (pattern.DamagedMask /*| pattern.CoveredMask*/)) != UInt128.Zero)
                        continue;
                }

                long localSum = 0;

                var p = new Pattern
                {
                    DamagedMask = pattern.DamagedMask,
                    OperationalMask = pattern.OperationalMask,
                    CoveredMask = pattern.CoveredMask | (r.Mask << i)
                };

                if (ruleIndex == rules.Count - 1)
                {
                    if ((p.DamagedMask & ~p.CoveredMask) == UInt128.Zero)
                    {
                        localSum += 1;
                    }
                }
                else
                {
                    localSum += MutateRules(p, rules, ruleIndex + 1, i + r.Length + 1, cache);
                }

                sum += localSum;
            }

            //cache.Add((pattern.CoveredMask, ruleIndex, startPosition), sum);

            return sum;
        }

        private long MutatePatternBits(List<Symbol> pattern, List<int> rules)
        {
            var pp = BuildPattern(pattern);
            var rr = BuildRules(rules, pattern.Count).ToList();

            return MutateRules(pp, rr, 0, 0, new());
        }

        private (long, long) CountMutations(string patternString, string rulesString)
        {
            long n = 0;
            var pattern = patternString.Select(ToSymbol).ToList();
            var rules = rulesString.Split(',').Select(int.Parse).ToList();

            var sw = System.Diagnostics.Stopwatch.StartNew();


            if (true)
            {
                /*
                var sw3 = System.Diagnostics.Stopwatch.StartNew();
                var rs = CalcLastRuleStarts(rules, pattern.Count);
                var n2 = MutatePattern(pattern, rules, rs, ValidationState.Start);
                sw3.Stop();

                var sw2 = System.Diagnostics.Stopwatch.StartNew();
                */
                n = MutatePatternBits(pattern, rules);
#if DEBUG
                var rs = CalcLastRuleStarts(rules, pattern.Count);
                var n2 = MutatePattern(pattern, rules, rs, ValidationState.Start);
                if( n != n2)
                {
                    System.Diagnostics.Debugger.Break();
                }
#endif
                /*
                sw2.Stop();

                Console.WriteLine($"n = {n}, {sw2.ElapsedMilliseconds} ms");
                Console.WriteLine($"n2 = {n}, {sw3.ElapsedMilliseconds} ms");
                */
            }
            else
            {
                var rs = CalcLastRuleStarts(rules, pattern.Count);
                n = MutatePattern(pattern, rules, rs, ValidationState.Start);
            }

            sw.Stop();

            Console.WriteLine($"Test: {patternString} {rulesString} -> {n} ({sw.ElapsedMilliseconds} ms)");
            return (n, sw.ElapsedMilliseconds);
        }

        protected override object Solve1(string filename)
        {
            //var test = CountMutations("?????.#??.??????.#??.??????.#??.??????.#??.??????.#??.", "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1");

            long sum = 0;

            foreach ( var line in System.IO.File.ReadAllLines(filename))
            {
                var m = Regex.Match(line, @"([\?\.#]+)\s([\d\,]+)");

                var (n, ms) = CountMutations(m.Groups[1].Value, m.Groups[2].Value);
                sum += n;
            }

            return sum;
        }

        private static (string, long) ParseCacheLine(string line)
        {
            var m = Regex.Match(line, @"[^\s]+ ([^\s]+\s[^\s]+) -> (\d+)");
            return (m.Groups[1].Value, long.Parse(m.Groups[2].Value));
        }

        protected override object Solve2(string filename)
        {
            long sum = 0;
            int count = 0;

            var cache = System.IO.File.ReadAllLines("day12/cache.txt")
                .Where(l => l.Length > 0)
                .Select(ParseCacheLine)
                .DistinctBy(p=> p.Item1)
                .ToDictionary(p => p.Item1, p => p.Item2);

            var lines = System.IO.File.ReadAllLines(filename).ToList();

            var mtx = new object();

            var remaining = new List<string>();

            foreach( var line in lines)
            {
                var m = Regex.Match(line, @"([\?\.#]+)\s([\d\,]+)");

                var patternString = string.Join('?', m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value);
                var rulesString = string.Join(',', m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value);

                var cacheLookup = $"{patternString} {rulesString}";
                if (cache.TryGetValue(cacheLookup, out long n))
                {
                    sum += n;
                    count += 1;

                    Console.WriteLine($"({count}/{lines.Count}) {patternString} {rulesString} -> {n} (CACHED)");
                }
                else
                {
                    remaining.Add(line);
                }
            }

            Console.WriteLine("Remaining lines:");
            foreach ( var line in remaining)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
            Console.WriteLine();

            //Parallel.ForEach(remaining,
            //(line) =>
            foreach ( var line in remaining)
                {
                    var m = Regex.Match(line, @"([\?\.#]+)\s([\d\,]+)");

                    var patternString = string.Join('?', m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value, m.Groups[1].Value);
                    var rulesString = string.Join(',', m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value, m.Groups[2].Value);

                    var (n, ms) = CountMutations(patternString, rulesString);

                    Interlocked.Add(ref sum, n);
                    Interlocked.Increment(ref count);

                    lock (mtx)
                    {
                        Console.WriteLine($"({count}/{lines.Count}) {patternString} {rulesString} -> {n} ({ms} ms)");
                        Console.Out.Flush();
                    }
                }
            //);

            return sum;
        }
    }
}
