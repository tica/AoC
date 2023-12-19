using AoC2023.Util;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Grid = AoC2023.Util.Grid<char>;
using Range = AoC2023.Util.Range;

namespace AoC2023
{
    public class Day19 : DayBase
    {
        public Day19(): base(19) { }

        public override object SolutionExample1 => throw new NotImplementedException();
        public override object SolutionPuzzle1 => throw new NotImplementedException();
        public override object SolutionExample2 => throw new NotImplementedException();
        public override object SolutionPuzzle2 => throw new NotImplementedException();


        record struct Rule(char Aspect, char Op, int Comparison, string Result)
        {
            public static Rule Parse(string input)
            {
                if (input.Contains(":"))
                {
                    var m = Regex.Match(input, @"(.)([<>])(\d+):(.+)");
                    return new Rule
                    {
                        Aspect = m.Groups[1].Value[0],
                        Op = m.Groups[2].Value[0],
                        Comparison = int.Parse(m.Groups[3].Value),
                        Result = m.Groups[4].Value
                    };
                }
                else
                {
                    return new Rule { Aspect = ':', Op = ':', Comparison = 0, Result = input};
                }
            }
        }

        record struct RuleSet(string Name, List<Rule> Rules)
        {
            public static RuleSet Parse(string input)
            {
                var m = Regex.Match(input, @"(.+){(.+)}");

                return new RuleSet
                {
                    Name = m.Groups[1].Value,
                    Rules = m.Groups[2].Value.Split(',').Select(Rule.Parse).ToList()
                };
            }

            public string ApplyTo(Dictionary<char, int> item)
            {
                foreach (var rule in Rules)
                {
                    switch (rule.Op)
                    {
                        case ':': return rule.Result;
                        case '<':
                            if (item[rule.Aspect] < rule.Comparison)
                                return rule.Result;
                            break;
                        case '>':
                            if (item[rule.Aspect] > rule.Comparison)
                                return rule.Result;
                            break;
                        default:
                            throw new Exception("oops");
                    }
                }

                throw new Exception("oops");
            }
        }

        Dictionary<char, int> ParseData(string input)
        {
            return input.Split(new char[] { ',', '{', '}' }, StringSplitOptions.RemoveEmptyEntries)
                .ToDictionary(s => s.Split('=').First()[0], s => int.Parse(s.Split('=').Last()));
        }

        private bool ProcessItem(Dictionary<string, RuleSet> rules, Dictionary<char, int> item)
        {
            var rule = rules["in"];

            Console.Write($"in ");

            while (true)
            {
                var next = rule.ApplyTo(item);

                Console.Write($"{next} ");

                if (next == "A")
                    return true;
                if (next == "R")
                    return false;

                rule = rules[next];
            }
        }

        protected override object Solve1(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename).ToList();

            var rulesInput = lines.TakeWhile(s => !string.IsNullOrEmpty(s)).ToList();
            var dataInput = lines.Skip(rulesInput.Count + 1).ToList();

            var rules = rulesInput.Select(RuleSet.Parse).ToDictionary(r => r.Name);

            long result = 0;

            foreach( var data in dataInput)
            {
                var item = ParseData(data);

                Console.Write($"{data} ");

                if (ProcessItem(rules, item))
                {
                    Console.WriteLine();

                    Console.WriteLine("A");
                    var rr = item['x'] + item['m'] + item['a'] + item['s'];
                    result += rr;
                }
                else
                {
                    Console.WriteLine("R");
                }
            }

            return result;
        }

        protected override object Solve2(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
