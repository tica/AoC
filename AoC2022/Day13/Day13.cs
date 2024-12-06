using System.Numerics;

namespace AoC2022
{
    public class Day13 : AoC.DayBase
    {
        private class Packet
        {
            public abstract record Item : IComparable<Item>
            {
                public record List(IEnumerable<Item> Items) : Item
                {
                    public override string ToString()
                    {
                        return $"[{string.Join(',', Items)}]";
                    }

                    public static int Compare(List left, List right)
                    {
                        var l = left.Items.ToArray();
                        var r = right.Items.ToArray();
                        int n = Math.Max(l.Length, r.Length);
                        for (int i = 0; i < n; ++i)
                        {
                            if (i >= l.Length)
                                return -1;
                            if (i >= r.Length)
                                return 1;

                            int cmp = l[i].CompareTo(r[i]);
                            if (cmp != 0)
                                return cmp;
                        }

                        return 0;
                    }

                    public override int CompareTo(Item? other)
                    {
                        return other switch
                        {
                            List l => Compare(this, l),
                            Number n => CompareTo(new List(new[] { n })),
                            _ => throw new NotImplementedException()
                        };
                    }
                }
                public record Number(int Value) : Item
                {
                    public override string ToString()
                    {
                        return Value.ToString();
                    }

                    public override int CompareTo(Item? other)
                    {
                        return other switch
                        {
                            List l => new List(new[] { this }).CompareTo(other),
                            Number n => (Value - n.Value),
                            _ => throw new NotImplementedException()
                        };
                    }
                }

                public abstract int CompareTo(Item? other);
            }

            private static Item Parse(IEnumerator<char> input)
            {
                if (input.Current == '[')
                {
                    input.MoveNext();

                    List<Item> children = new List<Item>();
                    while (input.Current != ']')
                    {
                        children.Add(Parse(input));

                        if (input.Current == ',')
                            input.MoveNext();
                    }

                    input.MoveNext();

                    return new Item.List(children);
                }
                else if (char.IsNumber(input.Current))
                {
                    string numString = "";
                    while (char.IsNumber(input.Current))
                    {
                        numString += input.Current;
                        input.MoveNext();
                    }

                    return new Item.Number(int.Parse(numString));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            public static Item Parse(string str)
            {
                var enumerator = str.AsEnumerable().GetEnumerator();
                enumerator.MoveNext();

                return Parse(enumerator);
            }
        }

        protected override object Solve1(string filename)
        {
            var pairs = File.ReadAllLines(filename)
                .Chunk(3)
                .Select(ch => ch.Take(2)
                    .Select(Packet.Parse))
                    .Select(a => (a.First(), a.Last())
                );

            return pairs.Select((p, i) => new { Pair = p, Index = i })
                    .Sum(pi => (pi.Pair.Item1.CompareTo(pi.Pair.Item2) <= 0 ? pi.Index + 1 : 0));
        }

        protected override object Solve2(string filename)
        {
            var d0 = Packet.Parse("[[2]]");
            var d1 = Packet.Parse("[[6]]");
            var packets = File.ReadAllLines(filename)
                .Chunk(3)
                .SelectMany(ch => ch.Take(2).Select(Packet.Parse))
                .Append(d0)
                .Append(d1)
                .ToList();

            packets.Sort();

            return (packets.IndexOf(d0) + 1) * (packets.IndexOf(d1) + 1);
        }

        public override object SolutionExample1 => 13;
        public override object SolutionPuzzle1 => 5340;
        public override object SolutionExample2 => 140;
        public override object SolutionPuzzle2 => 21276;
    }
}
