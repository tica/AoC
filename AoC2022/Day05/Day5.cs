using System;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day5 : AoC.DayBase
    {
        record class Command(int Num, int From, int To)
        {
            public static Command Parse(string line)
            {
                var m = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");

                var num = int.Parse(m.Groups[1].Value);
                var from = int.Parse(m.Groups[2].Value) - 1;
                var to = int.Parse(m.Groups[3].Value) - 1;

                return new Command(num, from, to);
            }
        }

        private (List<Stack<char>>, IEnumerable<Command>) ParseInput(string filename)
        {
            var input = System.IO.File.ReadAllLines(filename);

            int numColumns = input.First().Length / 4 + 1;
            var columns = Enumerable.Repeat(0, numColumns).Select(_ => new Stack<char>()).ToList();

            var boardLines = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).Reverse().Skip(1);

            foreach( var line in boardLines)
            {
                for (int i = 0; i < numColumns; ++i)
                {
                    int c = i * 4 + 1;
                    char ch = line[c];
                    if (ch != ' ')
                    {
                        columns[i].Push(ch);
                    }
                }
            }

            var commands = input.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).Select(Command.Parse);

            return (columns, commands);
        }

        protected override object Solve1(string filename)
        {
            var (columns, commands) = ParseInput(filename);

            foreach( var cmd in commands)
            {
                for (int i = 0; i < cmd.Num; ++i)
                {
                    columns[cmd.To].Push(columns[cmd.From].Pop());
                }
            }

            return string.Concat(columns.Select(c => c.Peek()));
        }

        protected override object Solve2(string filename)
        {
            var (columns, commands) = ParseInput(filename);

            foreach (var cmd in commands)
            {
                var temp = new Stack<char>();

                for (int i = 0; i < cmd.Num; ++i)
                {
                    temp.Push(columns[cmd.From].Pop());
                }

                for (int i = 0; i < cmd.Num; ++i)
                {
                    columns[cmd.To].Push(temp.Pop());
                }
            }

            return string.Concat(columns.Select(c => c.Peek()));
        }

        public override object SolutionExample1 => "CMZ";
        public override object SolutionPuzzle1 => "VGBBJCRMN";
        public override object SolutionExample2 => "MCD";
        public override object SolutionPuzzle2 => "LBBVJBRMH";
    }
}
