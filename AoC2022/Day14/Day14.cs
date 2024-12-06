namespace AoC2022
{
    public class Day14 : AoC.DayBase
    {
        const int WIDTH = 1000;
        const int HEIGHT = 1000;

        void printGrid(char[,] grid)
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

        bool dropSand(char[,] grid)
        {
            int x = 500; int y = 0;

            while (y < HEIGHT - 1)
            {
                if (grid[x, y + 1] == '.')
                {
                    y += 1;
                }
                else if (grid[x - 1, y + 1] == '.')
                {
                    y += 1;
                    x -= 1;
                }
                else if (grid[x + 1, y + 1] == '.')
                {
                    y += 1;
                    x += 1;
                }
                else
                {
                    if (grid[x, y] == '+')
                        return false;

                    grid[x, y] = 'o';
                    return true;
                }
            }

            return false;
        }

        private (char[,] grid, int maxY) ParseInput(string filename)
        {
            char[,] grid = new char[1000, 1000];

            for (int y = 0; y < HEIGHT; ++y)
            {
                for (int x = 0; x < WIDTH; ++x)
                {
                    grid[x, y] = '.';
                }
            }

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
