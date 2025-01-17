﻿using AoC.Util;

namespace AoC2023
{
    public class Day14 : AoC.DayBase
    {
        public override object SolutionExample1 => 136L;
        public override object SolutionPuzzle1 => 105982L;
        public override object SolutionExample2 => 64L;
        public override object SolutionPuzzle2 => 85175L;

        private void MoveNorth(Grid<char> grid)
        {
            foreach( var row in grid.Rows )
            {
                foreach( var coord in row )
                {
                    if (grid[coord] == 'O')
                    {
                        var p = coord;
                        while(!p.IsTopBorder && grid[p.Top] == '.')
                        {
                            grid.Set(p, '.');
                            grid.Set(p.Top, 'O');
                            p = p.Top;
                        }
                    }
                }
            }
        }

        private void MoveSouth(Grid<char> grid)
        {
            foreach (var row in grid.Rows.Reverse())
            {
                foreach (var coord in row)
                {
                    if (grid[coord] == 'O')
                    {
                        var p = coord;
                        while (!p.IsBottomBorder && grid[p.Bottom] == '.')
                        {
                            grid.Set(p, '.');
                            grid.Set(p.Bottom, 'O');
                            p = p.Bottom;
                        }
                    }
                }
            }
        }

        private void MoveWest(Grid<char> grid)
        {
            foreach (var col in grid.Columns)
            {
                foreach (var coord in col)
                {
                    if (grid[coord] == 'O')
                    {
                        var p = coord;
                        while (!p.IsLeftBorder && grid[p.Left] == '.')
                        {
                            grid.Set(p, '.');
                            grid.Set(p.Left, 'O');
                            p = p.Left;
                        }
                    }
                }
            }
        }

        private void MoveEast(Grid<char> grid)
        {
            foreach (var col in grid.Columns.Reverse())
            {
                foreach (var coord in col)
                {
                    if (grid[coord] == 'O')
                    {
                        var p = coord;
                        while (!p.IsRightBorder && grid[p.Right] == '.')
                        {
                            grid.Set(p, '.');
                            grid.Set(p.Right, 'O');
                            p = p.Right;
                        }
                    }
                }
            }
        }

        private long Eval(Grid<char> grid)
        {
            long sum = 0;

            foreach( var p in grid.AllCoordinates)
            {
                if (grid[p] == 'O')
                    sum += grid.Height - p.Y;
            }

            return sum;
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);

            MoveNorth(grid);

            return Eval(grid);
        }

        int FindCycle(Grid<char> grid)
        {
            var clones = new List<Grid<char>>();
                        
            for (int j = 0; j < 1_000_000_000; ++j)
            {
                MoveNorth(grid);
                MoveWest(grid);
                MoveSouth(grid);
                MoveEast(grid);

                for (int i = 0; i < clones.Count; ++i)
                {
                    if (clones[i].Equals(grid))
                    {
                        return (j - i);
                    }
                }
                
                clones.Add(grid.Clone());
            }

            return 0;
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);
            
            var cycle = FindCycle(grid.Clone());
            int limit = 1_000_000_000 - ((1_000_000_000 / cycle) * cycle);

            for (int j = 0; j < limit; ++j)
            {
                MoveNorth(grid);
                MoveWest(grid);
                MoveSouth(grid);
                MoveEast(grid);
            }

            long e = 0;
            do
            {
                e = Eval(grid);
                for (int j = 0; j < cycle; ++j)
                {
                    MoveNorth(grid);
                    MoveWest(grid);
                    MoveSouth(grid);
                    MoveEast(grid);
                }
            }
            while (Eval(grid) != e);

            // 85264 too high
            return Eval(grid);
        }
    }
}
