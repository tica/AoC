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
    public class Day20 : DayBase
    {
        public Day20(): base(20) { }

        public override object SolutionExample1 => throw new NotImplementedException();
        public override object SolutionPuzzle1 => throw new NotImplementedException();
        public override object SolutionExample2 => throw new NotImplementedException();
        public override object SolutionPuzzle2 => throw new NotImplementedException();

        enum Type
        {
            Broadcaster,
            FlipFlop,
            Conjunction,
            Output
        }

        record class Module(Type Type, string Name, List<string> Targets)
        {
            private Dictionary<string, bool> Sources = new();
            public bool State { get; internal set; }

            public static Module Parse(string input)
            {
                var m = Regex.Match(input, @"([%&])?(.+) -> (.+)");
                var type = m.Groups[1].Success ? (m.Groups[1].Value[0] == '%' ? Type.FlipFlop : Type.Conjunction) : Type.Broadcaster;
                var name = m.Groups[2].Value;
                var dest = m.Groups[3].Value.Split(',').Select(m => m.Trim()).ToList();

                return new Module(type, name, dest);
            }

            public void RegisterInput(string name)
            {
                Sources[name] = false;
            }

            public IEnumerable<(string From, bool Val, string To)> Pulse(bool v, string source)
            {
                switch(Type)
                {
                    case Type.Broadcaster:
                        foreach( var t in Targets)
                        {
                            //Console.WriteLine($"{Name} -{v}-> {t}");
                            yield return (Name, v, t);
                        }
                        break;
                    case Type.FlipFlop:
                        if (v)
                            yield break;
                        State = !State;
                        foreach (var t in Targets)
                        {
                            //Console.WriteLine($"{Name} -{State}-> {t}");
                            yield return (Name, State, t);
                        }
                        break;
                    case Type.Conjunction:
                        Sources[source] = v;
                        var p = !Sources.Values.All(v => v == true);
                        foreach (var t in Targets)
                        {
                            //Console.WriteLine($"{Name} -{p}-> {t}");
                            yield return (Name, p, t);
                        }
                        break;
                    case Type.Output:
                        State = v;
                        break;
                    default:
                        throw new Exception("oops");
                }
            }
        }

        protected override object Solve1(string filename)
        {
            var modules = File.ReadAllLines(filename).Select(Module.Parse).ToDictionary(m => m.Name);

            var output = new Module(Type.Output, "output", new());
            modules["output"] = output;
            var rx= new Module(Type.Output, "rx", new());
            modules["rx"] = output;

            var bc = modules["broadcaster"];

            foreach( var (n,m) in modules)
            {
                foreach( var t in m.Targets)
                {
                    modules[t].RegisterInput(m.Name);
                }
            }

            long numLow = 0;
            long numHigh = 0;

            for (int i = 0; i < 1000; ++i)
            {
                //Console.WriteLine($"Push {i}:");

                Queue<(string from, bool val, string to)> active = new();
                active.Enqueue(("button", false, "broadcaster"));

                while (active.Any())
                {
                    var (from, val, to) = active.Dequeue();

                    foreach (var t in modules[to].Pulse(val, from))
                    {
                        active.Enqueue(t);
                    }

                    if (val)
                        numHigh += 1;
                    else
                        numLow += 1;
                }
            }

            return numLow * numHigh;
        }

        protected override object Solve2(string filename)
        {
            var modules = File.ReadAllLines(filename).Select(Module.Parse).ToDictionary(m => m.Name);

            var output = new Module(Type.Output, "output", new());
            modules["output"] = output;
            var rx = new Module(Type.Output, "rx", new());
            modules["rx"] = output;

            var bc = modules["broadcaster"];

            foreach (var (n, m) in modules)
            {
                foreach (var t in m.Targets)
                {
                    modules[t].RegisterInput(m.Name);
                }
            }

            long count = 0;
            while( true )
            {
                count += 1;

                Queue<(string from, bool val, string to)> active = new();
                active.Enqueue(("button", false, "broadcaster"));

                while (active.Any())
                {
                    var (from, val, to) = active.Dequeue();

                    foreach (var t in modules[to].Pulse(val, from))
                    {
                        active.Enqueue(t);
                    }
                }

                if( rx.State )
                {
                    break;
                }

                if( count % 1000000 == 0)
                    Console.WriteLine(count);
            }

            return count;
        }
    }
}
