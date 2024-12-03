using System.Text.RegularExpressions;
using Range = AoC.Util.Range;

namespace AoC2023
{
    public class Day5 : AoC.DayBase
    {
        public override object SolutionExample1 => 35L;
        public override object SolutionPuzzle1 => 662197086L;
        public override object SolutionExample2 => 46L;
        public override object SolutionPuzzle2 => 52510809L;


        record class RangeMapping(long Destination, Range SourceRange)
        {
            public bool Contains(long val) => SourceRange.Contains(val);

            public long MapValue(long val)
            {
                if (!Contains(val))
                    throw new Exception("oops");

                return val - SourceRange.Begin + Destination;
            }

            public (Range? mapped, Range? unmapped1, Range? unmapped2) MapRange(Range range)
            {
                if (range.Begin >= SourceRange.End) // Right side mismatch
                    return (null, range, null);

                if (range.End <= SourceRange.Begin) // Left side mismatch
                    return (null, range, null);

                Range? unmappedLeft = range.Begin < SourceRange.Begin ? new Range(range.Begin, SourceRange.Begin - range.Begin ) : null;
                var intersection = SourceRange.Intersect(range);
                Range mapped = new Range(MapValue(intersection.Begin), intersection.Length);
                Range? unmappedRight = range.End > SourceRange.End ? new Range(SourceRange.End, range.End - SourceRange.End) : null;

                return (mapped, unmappedLeft, unmappedRight);
            }
        }

        record class Map(string SourceName, string DestinationName, List<RangeMapping> Mappings)
        {
            private static IEnumerable<RangeMapping> ParseRanges(IEnumerator<string> input)
            {
                while( input.Current != "")
                {
                    var m = Regex.Match(input.Current, @"(\d+) (\d+) (\d+)");

                    yield return new RangeMapping(
                        long.Parse(m.Groups[1].Value),
                        new Range(long.Parse(m.Groups[2].Value), long.Parse(m.Groups[3].Value))
                    );

                    if (!input.MoveNext())
                        break;
                }
            }

            public static Map Parse(IEnumerator<string> input)
            {
                var m = Regex.Match(input.Current, "(.+)-to-(.+) map:");
                if (!m.Success)
                    throw new Exception("oops!?");

                input.MoveNext();

                var sourceName = m.Groups[1].Value;
                var destinationName  = m.Groups[2].Value;

                return new Map(sourceName, destinationName, ParseRanges(input).ToList());
            }

            public long MapValue(long val)
            {
                foreach( var range in Mappings)
                {
                    if (range.Contains(val))
                        return range.MapValue(val);
                }

                return val;
            }

            public List<Range> MapRanges(IEnumerable<Range> ranges)
            {
                List<Range> result = new();
                List<Range> unmapped = ranges.ToList();

                while (unmapped.Count > 0)
                {
                    var x = unmapped.Last();
                    unmapped.RemoveAt(unmapped.Count - 1);

                    bool ignored = true;
                    foreach (var m in Mappings)
                    {
                        if (m.SourceRange.Intersects(x))
                        {
                            var (r, u1, u2) = m.MapRange(x);

                            if (r != null)
                                result.Add(r);
                            if (u1 != null)
                                unmapped.Add(u1);
                            if (u2 != null)
                                unmapped.Add(u2);

                            ignored = false;
                        }
                    }

                    if (ignored)
                        result.Add(x);
                }

                return result;
            }
        }

        protected override object Solve1(string filename)
        {
            IEnumerator<string> lines = System.IO.File.ReadAllLines(filename).ToList().GetEnumerator();
            lines.MoveNext();
            var seedsLine = lines.Current;

            var seeds = seedsLine.Substring(7).Split(' ').Select(long.Parse).ToList();

            var seedRanges = seeds.Select(s => new Range(s, 1));

            lines.MoveNext();

            List<Map> maps = new();
            while( lines.MoveNext())
            {
                maps.Add(Map.Parse(lines));
            }

            var x = seedRanges;

            foreach( var map in maps )
            {
                x = map.MapRanges(x);
            }

            return x.Min(r => r.Begin);
        }

        protected override object Solve2(string filename)
        {
            IEnumerator<string> lines = System.IO.File.ReadAllLines(filename).ToList().GetEnumerator();
            lines.MoveNext();
            var seedsLine = lines.Current;

            var seedsNumbers = seedsLine.Substring(7).Split(' ').Select(long.Parse).ToList();
            var seeds = new List<Range>();

            for( int i = 0; i < seedsNumbers.Count; i += 2 )
            {
                seeds.Add(new Range(seedsNumbers[i], seedsNumbers[i + 1]));
            }

            lines.MoveNext();

            List<Map> maps = new();
            while (lines.MoveNext())
            {
                maps.Add(Map.Parse(lines));
            }

            var x = seeds;

            foreach (var map in maps)
            {
                x = map.MapRanges(x);
            }

            return x.Min(r => r.Begin);
        }
    }
}
