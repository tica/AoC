namespace AoC2022
{
    public class Day17 : AoC.DayBase
    {
        record struct Piece(bool[,] Map)
        {
            public int Width => Map.GetLength(1);
            public int Height => Map.GetLength(0);            
        }

        readonly static Piece[] Pieces = new[]
        {
            new Piece(new[,] {
                { true, true, true, true }
            }),
            new Piece(new[,] {
                { false, true, false },
                { true, true, true },
                { false, true, false }
            }),
            new Piece(new[,] {
                { false, false, true },
                { false, false, true },
                { true, true, true }
            }),
            new Piece(new[,] {
                { true },
                { true },
                { true },
                { true }
            }),
            new Piece(new[,] {
                { true, true },
                { true, true }
            }),
        };

        class VirtualGrid
        {
            public List<bool[]> Rows = new();

            public int TotalHeight
            {
                get
                {
                    for (int i = Rows.Count - 1; i >= 0;  --i)
                    {
                        if (Rows[i].Any(b => b))
                            return i + 1;
                    }

                    return 0;
                }
            }
            
            public bool Get(int row, int col)
            {
                if( row < Rows.Count )
                {
                    return Rows[row][col];
                }

                return false;
            }

            public void Put(Piece piece, int row, int col)
            {
                for( int y = 0; y < piece.Height; ++y)
                {
                    for (int x = 0; x < piece.Width; ++x)
                    {
                        if (piece.Map[y,x])
                            Put(row - y, col + x);
                    }
                }
            }

            private void Put(int row, int col)
            {
                while (row >= Rows.Count)
                {
                    Rows.Add(new bool[7]);
                }

                Rows[row][col] = true;
            }

            public bool Collides(Piece piece, int row, int col)
            {
                if (col < 0)
                    return true;
                if (col + piece.Width > 7)
                    return true;

                for (int y = 0; y < piece.Height; ++y)
                {
                    if (row + y < 0)
                        return true;

                    for (int x = 0; x < piece.Width; ++x)
                    {
                        if (col + x < 0)
                            return true;
                        if (col + x >= 7)
                            return true;

                        if (Get(row - y, col + x) && (piece.Map[y, x]))
                            return true;
                    }
                }

                return false;
            }
        }

        private long Solve(string filename, long rockLimit)
        {
            var pattern = File.ReadAllText(filename);

            var grid = new VirtualGrid();

            int activeIndex = 0;
            Piece piece = Pieces[activeIndex];
            int col = 2;
            int row = grid.TotalHeight + 2 + piece.Height;

            long numrocks = 0;
            int prevheight = 0;
            long prevrocks = 0;

            List<(int, long)> deltas = new();

            long skipRocks = 0;
            long skippedHeight = 0;

            while (numrocks < (rockLimit - skipRocks))
            {
                int deltaHeight = grid.TotalHeight - prevheight;
                long deltaRocks = numrocks - prevrocks;

                var delta = (deltaHeight, deltaRocks);

                int cycledetect = 3;

                if (deltas.Count(d => d == delta) == cycledetect)
                {
                    int cycleStart = deltas.FindIndex(d => d == delta);
                    for (int i = 0; i < cycledetect - 2; ++i)
                        cycleStart = deltas.FindIndex(cycleStart + 1, d => d == delta);

                    int cycleLength = deltas.FindLastIndex(d => d == delta) - cycleStart;
                    int cycleSumHeight = deltas.Skip(cycleStart).Take(cycleLength).Sum(d => d.Item1);
                    long cycleSumRocks = deltas.Skip(cycleStart).Take(cycleLength).Sum(d => d.Item2);

                    long skipCycles = (rockLimit - numrocks) / cycleSumRocks;
                    skipRocks = skipCycles * cycleSumRocks;
                    skippedHeight = skipCycles * cycleSumHeight;
                }

                deltas.Add(delta);
                prevheight = grid.TotalHeight;
                prevrocks = numrocks;

                foreach (var dir in pattern)
                {
                    if (dir == '>' && !grid.Collides(piece, row, col + 1))
                    {
                        col += 1;
                    }
                    else if (dir == '<' && !grid.Collides(piece, row, col - 1))
                    {
                        col -= 1;
                    }

                    if (grid.Collides(piece, row - 1, col))
                    {
                        grid.Put(piece, row, col);

                        if (++numrocks == (rockLimit - skipRocks))
                            break;

                        activeIndex += 1;
                        activeIndex %= Pieces.Length;
                        piece = Pieces[activeIndex];
                        col = 2;
                        row = grid.TotalHeight + 2 + piece.Height;

                        /*
                        Console.WriteLine();
                        for (int y = row; y >= 0; --y)
                        {
                            Console.Write('|');
                            if (row - y < piece.Height)
                            {
                                for (int x = 0; x < 7; ++x)
                                {
                                    if (x - col >= 0 && x - col < piece.Width)
                                    {
                                        Console.Write(piece.Map[row - y, x - col] ? '@' : '.');
                                    }
                                    else
                                    {
                                        Console.Write('.');
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < 7; ++x)
                                {
                                    Console.Write(grid.Get(y, x) ? '#' : '.');
                                }
                            }
                            Console.WriteLine('|');
                        }
                        Console.WriteLine("+-------+");
                        Console.ReadKey();
                        */
                    }
                    else
                    {
                        row -= 1;
                    }
                }
            }

            return grid.TotalHeight + skippedHeight;
        }

        protected override object Solve1(string filename)
        {
            return Solve(filename, 2022);
        }

        protected override object Solve2(string filename)
        {
            return Solve(filename, 1000000000000);
        }

        public override object SolutionExample1 => 3068L;
        public override object SolutionPuzzle1 => 3067L;
        public override object SolutionExample2 => 1514285714288L;
        public override object SolutionPuzzle2 => 1514369501484L;
    }
}
