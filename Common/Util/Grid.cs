using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public enum Direction
    {
        Right, Left, Up, Down,
    }

    public static class DirectionHelper
    {
        public static Direction TurnRight(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new ArgumentException(nameof(direction))
            };
        }

        public static Direction TurnLeft(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Right => Direction.Up,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                _ => throw new ArgumentException(nameof(direction))
            };
        }

        public static Direction Reverse(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => throw new ArgumentException(nameof(direction))
            };
        }

        public static Direction Parse(char value)
        {
            return value switch
            {
                '<' => Direction.Left,
                '>' => Direction.Right,
                '^' => Direction.Up,
                'v' => Direction.Down,
                _ => throw new ArgumentException(nameof(value))
            };
        }
    }

    public static class GridHelper
    {
        public static Grid<char> Load(string path)
        {
            var data = System.IO.File.ReadAllLines(path)
                .Select(s => s.ToList())
                .ToList();

            return new Grid<char>(data);
        }

        public static Grid<T> Load<T>(string path, Func<char, T> convert) where T : IEquatable<T>
        {
            var data = System.IO.File.ReadAllLines(path)
                .Select(s => s.Select(convert).ToList())
                .ToList();

            return new Grid<T>(data);
        }

        public static IEnumerable<Grid<char>> LoadMultiple(string filename)
        {
            var current = new List<List<char>>();

            foreach (var line in System.IO.File.ReadAllLines(filename))
            {
                if (line.Length > 0)
                {
                    current.Add(line.ToList());
                }
                else
                {
                    yield return new Grid<char>(current);
                    current = new();
                }
            }

            if (current.Count > 0)
                yield return new Grid<char>(current);
        }
    }

    public class Grid<T> : IEquatable<Grid<T>> where T: IEquatable<T>
    {
        private List<List<T>> data;

        public int Width { get; private set;}
        public int Height { get; private set; }

        internal Grid(List<List<T>> d)
        {
            data = d;
            Width = data[0].Count;
            Height = data.Count;
        }

        public Grid(int w, int h, T val)
        {
            data = new();
            for (int y = 0; y < h; ++y)
            {
                data.Add(Enumerable.Repeat(val, w).ToList());
            }
            Width = w;
            Height = h;
        }

        public Grid<T> Clone()
        {
            return new Grid<T>(data.Select(
                x => new List<T>(x)).ToList());
        }

        public bool Equals(Grid<T>? other)
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
                if (!this[p].Equals(other[p]))
                    return false;
            }

            return true;
        }

        public Grid<TResult> Transform<TResult>(Func<T, TResult> selector) where TResult: IEquatable<TResult>
        {
            return new Grid<TResult>(
                data.Select(
                    arr => arr.Select(selector).ToList()
                ).ToList()
            ); ;
        }

        public int Count(Func<T, bool> predicate)
        {
            return AllCoordinates.Count(p => predicate(this[p]));
        }

        public Coord Pos(int x, int y)
        {
            return new Coord(this, x, y);
        }

        public Coord TopLeft => Pos(0, 0);
        public Coord BottomRight => Pos(Width - 1, Height - 1);

        public int Distance(Coord a, Coord b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public void Print()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    var c = new Grid<T>.Coord(this, x, y);
                    Console.Write(this[c]);
                }
                Console.WriteLine();
            }
        }

        public void Print(Func<Coord, ConsoleColor> colorSelector)
        {
            var oldColor = Console.ForegroundColor;

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    var c = new Grid<T>.Coord(this, x, y);
                    Console.ForegroundColor = colorSelector(c);
                    Console.Write(c.Value);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = oldColor;
        }

        public void Print(Func<T, char> selector)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    var c = new Grid<T>.Coord(this, x, y);
                    Console.Write(selector(this[c]));
                }
                Console.WriteLine();
            }
        }

        public void InsertRow(int y, T c)
        {
            data.Insert(y, Enumerable.Repeat(c, Width).ToList());
            Height += 1;
        }

        public void InsertColumn(int x, T c)
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

        public IEnumerable<T> AllValues => AllCoordinates.Select(c => c.Value);

        public IEnumerable<Coord> WhereValue(T val)
        {
            return AllCoordinates.Where(c => c.Value.Equals(val));
        }

        public IEnumerable<Coord> Row(int y)
        {
            for(int x = 0; x < Width; ++x)
            {
                yield return new Coord(this, x, y);
            }
        }

        public IEnumerable<Coord> FirstRow => Row(0);

        public IEnumerable<Coord> FirstColumn => Column(0);
        public IEnumerable<Coord> LastRow => Row(Height - 1); 
        public IEnumerable<Coord> LastColumn => Column(Width - 1);

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
        public IEnumerable<IEnumerable<Coord>> Diagonals
        {
            get
            {
                for (int y = 0; y < Height; ++y)
                {
                    yield return DiagonalUp(0, y);
                    yield return DiagonalDown(0, y);
                }
                for (int x = 1; x < Width; ++x)
                {
                    yield return DiagonalDown(x, 0);
                    yield return DiagonalUp(x, Height - 1);
                }
            }
        }

        private ulong ToBits(List<Coord> coords, Func<T, bool> predicate)
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

        public ulong RowBits(int y, Func<T, bool> predicate)
        {
            return ToBits(Row(y).ToList(), predicate);
        }
        public ulong ColumnBits(int x, Func<T, bool> predicate)
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

        public IEnumerable<Coord> DiagonalDown(int x, int y)
        {
            while( x < Width && y < Height )
            {
                yield return new Coord(this, x, y);

                x += 1;
                y += 1;
            }
        }

        public IEnumerable<Coord> DiagonalUp(int x, int y)
        {
            while (x < Width && y >= 0)
            {
                yield return new Coord(this, x, y);

                x += 1;
                y -= 1;
            }
        }

        public T this[Coord p] => Get(p);
        public T this[int x, int y]
        {
            get => data[y][x];
            set
            {
                data[y][x] = value;
            }
        }

        public T Get(Coord p)
        {
            return data[p.Y][p.X];
        }
        public void Set(Coord p, T val)
        {
            data[p.Y][p.X] = val;
        }
        public void Set(int X, int Y, T val)
        {
            data[Y][X] = val;
        }

        public T Exchange(Coord p, T val)
        {
            var r = Get(p);
            Set(p, val);
            return r;
        }

        public IEnumerable<Coord> Between(Coord p1, Coord p2)
        {
            if (p1.Y == p2.Y)
            {
                var x1 = Math.Min(p1.X, p2.X);
                var x2 = Math.Max(p1.X, p2.X);
                for (int x = x1 + 1; x < x2; ++x)
                {
                    yield return new Coord(this, x, p1.Y);
                }
            }
            else if (p1.X == p2.X)
            {
                var y1 = Math.Min(p1.Y, p2.Y);
                var y2 = Math.Max(p1.Y, p2.Y);
                for (int y = y1 + 1; y < y2; ++y)
                {
                    yield return new Coord(this, p1.X, y);
                }
            }
            else
            {
                throw new InvalidOperationException("Not on the samw row/col");
            }
        }

        public record Coord(Grid<T> Parent, int X, int Y)
        {
            public override string ToString()
            {
                return $"({X}, {Y} [{Value}] )";
            }

            public static Coord Invalid = new Coord(null!, 0, 0);

            public bool IsValid => Parent != null;

            public T Value => Parent[this];

            public bool IsLeftBorder => X == 0;
            public bool IsRightBorder => X == Parent.Width - 1;
            public bool IsTopBorder => Y == 0;
            public bool IsBottomBorder => Y == Parent.Height - 1;
            public bool IsAnyBorder => IsLeftBorder || IsRightBorder || IsTopBorder || IsBottomBorder;

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

            public Coord Neighbor(Direction dir)
            {
                switch (dir)
                {
                    case Direction.Left:
                        return Left;
                    case Direction.Right:
                        return Right;
                    case Direction.Up:
                        return Top;
                    case Direction.Down:
                        return Bottom;
                    default:
                        throw new Exception("oops");
                }
            }

            public T? NeighborValue(Direction dir)
            {
                var p = Neighbor(dir);
                if (p.IsValid)
                    return p.Value;
                return default;
            }

            public Coord Move(int dx, int dy)
            {
                if (X + dx < 0)
                    return Invalid;
                if (X + dx >= Parent.Width)
                    return Invalid;
                if (Y + dy < 0)
                    return Invalid;
                if (Y + dy >= Parent.Height)
                    return Invalid;

                return new Coord(Parent, X + dx, Y + dy);
            }

            public int ManhattanDistance(Coord to)
            {
                return Math.Abs(X - to.X) + Math.Abs(Y - to.Y);
            }

            public IEnumerable<Coord> WithinRange(int range)
            {
                for( int dy = -range; dy <= range; ++dy)
                {
                    int maxdx = range - Math.Abs(dy);
                    for( int dx = -maxdx; dx <= maxdx; ++dx )
                    {
                        var c = Move(dx, dy);
                        if( c.IsValid )
                        {
                            yield return c;
                        }
                    }
                }
            }

            public static Vec operator -(Coord left, Coord right)
            {
                if (!left.IsValid)
                    throw new InvalidOperationException();
                if (!right.IsValid)
                    throw new InvalidOperationException();

                return new Vec(left.X - right.X, left.Y - right.Y);
            }

            public static Coord operator +(Coord left, Vec delta)
            {
                return left.Move(delta.dx, delta.dy);
            }

            public static Coord operator -(Coord left, Vec delta)
            {
                return left.Move(-delta.dx, -delta.dy);
            }
        }

        public record class Vec(int dx, int dy)
        {
            public Vec Normalized
            {
                get
                {
                    int div = AoC.Util.MathFunc.GCD(dx, dy);
                    return new Vec(dx / div, dy / div);
                }
            }
        }
    };
}
