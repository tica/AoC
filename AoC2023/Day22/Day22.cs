using System.Text.RegularExpressions;

namespace AoC2023
{
    public class Day22 : AoC.DayBase
    {
        public override object SolutionExample1 => 5;
        public override object SolutionPuzzle1 => 507;
        public override object SolutionExample2 => 7;
        public override object SolutionPuzzle2 => 51733;

        record class Brick(int X0, int Y0, int Z0, int X1, int Y1, int Z1, string Name)
        {
            public static Brick Parse(string line)
            {
                var tagm = Regex.Match(line, @"<- (.+)");

                var m = Regex.Match(line, @"(\d+),(\d+),(\d+)~(\d+),(\d+),(\d+)");

                var x0 = int.Parse(m.Groups[1].Value);
                var x1 = int.Parse(m.Groups[4].Value);
                var y0 = int.Parse(m.Groups[2].Value);
                var y1 = int.Parse(m.Groups[5].Value);
                var z0 = int.Parse(m.Groups[3].Value);
                var z1 = int.Parse(m.Groups[6].Value);

                return new Brick(
                    X0: Math.Min(x0, x1),
                    Y0: Math.Min(y0, y1),
                    Z0: Math.Min(z0, z1),
                    X1: Math.Max(x0, x1),
                    Y1: Math.Max(y0, y1),
                    Z1: Math.Max(z0, z1),
                    Name: tagm.Success ? tagm.Groups[1].Value : ""
                );
            }
        }

        protected override object Solve1(string filename)
        {
            var bricks = System.IO.File.ReadAllLines(filename).Select(Brick.Parse).OrderBy(b => Math.Min(b.Z0, b.Z1)).ToList();

            var W = bricks.Max(b => Math.Max(b.X0, b.X1)) + 1;
            var H = bricks.Max(b => Math.Max(b.Y0, b.Y1)) + 1;

            var grid = new (int z, Brick brick)[W, H];

            HashSet<Brick> unremovable = new();

            foreach (var brick in bricks)
            {
                HashSet<Brick> touched = new();

                int maxZ = 0;

                for (int x = brick.X0; x <= brick.X1; ++x)
                {
                    for (int y = brick.Y0; y <= brick.Y1; ++y)
                    {
                        if (grid[x, y].z > maxZ)
                        {
                            touched = new() { grid[x, y].brick };
                            maxZ = grid[x, y].z;
                        }
                        else if( grid[x, y].z == maxZ)
                        {
                            touched.Add(grid[x, y].brick);
                        }
                    }
                }

                if( touched.Count == 1 && touched.First() != null )
                {                    
                    unremovable.Add(touched.First());
                }

                for (int x = brick.X0; x <= brick.X1; ++x)
                {
                    for (int y = brick.Y0; y <= brick.Y1; ++y)
                    {
                        grid[x, y] = (maxZ + 1 + brick.Z1 - brick.Z0, brick);
                    }
                }
            }

            return bricks.Count - unremovable.Count;
        }

        protected override object Solve2(string filename)
        {
            var bricks = System.IO.File.ReadAllLines(filename).Select(Brick.Parse).OrderBy(b => Math.Min(b.Z0, b.Z1)).ToList();

            var W = bricks.Max(b => Math.Max(b.X0, b.X1)) + 1;
            var H = bricks.Max(b => Math.Max(b.Y0, b.Y1)) + 1;

            var floor = new Brick(0, 0, 0, H - 1, W - 1, 0, "Floor");

            var grid = new (int z, Brick brick)[W, H];
            for( int x = 0; x < W; ++x)
            {
                for( int y = 0; y < H; ++y)
                {
                    grid[x, y] = (0, floor);
                }
            }

            Dictionary<Brick, List<Brick>> supportingBricks = new();

            foreach (var brick in bricks)
            {
                HashSet<Brick> touched = new();

                int maxZ = 0;

                for (int x = brick.X0; x <= brick.X1; ++x)
                {
                    for (int y = brick.Y0; y <= brick.Y1; ++y)
                    {
                        if (grid[x, y].z > maxZ)
                        {
                            touched = new() { grid[x, y].brick };
                            maxZ = grid[x, y].z;
                        }
                        else if (grid[x, y].z == maxZ)
                        {
                            touched.Add(grid[x, y].brick);
                        }
                    }
                }

                foreach (var b in touched)
                {
                    supportingBricks[brick] = touched.Where(t => t != null).ToList();
                }

                for (int x = brick.X0; x <= brick.X1; ++x)
                {
                    for (int y = brick.Y0; y <= brick.Y1; ++y)
                    {
                        grid[x, y] = (maxZ + 1 + brick.Z1 - brick.Z0, brick);
                    }
                }
            }

            int sum = 0;

            foreach( var brick in bricks )
            {
                HashSet<Brick> falling = new();
                HashSet<Brick> remaining = new(bricks);

                falling.Add(brick);
                remaining.Remove(brick);

                bool progress = false;

                do
                {
                    progress = false;

                    foreach( var b in remaining.Where(bb => supportingBricks[bb].All(falling.Contains)))
                    {
                        falling.Add(b);
                        remaining.Remove(b);
                        progress = true;
                    }
                }
                while (progress);

                sum += falling.Count - 1;
            }

            return sum;
        }
    }
}
