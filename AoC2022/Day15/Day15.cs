using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day15 : AoC.DayBase
    {
        internal record BlockRange(int From, int To)
        {
            public bool Overlaps(BlockRange other)
            {
                if (other.From > To)
                    return false;
                if (From > other.To)
                    return false;

                return true;
            }

            public bool IsBehind(BlockRange other)
            {
                return To > other.From;
            }

            public BlockRange Merge(BlockRange other)
            {
                return new BlockRange(Math.Min(From, other.From), Math.Max(To, other.To));
            }
        }

        void insertBlock(List<BlockRange> blocked, int from, int to)
        {
            if (to < from)
                return;

            var newBlock = new BlockRange(from, to);

            for (int i = 0; i < blocked.Count; ++i)
            {
                if (newBlock.Overlaps(blocked[i]))
                {
                    var merged = blocked[i].Merge(newBlock);
                    blocked.RemoveAt(i);

                    while (blocked.Count > i && blocked[i].Overlaps(merged))
                    {
                        merged = merged.Merge(blocked[i]);
                        blocked.RemoveAt(i);
                    }

                    blocked.Insert(i, merged);
                    return;
                }

                if (blocked[i].IsBehind(newBlock))
                {
                    blocked.Insert(i, newBlock);
                    return;
                }
            }

            blocked.Add(newBlock);
        }

        int count(List<BlockRange> blocked)
        {
            int sum = 0;

            foreach (var b in blocked)
            {
                sum += (b.To - b.From + 1);
            }

            return sum;
        }

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

        List<BlockRange> buildBlockInfo(List<((int, int), (int, int))> input, int lineNumber, bool blockBeacons)
        {
            var blocked = new List<BlockRange>();

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
                        insertBlock(blocked, sensorX - blockSize, beaconX - 1);
                        insertBlock(blocked, beaconX + 1, sensorX + blockSize);
                    }
                    else
                    {
                        insertBlock(blocked, sensorX - blockSize, sensorX + blockSize);
                    }
                }
            }

            return blocked;
        }

        int? findFree(List<BlockRange> blocks, int from, int to)
        {
            for (int i = 0; i < blocks.Count - 1; ++i)
            {
                if (blocks[i].To < blocks[i + 1].From + 1)
                {
                    int free = blocks[i].To + 1;

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
            return count(blocked10);
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

            Console.WriteLine("NOT FOUND");

            return 0;
        }

        public override object SolutionExample1 => 26;
        public override object SolutionPuzzle1 => 5838453;
        public override object SolutionExample2 => 56000011L;
        public override object SolutionPuzzle2 => 12413999391794L;
    }
}
