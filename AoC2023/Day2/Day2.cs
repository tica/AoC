﻿using System;
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

            public bool Possible(int limitRed, int limitGreen, int limitBlue)
            {
                return CalcMax("red") <= limitRed
                    && CalcMax("green") <= limitGreen
                    && CalcMax("blue") <= limitBlue;
            }

            public int CalcPower()
            {
                return CalcMax("red") * CalcMax("green") * CalcMax("blue");
            }
        }

        private static int Solve1(string filename)
        {
            return System.IO.File.ReadAllLines(filename)
                .Select(Game.Parse)
                .Where(g => g.Possible(12, 13, 14))
                .Sum(g => g.Number);
        }

        private static int Solve2(string filename)
        {
            return System.IO.File.ReadAllLines(filename)
                .Select(Game.Parse)
                .Sum(g => g.CalcPower());
        }

        public static void Solve()
        {
            Console.WriteLine(Solve1("Day2/input.txt"));
            Console.WriteLine(Solve2("Day2/input.txt"));
        }
    }
}
