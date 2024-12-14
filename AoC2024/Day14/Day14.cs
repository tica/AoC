using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day14 : AoC.DayBase
    {
        class Robot
        {
            public int PositionX { get; set; }
            public int PositionY { get; set; }

            public int VelocityX { get; set; }
            public int VelocityY { get; set; }

            public static Robot Parse(string line)
            {
                var m = Regex.Match(line, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
                var pv = m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).ToArray();

                return new Robot
                {
                    PositionX = pv[0],
                    PositionY = pv[1],
                    VelocityX = pv[2],
                    VelocityY = pv[3],
                };
            }

            public override string ToString()
            {
                return $"P=({PositionX}, {PositionY}) V=({VelocityX}, {VelocityY})";
            }
        }

        private void Print(List<Robot> robots, int width, int height, bool quadrants)
        {
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (quadrants && (x == width / 2 || y == height / 2))
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        int count = robots.Count(r => r.PositionX == x && r.PositionY == y);
                        Console.Write((count > 0) ? count.ToString() : ".");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        protected override object Solve1(string filename)
        {
            var robots = File.ReadAllLines(filename).Select(Robot.Parse).ToList();
            //var robots = new List<Robot> { Robot.Parse("p=2,4 v=2,-3") };

            int width = filename.Contains("example") ? 11 : 101;
            int height = filename.Contains("example") ? 7 : 103;

            int t = 100;

            foreach (var r in robots)
            {
                r.PositionX = (r.PositionX + (width + r.VelocityX) * t) % width;
                r.PositionY = (r.PositionY + (height + r.VelocityY) * t) % height;
            }

            int q00 = robots.Count(r => r.PositionY < height / 2 && r.PositionX < width / 2);
            int q01 = robots.Count(r => r.PositionY < height / 2 && r.PositionX > width / 2);
            int q10 = robots.Count(r => r.PositionY > height / 2 && r.PositionX < width / 2);
            int q11 = robots.Count(r => r.PositionY > height / 2 && r.PositionX > width / 2);

            return q00 * q01 * q10 * q11;
        }

        private bool ContainsHorizontalLine(List<Robot> robots, int width, int height, int len)
        {
            for (int y = 0; y < height; ++y)
            {
                int seq = 0;

                for (int x = 0; x < width; ++x)
                {
                    if( robots.Any(r => r.PositionX == x && r.PositionY == y) )
                    {
                        seq += 1;
                        if (seq >= len)
                            return true;
                    }
                    else
                    {
                        seq = 0;
                    }
                }
            }

            return false;
        }

        protected override object Solve2(string filename)
        {
            if (filename.Contains("example")) return 0;

            var robots = File.ReadAllLines(filename).Select(Robot.Parse).ToList();

            int width = filename.Contains("example") ? 11 : 101;
            int height = filename.Contains("example") ? 7 : 103;

            for( int n = 1; ; ++n)
            {
                foreach (var r in robots)
                {
                    r.PositionX = (r.PositionX + width + r.VelocityX) % width;
                    r.PositionY = (r.PositionY + height + r.VelocityY) % height;
                }

                if (ContainsHorizontalLine(robots, width, height, 8))
                {
                    return n;

                    //Print(robots, width, height, false);
                    //Console.WriteLine(n);
                    //Console.ReadKey();
                }
            }
        }

        public override object SolutionExample1 => 12;
        public override object SolutionPuzzle1 => 230461440;
        public override object SolutionExample2 => 0;
        public override object SolutionPuzzle2 => 6668;
    }
}
