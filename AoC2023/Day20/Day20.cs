using System.Text.RegularExpressions;

using AoC.Util;

namespace AoC2023
{
    public class Day20 : AoC.DayBase
    {
        public override object SolutionExample1 => 11687500L;
        public override object SolutionPuzzle1 => 944750144L;
        public override object SolutionExample2 => 0L;
        public override object SolutionPuzzle2 => 222718819437131L;

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
            public long LowCount { get; internal set; }

            public List<Module> TargetLinks { get; } = new();
            public List<Module> SourceLinks { get; } = new();

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

            public void ResolveTargetLinks(Dictionary<string, Module> modules)
            {
                foreach( var t in Targets)
                {
                    TargetLinks.Add(modules[t]);
                }
                foreach( var (n,v) in Sources)
                {
                    SourceLinks.Add(modules[n]);
                }
            }

            public IEnumerable<(string From, bool Val, Module To)> Pulse(bool v, string source)
            {
                if (!v)
                    LowCount += 1;

                switch (Type)
                {
                    case Type.Broadcaster:
                        foreach( var t in TargetLinks)
                        {
                            //Console.WriteLine($"{Name} -{v}-> {t.Name}");
                            yield return (Name, v, t);
                        }
                        break;
                    case Type.FlipFlop:
                        if (v)
                            yield break;
                        State = !State;
                        foreach (var t in TargetLinks)
                        {
                            //Console.WriteLine($"%{Name} -{State}-> {t.Name}");
                            yield return (Name, State, t);
                        }
                        break;
                    case Type.Conjunction:
                        Sources[source] = v;
                        //foreach (var (sn, sv) in Sources)
                        //{
                        //    Console.Write($"[{sn}] = {sv} ");
                        //}
                        //Console.WriteLine();
                        var p = !Sources.Values.All(v => v == true);
                        foreach (var t in TargetLinks)
                        {
                            //Console.WriteLine($"&{Name} -{p}-> {t.Name}");
                            yield return (Name, p, t);
                        }
                        break;
                    case Type.Output:
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

            var broadcaster = modules["broadcaster"];

            foreach (var (n, m) in modules)
            {
                foreach (var t in m.Targets)
                {
                    modules[t].RegisterInput(m.Name);
                }
            }
            foreach(var (n,m) in modules)
            {
                m.ResolveTargetLinks(modules);
            }

            long numLow = 0;
            long numHigh = 0;

            for (int i = 0; i < 1000; ++i)
            {
                //Console.WriteLine($"Push {i}:");

                Queue<(string from, bool val, Module to)> active = new();
                active.Enqueue(("button", false, broadcaster));

                while (active.Any())
                {
                    var (from, val, to) = active.Dequeue();

                    foreach (var t in to.Pulse(val, from))
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

        private long CalcPeriod(Module m)
        {
            switch(m.Type)
            {
                case Type.Output:
                    return CalcPeriod(m.SourceLinks.Single());
                case Type.FlipFlop:
                    var periods = m.SourceLinks.Select(m => CalcPeriod(m)).ToArray();
                    var freqs = periods.Select(p => 1.0 / p).ToArray();
                    var r = 2 / freqs.Sum();
                    return (long)r;
                case Type.Conjunction:
                    return MathFunc.LCM(m.SourceLinks.Select(CalcPeriod).ToArray());
                case Type.Broadcaster:
                    return 1;
                default:
                    throw new Exception("oops");
            }
        }

        protected override object Solve2(string filename)
        {
            if (filename.Contains("example"))
                return 0L;

            var modules = File.ReadAllLines(filename).Select(Module.Parse).ToDictionary(m => m.Name);

            var rx = new Module(Type.Output, "rx", new());
            modules["rx"] = rx;
            rx.State = true;

            var broadcaster = modules["broadcaster"];

            foreach (var (n, m) in modules)
            {
                foreach (var t in m.Targets)
                {
                    modules[t].RegisterInput(m.Name);
                }
            }
            foreach (var (n, m) in modules)
            {
                m.ResolveTargetLinks(modules);
            }

            var sj = modules["sj"];
            var qq = modules["qq"];
            var bg = modules["bg"];
            var ls = modules["ls"];
            var test = new List<Module> { sj, qq, bg, ls };
            var firstLow = new Dictionary<Module, long>();

            long count = 0;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while( true )
            {
                //Console.WriteLine();
                //Console.WriteLine($"Push {count}");
                count += 1;

                Queue<(string from, bool val, Module to)> active = new();
                active.Enqueue(("button", false, broadcaster));

                while (active.Any())
                {
                    var (from, val, to) = active.Dequeue();

                    foreach (var t in to.Pulse(val, from))
                    {
                        active.Enqueue(t);
                    }
                }

                foreach( var t in test )
                {
                    if( t.LowCount > 0 )
                    {
                        if( !firstLow.ContainsKey(t) )
                        {
                            firstLow.Add(t, count);
                            t.LowCount = 0;
                        }
                    }
                }

                if( firstLow.Count == 4 )
                {
                    return MathFunc.LCM(firstLow.Values.ToArray());
                }
            }
        }
    }
}
