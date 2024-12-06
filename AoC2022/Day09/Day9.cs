using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day9 : AoC.DayBase
    {
        struct Vector
        {
            public int X;
            public int Y;

            public static Vector operator +(Vector a, Vector b)
            {
                return new Vector { X = a.X + b.X, Y = a.Y + b.Y };
            }
        };

        Vector VectorFromDirection(char direction)
        {
            return direction switch
            {
                'L' => new Vector { X = -1, Y = 0 },
                'R' => new Vector { X = 1, Y = 0 },
                'U' => new Vector { X = 0, Y = -1 },
                'D' => new Vector { X = 0, Y = 1 },
            };
        }

        bool MustFollow(Vector head, Vector tail)
        {
            int dx = head.X - tail.X;
            int dy = head.Y - tail.Y;

            int maxd = Math.Max(Math.Abs(dx), Math.Abs(dy));

            return maxd >= 2;
        }
        Vector Follow(Vector head, Vector tail)
        {
            if (head.X == tail.X)
            {
                if (tail.Y < head.Y)
                    return new Vector { X = tail.X, Y = tail.Y + 1 };
                else
                    return new Vector { X = tail.X, Y = tail.Y - 1 };
            }
            else if (head.Y == tail.Y)
            {
                if (tail.X < head.X)
                    return new Vector { X = tail.X + 1, Y = tail.Y };
                else
                    return new Vector { X = tail.X - 1, Y = tail.Y };
            }

            int dx = Math.Sign(head.X - tail.X);
            int dy = Math.Sign(head.Y - tail.Y);

            return new Vector { X = tail.X + dx, Y = tail.Y + dy };
        }

        private int Solve(string filename, int ropeLength)
        {
            HashSet<Vector> visited = new HashSet<Vector>();
            List<Vector> rope = Enumerable.Repeat(new Vector { X = 0, Y = 0 }, ropeLength).ToList();

            foreach (var line in File.ReadLines(filename))
            {
                var m = Regex.Match(line, @"([LRUD]) (\d+)");
                var direction = VectorFromDirection(m.Groups[1].Value[0]);
                var distance = int.Parse(m.Groups[2].Value);

                for (int i = 0; i < distance; ++i)
                {
                    rope[0] += direction;

                    for (int j = 1; j < ropeLength; ++j)
                    {
                        if (MustFollow(rope[j - 1], rope[j]))
                        {
                            rope[j] = Follow(rope[j - 1], rope[j]);
                        }
                    }

                    visited.Add(rope.Last());
                }
            }

            return visited.Count;
        }

        protected override object Solve1(string filename)
        {
            return Solve(filename, 2);
        }

        protected override object Solve2(string filename)
        {
            return Solve(filename, 10);
        }

        public override object SolutionExample1 => 13;
        public override object SolutionPuzzle1 => 5710;
        public override object SolutionExample2 => 36;
        public override object SolutionPuzzle2 => 2259;
    }
}
