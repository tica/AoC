using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2023
{
    public class Day4 : AoC.DayBase
    {
        public override object SolutionExample1 => 13;
        public override object SolutionPuzzle1 => 21138;
        public override object SolutionExample2 => 30;
        public override object SolutionPuzzle2 => 7185540;

        record class Card(int Number, List<int> Winning, List<int>Mine)
        {
            public static Card Parse(string line)
            {
                var m = Regex.Match(line, @"Card\s+(\d+):([^\|]+)\|(.+)");

                int n = int.Parse(m.Groups[1].Value);

                var winning = m.Groups[2].Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                var mine = m.Groups[3].Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                return new Card(n, winning, mine);
            }

            public int NumMatches => Mine.Count(Winning.Contains);

            public int Score
            {
                get
                {
                    return (int)Math.Pow(2, NumMatches - 1);
                }
            }
        }

        protected override object Solve1(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);

            var cards = lines.Select(Card.Parse).ToList();

            return cards.Sum(c => c.Score);
        }

        protected override object Solve2(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);

            var cards = lines.Select(Card.Parse).ToList();

            var numCards = Enumerable.Range(0, cards.Count + 1).ToDictionary(n => n, n => 1);
            numCards[0] = 0;

            foreach( var card in cards)
            {
                var m = numCards[card.Number];
                for ( int i = 0; i < card.NumMatches; ++i)
                {
                    int j = card.Number + 1 + i;
                    if ( numCards.ContainsKey(j))
                        numCards[j] += m;
                }
            }

            return numCards.Values.Sum();
        }
    }
}
