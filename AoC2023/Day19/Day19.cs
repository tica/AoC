using AoC2023.Util;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Range = AoC2023.Util.Range;

namespace AoC2023
{
    public class Day19 : DayBase
    {
        public Day19(): base(19) { }

        public override object SolutionExample1 => 19114L;
        public override object SolutionPuzzle1 => 353553L;
        public override object SolutionExample2 => 167409079868000L;
        public override object SolutionPuzzle2 => 124615747767410L;


        record class Rule(char Aspect, char Op, int Comparison, string Result)
        {
            public RuleSet ResultingRuleSet { get; set; } = null!;

            //public void ResolveLink(RuleSet rs)

            public static Rule Parse(string input)
            {
                if (input.Contains(":"))
                {
                    var m = Regex.Match(input, @"(.)([<>])(\d+):(.+)");
                    return new Rule(
                        m.Groups[1].Value[0],
                        m.Groups[2].Value[0],
                        int.Parse(m.Groups[3].Value),
                        m.Groups[4].Value
                    );
                }
                else
                {
                    return new Rule(':', ':', 0, input);
                }
            }
        }

        record class RuleSet(string Name, List<Rule> Rules)
        {
            public static RuleSet Parse(string input)
            {
                var m = Regex.Match(input, @"(.+){(.+)}");

                return new RuleSet(m.Groups[1].Value, m.Groups[2].Value.Split(',').Select(Rule.Parse).ToList());
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

            while (true)
            {
                var next = rule.ApplyTo(item);

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

                if (ProcessItem(rules, item))
                {
                    var rr = item['x'] + item['m'] + item['a'] + item['s'];
                    result += rr;
                }
            }

            return result;
        }

        private void CollectAcceptingRules(RuleSet ruleSet, string ruleString, List<string> output)
        {
            var str = ruleString;

            foreach (var rule in ruleSet.Rules)
            {
                string rs = "";
                string invrs = "";
                if( rule.Op == ':' )
                {
                }
                else if( rule.Op == '<')
                {
                    rs = $" && {rule.Aspect}<{rule.Comparison}";
                    invrs = $" && {rule.Aspect}>={rule.Comparison}";
                }
                else if( rule.Op == '>')
                {
                    rs = $" && {rule.Aspect}>={rule.Comparison + 1}";
                    invrs = $" && {rule.Aspect}<{rule.Comparison + 1}";
                }
                else
                {
                    throw new Exception("oops");
                }

                if (rule.Result == "A")
                {
                    output.Add(ruleString + rs);
                    ruleString += invrs;
                }
                else if( rule.Result == "R")
                {
                    ruleString += invrs;
                }
                else
                {
                    CollectAcceptingRules(rule.ResultingRuleSet, ruleString + rs, output);
                    ruleString += invrs;
                }
            }
        }

        private void PrintRanges(Dictionary<char, RangeList> rls)
        {
            foreach( var (k,v) in rls)
            {
                Console.Write($"'{k}': ");

                foreach( var r in v.Ranges )
                {
                    Console.Write($"[{r.Begin}..{r.End}[ ");
                }
                Console.WriteLine();
            }
        }

        protected override object Solve2(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename).ToList();

            var rules = lines.TakeWhile(s => !string.IsNullOrEmpty(s)).Select(RuleSet.Parse).ToDictionary(r => r.Name);

            foreach( var rs in rules.Values )
            {
                foreach( var r in rs.Rules )
                {
                    if (r.Result == "A")
                        continue;
                    if (r.Result == "R")
                        continue;
                    r.ResultingRuleSet = rules[r.Result];
                }
            }

            // We know that our input is actually a tree (no cycles, every rule can be evaluated only once)


            var rulesStrings = new List<string>();
            CollectAcceptingRules(rules["in"], "", rulesStrings);

            long sum = 0;

            foreach ( var rs in rulesStrings )
            {
                //Console.WriteLine(rs);

                Dictionary<char, RangeList> test = new()
                {
                    { 'x', new RangeList() { Ranges = { new Range(1, 4000) }} },
                    { 'm', new RangeList() { Ranges = { new Range(1, 4000) }} },
                    { 'a', new RangeList() { Ranges = { new Range(1, 4000) }} },
                    { 's', new RangeList() { Ranges = { new Range(1, 4000) }} },
                };

                var parts = rs.Split(" && ", StringSplitOptions.RemoveEmptyEntries);
                foreach( var p in parts)
                {
                    var ml = Regex.Match(p, @"(.)<(\d+)");
                    var mg = Regex.Match(p, @"(.)>=(\d+)");
                    if ( ml.Success )
                    {
                        var c = ml.Groups[1].Value[0];
                        var v = int.Parse(ml.Groups[2].Value);

                        test[c].Subtract(new Range(v, 4000));
                    }
                    else if( mg.Success )
                    {
                        var c = mg.Groups[1].Value[0];
                        var v = int.Parse(mg.Groups[2].Value);

                        test[c].Subtract(new Range(0, v));
                    }
                    else
                    {
                        throw new Exception("oops");
                    }
                }

                //PrintRanges(test);
                long cx = test['x'].Ranges.Sum(r => r.Length);
                long cm = test['m'].Ranges.Sum(r => r.Length);
                long ca = test['a'].Ranges.Sum(r => r.Length);
                long cs = test['s'].Ranges.Sum(r => r.Length);

                var cp = cx * cm * ca * cs;
                //Console.WriteLine($" = {cx} * {cm} * {ca} * {cs} = {cp}");

                sum += cp;
            }

            return sum;
        }
    }
}
