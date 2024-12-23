using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;

namespace AoC2024
{
    public class Day21 : AoC.DayBase
    {
        IEnumerable<string> EnumKeySequences(Coord from, Coord to)
        {
            var dirs = new List<Direction>();

            int dx = to.X - from.X;
            if (dx > 0)
                dirs.AddRange(Enumerable.Repeat(Direction.Right, dx));
            else if( dx < 0 )
                dirs.AddRange(Enumerable.Repeat(Direction.Left, -dx));
            int dy = to.Y - from.Y;
            if (dy > 0)
                dirs.AddRange(Enumerable.Repeat(Direction.Down, dy));
            else if (dy < 0)
                dirs.AddRange(Enumerable.Repeat(Direction.Up, -dy));

            foreach ( var perm in dirs.Permute())
            {
                var p = from;
                bool valid = true;
                foreach (var d in perm)
                {
                    p = p.Neighbor(d);
                    valid &= (p.Value != ' ');
                }

                if( valid )
                {
                    var seq = new string(perm.Select(d => d.ToChar()).ToArray());
                    yield return seq + 'A';
                }
            }
        }

        IEnumerable<string> BuildKeySequences(Grid pad, string output)
        {
            var pos = pad.SingleValue('A');

            return BuildKeySequences(pad, pos, "", output);
        }

        IEnumerable<string> BuildKeySequences(Grid pad, Coord pos, string seq, string output)
        {
            if (output.Length == 0)
            {
                yield return seq;
                yield break;            
            }

            var next = pad.SingleValue(output.First());

            var variants = EnumKeySequences(pos, next).Distinct().ToList();
            foreach (var variant in variants)
            {
                foreach (var ks in BuildKeySequences(pad, next, seq + variant, output.Substring(1)))
                {
                    yield return ks;
                }
            }
        }

        enum DirSeqPart
        {
            A,

            LA,
            vA,
            RA,
            UA,

            vLLA,
            RRUA,

            RUA,
            URA,
            RUA_URA,

            LUA,
            ULA,
            LUA_ULA,

            RvA,
            vRA,
            RvA_vRA,

            vLA,
            LvA,
            vLA_LvA,
        }

        long SeqLength(DirSeqPart part, int depth, Dictionary<(DirSeqPart, int), long> c)
        {
            if (depth < 0)
                return 1;

            if( c.TryGetValue((part, depth), out var cached) )
            {
                return cached;
            }

            int d = depth - 1;

            long result = part switch
            {
                DirSeqPart.A => SeqLength(DirSeqPart.A, d, c),
                DirSeqPart.LA => SeqLength(DirSeqPart.vLLA, d, c) + SeqLength(DirSeqPart.RRUA, d, c),
                DirSeqPart.RA => SeqLength(DirSeqPart.vA, d, c) + SeqLength(DirSeqPart.UA, d, c),
                DirSeqPart.vA => SeqLength(DirSeqPart.vLA_LvA, d, c) + SeqLength(DirSeqPart.RUA_URA, d, c),
                DirSeqPart.UA => SeqLength(DirSeqPart.LA, d, c) + SeqLength(DirSeqPart.RA, d, c),
                DirSeqPart.vLLA => SeqLength(DirSeqPart.vLA_LvA, d, c) + SeqLength(DirSeqPart.LA, d, c) + SeqLength(DirSeqPart.A, d, c) + SeqLength(DirSeqPart.RRUA, d, c),
                DirSeqPart.RRUA => SeqLength(DirSeqPart.vA, d, c) + SeqLength(DirSeqPart.A, d, c) + SeqLength(DirSeqPart.LUA_ULA, d, c) + SeqLength(DirSeqPart.RA, d, c),
                DirSeqPart.LvA => SeqLength(DirSeqPart.vLLA, d, c) + SeqLength(DirSeqPart.RA, d, c) + SeqLength(DirSeqPart.RUA_URA, d, c),
                DirSeqPart.vLA => SeqLength(DirSeqPart.vLA_LvA, d, c) + SeqLength(DirSeqPart.LA, d, c) + SeqLength(DirSeqPart.RRUA, d, c),
                DirSeqPart.RUA => SeqLength(DirSeqPart.vA, d, c) + SeqLength(DirSeqPart.LUA_ULA, d, c) + SeqLength(DirSeqPart.RA, d, c),
                DirSeqPart.URA => SeqLength(DirSeqPart.LA, d, c) + SeqLength(DirSeqPart.RvA_vRA, d, c) + SeqLength(DirSeqPart.UA, d, c),
                DirSeqPart.LUA => SeqLength(DirSeqPart.vLLA, d, c) + SeqLength(DirSeqPart.RUA, d, c) + SeqLength(DirSeqPart.RA, d, c),
                DirSeqPart.ULA => SeqLength(DirSeqPart.LA, d, c) + SeqLength(DirSeqPart.vLA, d, c) + SeqLength(DirSeqPart.RRUA, d, c),
                DirSeqPart.RvA => SeqLength(DirSeqPart.vA, d, c) + SeqLength(DirSeqPart.LA, d, c) + SeqLength(DirSeqPart.RUA_URA, d, c),
                DirSeqPart.vRA => SeqLength(DirSeqPart.vLA_LvA, d, c) + SeqLength(DirSeqPart.RA, d, c) + SeqLength(DirSeqPart.UA, d, c),
                DirSeqPart.vLA_LvA => Math.Min(SeqLength(DirSeqPart.vLA, depth, c), SeqLength(DirSeqPart.LvA, depth, c)),
                DirSeqPart.RUA_URA => Math.Min(SeqLength(DirSeqPart.RUA, depth, c), SeqLength(DirSeqPart.URA, depth, c)),
                DirSeqPart.LUA_ULA => Math.Min(SeqLength(DirSeqPart.LUA, depth, c), SeqLength(DirSeqPart.ULA, depth, c)),
                DirSeqPart.RvA_vRA => Math.Min(SeqLength(DirSeqPart.RvA, depth, c), SeqLength(DirSeqPart.vRA, depth, c)),
                _ => throw new InvalidOperationException()
            };

            c.Add((part, depth), result);

            return result;
        }

