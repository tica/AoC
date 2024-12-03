using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AoC2023
{
    public class Day7 : AoC.DayBase
    {
        public override object SolutionExample1 => 6440L;

        public override object SolutionPuzzle1 => 246409899L;

        public override object SolutionExample2 => 5905L;

        public override object SolutionPuzzle2 => 244848487L;

        record class Hand(string Cards, int Bid) : IComparable<Hand>
        {
            public static Hand Parse(string line)
            {
                var m = Regex.Match(line, @"(.....)\s(\d+)");

                var cards = m.Groups[1].Value;
                var bid = int.Parse(m.Groups[2].Value);

                return new Hand(cards, bid);
            }

            public int Value
            {
                get
                {
                    Dictionary<char, int> counts = new();

                    foreach( var c in Cards )
                    {
                        counts[c] = Cards.Count(x => x == c);
                    }

                    if (counts.Values.Contains(5))
                        return 5000;
                    if (counts.Values.Contains(4))
                        return 4000;
                    if (counts.Values.Contains(3) && counts.Values.Contains(2))
                        return 3500;
                    if (counts.Values.Contains(3))
                        return 3000;
                    if (counts.Values.Count(x => x == 2) == 2)
                        return 2500;
                    if (counts.Values.Contains(2))
                        return 2000;
                    return 1000;
                }
            }

            private static char FixChar(char ch)
            {
                switch (ch)
                {
                    case 'A': return 'A';
                    case 'K': return 'B';
                    case 'Q': return 'C';
                    case 'J': return 'D';
                    case 'T': return 'E';
                    case '9': return 'F';
                    case '8': return 'G';
                    case '7': return 'H';
                    case '6': return 'I';
                    case '5': return 'J';
                    case '4': return 'K';
                    case '3': return 'L';
                    case '2': return 'M';
                    default:
                        throw new Exception("oops");
                }
            }

            public int CompareTo(Hand? other)
            {
                var v0 = this.Value;
                var v1 = other!.Value;

                if (v0 != v1)
                    return v1 - v0;

                var s0 = new String(Cards.Select(FixChar).ToArray());
                var s1 = new String(other.Cards.Select(FixChar).ToArray());

                return s0.CompareTo(s1);
            }
        }

        protected override object Solve1(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);

            var hands = lines.Select(Hand.Parse).ToList();

            hands.Sort();

            long result = 0;

            for( int i = 0; i < hands.Count; i++ )
            {
                int rank = hands.Count - i;
                //Console.WriteLine($"{hands[i].Cards} -> {hands[i].Value}: Rank {rank}");

                result += hands[i].Bid * (long)rank;
            }

            return result;
        }

        record class Hand2(string Cards, int Bid) : IComparable<Hand2>
        {
            public static Hand2 Parse(string line)
            {
                var m = Regex.Match(line, @"(.....)\s(\d+)");

                var cards = m.Groups[1].Value;
                var bid = int.Parse(m.Groups[2].Value);

                return new Hand2(cards, bid);
            }

            public int Value
            {
                get
                {
                    Dictionary<char, int> counts = new();

                    var jokers = Cards.Count(x => x == 'J');

                    int maxCount = 0;
                    char maxChar = '\0';

                    foreach (var c in Cards)
                    {
                        if (c != 'J')
                        {
                            int count = Cards.Count(x => x == c); ;
                            counts[c] = count;

                            if( count > maxCount)
                            {
                                maxCount = count;
                                maxChar = c;
                            }
                        }
                    }

                    if (counts.ContainsKey(maxChar))
                        counts[maxChar] += jokers;

                    if (jokers == 5)
                        return 5000;

                    if (counts.Values.Contains(5))
                        return 5000;
                    if (counts.Values.Contains(4))
                        return 4000;
                    if (counts.Values.Contains(3) && counts.Values.Contains(2))
                        return 3500;
                    if (counts.Values.Contains(3))
                        return 3000;
                    if (counts.Values.Count(x => x == 2) == 2)
                        return 2500;
                    if (counts.Values.Contains(2))
                        return 2000;
                    return 1000;
                }
            }

            private static char FixChar(char ch)
            {
                switch (ch)
                {
                    case 'A': return 'A';
                    case 'K': return 'B';
                    case 'Q': return 'C';
                    case 'T': return 'E';
                    case '9': return 'F';
                    case '8': return 'G';
                    case '7': return 'H';
                    case '6': return 'I';
                    case '5': return 'J';
                    case '4': return 'K';
                    case '3': return 'L';
                    case '2': return 'M';
                    case 'J': return 'X';
                    default:
                        throw new Exception("oops");
                }
            }

            public int CompareTo(Hand2? other)
            {
                var v0 = this.Value;
                var v1 = other!.Value;

                if (v0 != v1)
                    return v1 - v0;

                var s0 = new String(Cards.Select(FixChar).ToArray());
                var s1 = new String(other.Cards.Select(FixChar).ToArray());

                return s0.CompareTo(s1);
            }
        }

        protected override object Solve2(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);

            var hands = lines.Select(Hand2.Parse).ToList();

            hands.Sort();

            long result = 0;

            for (int i = 0; i < hands.Count; i++)
            {
                int rank = hands.Count - i;
                Console.WriteLine($"{hands[i].Cards} -> {hands[i].Value}: Rank {rank}");

                result += hands[i].Bid * (long)rank;
            }

            return result;
        }
    }
}
