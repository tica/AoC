using AoC.Util;
using Range = AoC.Util.Range;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day15 : AoC.DayBase
    {
        List<((int, int), (int, int))> ParseInput(string fileName)
        {
            return File.ReadAllLines(fileName).Select(
                line =>
                {
                    var m = Regex.Match(line, @"Sensor at x=(\d+), y=(\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

                    var sensor = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
                    var beacon = (int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));

                    return (sensor, beacon);
                }
            ).ToList();
        }

        RangeList buildBlockInfo(List<((int, int), (int, int))> input, int lineNumber, bool blockBeacons)
        {
            var blocked = new RangeList();

            foreach (var line in input)
            {
                ((int sensorX, int sensorY), (int beaconX, int beaconY)) = line;

                int distance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);
                int fromRow = Math.Abs(lineNumber - sensorY);
                int blockSize = distance - fromRow;

                if (blockSize >= 0)
                {
                    if (!blockBeacons && beaconY == lineNumber)
                    {
                        blocked.Add(Range.FromToInclusive(sensorX - blockSize, beaconX - 1));
                        blocked.Add(Range.FromToInclusive(beaconX + 1, sensorX + blockSize));
                    }
                    else
                    {
                        blocked.Add(Range.FromToInclusive(sensorX - blockSize, sensorX + blockSize));
                    }
                }
            }

            return blocked;
        }

        long? findFree(RangeList blocks, int from, int to)
        {
            foreach( var (l, r) in blocks.Ranges.Pairwise() )
            {
                if (l.End < r.Begin)
                {
                    var free = l.End;

                    if (free >= from && free <= to)
                    {
                        return free;
                    }
                }
            }

            return null;
        }

        protected override object Solve1(string filename)
        {
            var input = ParseInput(filename);
            int y = filename.Contains("example") ? 10 : 2000000;

            var blocked10 = buildBlockInfo(input, y, false);
            return blocked10.Ranges.Sum(b => b.Length);
        }

        protected override object Solve2(string filename)
        {
            var input = ParseInput(filename);

            int SEARCH_MAX = 4000000;

            for (int y = 0; y < SEARCH_MAX; ++y)
            {
                var blocked = buildBlockInfo(input, y, true);

                var free = findFree(blocked, 0, SEARCH_MAX);
                if (free.HasValue)
                {
                    return free.Value * 4000000L + y;
                }
            }

            throw new NotImplementedException();
        }

        public override object SolutionExample1 => 26L;
        public override object SolutionPuzzle1 => 5838453L;
        public override object SolutionExample2 => 56000011L;
        public override object SolutionPuzzle2 => 12413999391794L;
    }
}