        long GuessDirSequenceLength(string seq, int depth, Dictionary<(DirSeqPart, int), long> cache)
        {
            char prev = 'A';

            long sum = 0;

            foreach (var k in seq)
            {
                int d1 = depth - 1;
                sum += prev switch
                {
                    'A' => k switch
                    {
                        '<' => SeqLength(DirSeqPart.vLLA, d1, cache), // "v<<A",
                        '^' => SeqLength(DirSeqPart.LA, d1, cache), // "<A",
                        '>' => SeqLength(DirSeqPart.vA, d1, cache), // "vA"
                        'v' => SeqLength(DirSeqPart.vLA_LvA, d1, cache), // "<vA",
                        'A' => SeqLength(DirSeqPart.A, d1, cache), // "A",
                        _ => throw new InvalidOperationException()
                    },
                    '<' => k switch
                    {
                        '<' => SeqLength(DirSeqPart.A, d1, cache), // "A",
                        'A' => SeqLength(DirSeqPart.RRUA, d1, cache), // ">>^A",
                        '^' => SeqLength(DirSeqPart.RUA, d1, cache), // ">^A",
                        'v' => SeqLength(DirSeqPart.RA, d1, cache), // ">A",
                        _ => throw new InvalidOperationException()
                    },
                    '>' => k switch
                    {
                        '>' => SeqLength(DirSeqPart.A, d1, cache), // "A",
                        'v' => SeqLength(DirSeqPart.LA, d1, cache), // "<A",
                        '^' => SeqLength(DirSeqPart.LUA_ULA, d1, cache), // "<^A",
                        'A' => SeqLength(DirSeqPart.UA, d1, cache), // "^A",
                        _ => throw new InvalidOperationException()
                    },
                    '^' => k switch
                    {
                        '^' => SeqLength(DirSeqPart.A, d1, cache), // "A",
                        '>' => SeqLength(DirSeqPart.RvA_vRA, d1, cache), // ">vA", // v>A ?
                        'A' => SeqLength(DirSeqPart.RA, d1, cache), // ">A",
                        '<' => SeqLength(DirSeqPart.vLA, d1, cache), // "v<A",
                        _ => throw new InvalidOperationException()
                    },
                    'v' => k switch
                    {
                        'v' => SeqLength(DirSeqPart.A, d1, cache), // "A",
                        '>' => SeqLength(DirSeqPart.RA, d1, cache), // ">A",
                        '<' => SeqLength(DirSeqPart.LA, d1, cache), // "<A",
                        'A' => SeqLength(DirSeqPart.RUA_URA, d1, cache), // ">^A",
                        _ => throw new InvalidOperationException()
                    },
                    _ => throw new InvalidOperationException()
                };

                prev = k;
            }

            return sum;
        }

        long Solve(string filename, int depth)
        {
            var numpad = GridHelper.Load(Path.Combine(Path.GetDirectoryName(filename)!, "numpad.txt"));

            var cache = new Dictionary<(DirSeqPart, int), long>();

            return File.ReadAllLines(filename).Sum(
                line => int.Parse(line.Substring(0, 3)) * BuildKeySequences(numpad, line).Min(s => GuessDirSequenceLength(s, depth, cache))
            );
        }

        protected override object Solve1(string filename)
        {
            return Solve(filename, 2);
        }

        protected override object Solve2(string filename)
        {
            return Solve(filename, 25);
        }

        public override object SolutionExample1 => 126384L;
        public override object SolutionPuzzle1 => 125742L;
        public override object SolutionExample2 => 154115708116294L;
        public override object SolutionPuzzle2 => 157055032722640L;
    }
}
