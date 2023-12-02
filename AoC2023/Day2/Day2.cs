using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2023.Day2
{
    internal class Day2
    {
        record Game(int Number, List<Dictionary<string, int>> Rounds)
        {
            static Dictionary<string, int> ParseRound(string input)
            {
                var colors = input.Split(',');

                return colors
                    .Select(c => Regex.Match(c, @"(\d+) (\w+)"))
                    .ToDictionary(
                        m => m.Groups[2].Value,
                        m => int.Parse(m.Groups[1].Value)
                    );
            }

            public static Game Parse(string line)
            {
                var parts = line.Split(':', ';');
                var m = Regex.Match(parts[0], @"Game (\d+)");
                var gameNumber = int.Parse(m.Groups[1].Value);
                return new Game(gameNumber, parts.Skip(1).Select(ParseRound).ToList());
            }

            public int CalcMax(string color)
            {
                return Rounds.Max(r => r.ContainsKey(color) ? r[color] : 0);
            }

            public int CalcPower()
            {
                return CalcMax("red") * CalcMax("green") * CalcMax("blue");
            }
        }

        private static int Solve(string filename)
        {
            int limitRed = 12;
            int limitGreen = 13;
            int limitBlue = 14;
            int sum = 0;

            foreach ( var line in System.IO.File.ReadAllLines(filename))
            {
                var game = Game.Parse(line);
                if (game.CalcMax("red") <= limitRed
                    && game.CalcMax("green") <= limitGreen
                    && game.CalcMax("blue") <= limitBlue)
                    sum += game.Number;
            }

            return sum;
        }

        private static int Solve2(string filename)
        {
            int sum = 0;

            foreach (var line in System.IO.File.ReadAllLines(filename))
            {
                var game = Game.Parse(line);
                int power = game.CalcPower();
                sum += power;
            }

            return sum;
        }

        public static void Solve()
        {
            int x = Solve2("Day2/input.txt");
            Console.WriteLine(x);
        }
    }
}
