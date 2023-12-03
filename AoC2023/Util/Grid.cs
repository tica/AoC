using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023.Util
{
    internal class Grid
    {
        private char[][] data;

        public int Width { get; init; }
        public int Height { get; init; }

        public Grid(string fileName)
        {
            data = System.IO.File.ReadAllLines(fileName)
                .Select(s => s.ToArray())
                .ToArray();
            Width = data[0].Length;
            Height = data.Length;
        }

        public IEnumerable<Coord> AllCoordinates
        {
            get
            {
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        yield return new Coord(this, x, y);
                    }
                }
            }
        }

        public char this[Coord p] => Get(p);

        public char Get(Coord p)
        {
            return data[p.Y][p.X];
        }
        public void Set(Coord p, char val)
        {
            data[p.Y][p.X] = val;
        }

        public char Exchange(Coord p, char val)
        {
            var r = Get(p);
            Set(p, val);
            return r;
        }

        public record Coord(Grid Parent, int X, int Y)
        {
            public override string ToString()
            {
                return $"({X}, {Y})";
            }

            public static Coord Invalid = new Coord(null!, 0, 0);

            public bool IsValid => Parent != null;

            public bool IsLeftBorder => X == 0;
            public bool IsRightBorder => X == Parent.Width - 1;
            public bool IsTopBorder => Y == 0;
            public bool IsBottomBorder => Y == Parent.Height - 1;

            public static Coord operator ++(Coord p)
            {
                return p.Right;
            }
            public static Coord operator --(Coord p)
            {
                return p.Left;
            }

            public Coord TopLeft
            {
                get
                {
                    if (IsLeftBorder || IsTopBorder)
                        return Invalid;

                    return new Coord(Parent, X - 1, Y - 1);
                }
            }
            public Coord Left
            {
                get
                {
                    if (IsLeftBorder)
                        return Invalid;

                    return new Coord(Parent, X - 1, Y);
                }
            }
            public Coord BottomLeft
            {
                get
                {
                    if (IsLeftBorder || IsBottomBorder)
                        return Invalid;

                    return new Coord(Parent, X - 1, Y + 1);
                }
            }
            public Coord Top
            {
                get
                {
                    if (IsTopBorder)
                        return Invalid;

                    return new Coord(Parent, X, Y - 1);
                }
            }
            public Coord Bottom
            {
                get
                {
                    if (IsTopBorder)
                        return Invalid;

                    return new Coord(Parent, X, Y + 1);
                }
            }
            public Coord TopRight
            {
                get
                {
                    if (IsRightBorder || IsTopBorder)
                        return Invalid;

                    return new Coord(Parent, X + 1, Y - 1);
                }
            }
            public Coord Right
            {
                get
                {
                    if (IsRightBorder)
                        return Invalid;

                    return new Coord(Parent, X + 1, Y);
                }
            }
            public Coord BottomRight
            {
                get
                {
                    if (IsRightBorder || IsBottomBorder)
                        return Invalid;

                    return new Coord(Parent, X + 1, Y + 1);
                }
            }

            public IEnumerable<Coord> AdjacentCoords
            {
                get
                {
                    if (!IsLeftBorder)
                    {
                        if (!IsTopBorder) yield return TopLeft;
                        yield return Left;
                        if (!IsBottomBorder) yield return BottomLeft;
                    }
                    if (!IsTopBorder) yield return Top;
                    if (!IsBottomBorder) yield return Bottom;
                    if (!IsRightBorder)
                    {
                        if (!IsTopBorder) yield return TopRight;
                        yield return Right;
                        if (!IsBottomBorder) yield return BottomRight;
                    }
                }
            }
        }
    };
}
