using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AoC2023
{
    public class Day8 : DayBase
    {
        public Day8(): base(8) { }

        public override object SolutionExample1 => 6;
        public override object SolutionPuzzle1 => 22199;
        public override object SolutionExample2 => 6L;
        public override object SolutionPuzzle2 => 13334102464297L;

        record Crossroads(string Id, string LeftId, string RightId)
        {
            public Crossroads? Left { get; set; }
            public Crossroads? Right { get; set; }

            public static Crossroads Parse(string line)
            {
                var m = Regex.Match(line, @"(...) = \((...), (...)\)");

                return new Crossroads(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value);
            }

            private bool? endsWithZ = null;

            public bool IdEndsWithZ => endsWithZ ??= Id.EndsWith("Z");
        }

        private static IEnumerable<char> RepeatChars(string input)
        {
            while( true)
            {
                foreach( var ch in input)
                {
                    yield return ch;
                }
            }
        }

        protected override object Solve1(string filename)
        { 
            var lines = System.IO.File.ReadAllLines(filename);
            var instructions = lines.First();

            var xr = lines.Skip(2).Select(Crossroads.Parse).ToList();

            var xd = xr.ToDictionary(x => x.Id, x => x);

            foreach (var x in xd.Values)
            {
                x.Left = xd[x.LeftId];
                x.Right = xd[x.RightId];
            }

            var p = xd["AAA"];
            int steps = 0;

            foreach( var ch in RepeatChars(instructions))
            {
                if( ch == 'L' )
                {
                    p = p.Left;
                }
                else
                {
                    p = p.Right;
                }

                steps += 1;

                if (p!.Id == "ZZZ")
                    break;
            }

            return steps;
        }

        (int,int) FindStepsToTargetTwice(Crossroads p, string instructions)
        {
            int steps = 0;
            int goal1 = 0;

            foreach( var ch in RepeatChars(instructions))
            {
                if (ch == 'L')
                {
                    p = p.Left!;
                }
                else
                {
                    p = p.Right!;
                }

                steps += 1;

                if( p.IdEndsWithZ )
                {
                    if (goal1 == 0)
                        goal1 = steps;
                    else
                        return (goal1, steps - goal1);
                }
            }

            throw new Exception("oops");
        }

        int MinIndex(int[] arr)
        {
            int m = int.MaxValue;
            int r = -1;

            for( int i = 0; i < arr.Length; ++i)
            {
                if (arr[i] < m)
                {
                    r = i;
                    m = arr[i];
                }
            }

            return r;
        }

        static long LCM(long[] numbers)
        {
            return numbers.Aggregate(lcm);
        }
        static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        protected override object Solve2(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            var instructions = lines.First();

            var xr = lines.Skip(2).Select(Crossroads.Parse).ToList();

            var xd = xr.ToDictionary(x => x.Id, x => x);

            foreach( var x in xd.Values )
            {
                x.Left = xd[x.LeftId];
                x.Right = xd[x.RightId];
            }

            var pp = xr.Where(x => x.Id.EndsWith("A")).ToArray();

            var qq = pp.Select(p => FindStepsToTargetTwice(p, instructions)).ToArray();

            var current = qq.Select(x => x.Item1).ToArray();

            var yy = LCM(current.Select(n => (long)n).ToArray());
            return yy;
        }
    }
}
