using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023.Util
{
    internal class Grid : IEquatable<Grid>
    {
        private List<List<char>> data;

        public int Width { get; private set;}
        public int Height { get; private set; }

        public Grid(string fileName)
        {
            data = System.IO.File.ReadAllLines(fileName)
                .Select(s => s.ToList())
                .ToList();
            Width = data[0].Count;
            Height = data.Count;
        }

        private Grid(List<List<char>> d)
        {
            data = d;
            Width = data[0].Count;
            Height = data.Count;
        }

        public Grid Clone()
        {
            return new Grid(data.Select(
                x => new List<char>(x)).ToList());
        }

        public bool Equals(Grid? other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (Width != other.Width)
                return false;
            if (Height != other.Height)
                return false;

            foreach(var p in AllCoordinates)
            {
                if (this[p] != other[p])
                    return false;
            }

            return true;
        }

        public static IEnumerable<Grid> LoadMultiple(string filename)
        {
            var current = new List<List<char>>();

            foreach(var line in System.IO.File.ReadAllLines(filename))
            {
                if( line.Length > 0)
                {
                    current.Add(line.ToList());
                }
                else
                {
                    yield return new Grid(current);
                    current = new();
                }
            }

            if( current.Count > 0 )
                yield return new Grid(current);
        }

        public void Print()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    var c = new Grid.Coord(this, x, y);
                    Console.Write(this[c]);
                }
                Console.WriteLine();
            }
        }

        public void InsertRow(int y, char c)
        {
            data.Insert(y, Enumerable.Repeat(c, Width).ToList());
            Height += 1;
        }

        public void InsertColumn(int x, char c)
        {
            foreach (var row in data)
            {
                row.Insert(x, c);
            }
            Width += 1;
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

        public IEnumerable<Coord> Row(int y)
        {
            for(int x = 0; x < Width; ++x)
            {
                yield return new Coord(this, x, y);
            }
        }

        public IEnumerable<IEnumerable<Coord>> Rows
        {
            get
            {
                for (int y = 0; y < Height; ++y)
                {
                    yield return Row(y);
                }
            }
        }
        public IEnumerable<IEnumerable<Coord>> Columns
        {
            get
            {
                for (int x = 0; x < Width; ++x)
                {
                    yield return Column(x);
                }
            }
        }

        private ulong ToBits(List<Coord> coords, Func<char, bool> predicate)
        {
            if (coords.Count > 64)
                throw new InvalidOperationException();

            ulong result = 0;
            for (int i = 0; i < coords.Count; ++i)
            {
                if (predicate(this[coords[i]]))
                {
                    result |= (1ul << i);
                }
            }

            return result;
        }

        public ulong RowBits(int y, Func<char, bool> predicate)
        {
            return ToBits(Row(y).ToList(), predicate);
        }
        public ulong ColumnBits(int x, Func<char, bool> predicate)
        {
            return ToBits(Column(x).ToList(), predicate);
        }

        public IEnumerable<Coord> Column(int x)
        {
            for (int y = 0; y < Height; ++y)
            {
                yield return new Coord(this, x, y);
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
                    if (IsBottomBorder)
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

            public IEnumerable<Coord> NeighborCoords
            {
                get
                {
                    if (!IsLeftBorder) yield return Left;
                    if (!IsTopBorder) yield return Top;
                    if (!IsBottomBorder) yield return Bottom;
                    if (!IsRightBorder) yield return Right;
                }
            }
        }
    };
}
