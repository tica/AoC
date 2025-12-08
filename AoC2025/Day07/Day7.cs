using AoC.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2025
{
    public class Day7 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);
            var start = grid.WhereValue('S').Single();
            var beams = new HashSet<Coord> { start };
            var nextbeams = new HashSet<Coord>();

            int numsplits = 0;

            do
            {
                foreach (var b in beams)
                {
                    switch (b.Bottom.Value)
                    {
                        case '^':
                            nextbeams.Add(b.BottomLeft);
                            nextbeams.Add(b.BottomRight);
                            numsplits += 1;
                            break;
                        case '.':
                            nextbeams.Add(b.Bottom);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                beams = nextbeams;
                nextbeams = new();
            } while (!beams.First().IsBottomBorder);

            return numsplits;
        }

        class Counter<T> : IEnumerable<KeyValuePair<T, long>> where T: notnull
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

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);
            var start = grid.WhereValue('S').Single();
            var beams = new Counter<Coord>();
            var nextbeams = new Counter<Coord>();

            beams.Add(start, 1);

            do
            {
                foreach (var (b, c) in beams)
                {
                    switch (b.Bottom.Value)
                    {
                        case '^':
                            nextbeams.Add(b.BottomLeft, c);
                            nextbeams.Add(b.BottomRight, c);
                            break;
                        case '.':
                            nextbeams.Add(b.Bottom, c);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                beams = nextbeams;
                nextbeams = new();
            } while (!beams.First().Key.IsBottomBorder);

            return beams.Sum(kvp => kvp.Value);
        }

        public override object SolutionExample1 => 21;
        public override object SolutionPuzzle1 => 1681;
        public override object SolutionExample2 => 40L;
        public override object SolutionPuzzle2 => 422102272495018L;
    }
}
