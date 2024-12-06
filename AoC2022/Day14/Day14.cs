using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2022
{
    public class Day14 : AoC.DayBase
    {
        const int WIDTH = 1000;
        const int HEIGHT = 200;

        void printGrid(Grid grid)
        {
            Console.Clear();
            for (int y = 0; y < 200; ++y)
            {
                for (int x = 450; x < 550; ++x)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }
        }

        bool dropSand(Grid grid)
        {
            var p = grid.Pos(500, 0);

            while (p.Y < HEIGHT - 1)
            {
                if (p.Bottom.Value == '.')
                {
                    p = p.Bottom;
                }
                else if (p.BottomLeft.Value == '.')
                {
                    p = p.BottomLeft;
                }
                else if (p.BottomRight.Value == '.')
                {
                    p = p.BottomRight;
                }
                else
                {
                    if (p.Value == '+')
                        return false;

                    grid.Set(p, 'o');
                    return true;
                }
            }

            return false;
        }

        private (Grid grid, int maxY) ParseInput(string filename)
        {
            var grid = new AoC.Util.Grid<char>(WIDTH, HEIGHT, '.');

            grid[500, 0] = '+';

            int maxY = 0;

            foreach (var line in File.ReadAllLines(filename))
            {
                int? prevX = null;
                int? prevY = null;
                foreach (var coord in line.Split(" -> ").Select(s => s.Split(',').Select(int.Parse).ToArray()))
                {
                    int x = coord[0];
                    int y = coord[1];

                    maxY = Math.Max(y, maxY);

                    if (prevX.HasValue && prevY.HasValue)
                    {
                        if (x == prevX.Value)
                        {
                            int begin = Math.Min(y, prevY.Value);
                            int end = Math.Max(y, prevY.Value);

                            for (int i = begin; i <= end; ++i)
                            {
                                grid[x, i] = '#';
                            }
                        }
                        else if (y == prevY.Value)
                        {
                            int begin = Math.Min(x, prevX.Value);
                            int end = Math.Max(x, prevX.Value);

                            for (int i = begin; i <= end; ++i)
                            {
                                grid[i, y] = '#';
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }

                    prevX = x;
                    prevY = y;
                }
            }

            return (grid, maxY);
        }

        protected override object Solve1(string filename)
        {
            var (grid, _) = ParseInput(filename);

            int count = 0;
            while (dropSand(grid))
            {
                count += 1;
            }

            //printGrid(grid);

            return count;
        }

        protected override object Solve2(string filename)
        {
            var (grid, maxY) = ParseInput(filename);

            for (int i = 0; i < WIDTH; ++i)
            {
                grid[i, maxY + 2] = '#';
            }

            int count = 0;
            while (dropSand(grid))
            {
                count += 1;
            }

            //printGrid(grid);

            return count + 1;
        }

        public override object SolutionExample1 => 24;
        public override object SolutionPuzzle1 => 757;
        public override object SolutionExample2 => 93;
        public override object SolutionPuzzle2 => 24943;
    }
}
