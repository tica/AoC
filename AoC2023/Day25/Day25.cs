using AoC2023.Util;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Grid = AoC2023.Util.Grid<char>;
using Coord = AoC2023.Util.Grid<char>.Coord;
using Range = AoC2023.Util.Range;
using System.IO;
using System.Text;
using System.ComponentModel.Design;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Serialization;
using System.Runtime.Intrinsics.X86;
using System.Diagnostics.Metrics;

namespace AoC2023
{
    public class Day25 : DayBase
    {
        public Day25(): base(25) { }

        public override object SolutionExample1 => 54;
        public override object SolutionPuzzle1 => 514794;
        public override object SolutionExample2 => throw new NotImplementedException();
        public override object SolutionPuzzle2 => throw new NotImplementedException();

        protected override object Solve1(string filename)
        {
            Console.WriteLine(filename);

            var graph = new Dictionary<string, List<string>>();

            foreach ( var line in System.IO.File.ReadAllLines(filename))
            {
                var split = line.Split(':');
                var left = split.First().Trim();
                var right = split.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

                foreach( var r in right)
                {
                    if( graph.ContainsKey(r) )
                    {
                        graph[r].Add(left);
                    }
                    else
                    {
                        graph[r] = new() { left };
                    }
                    if (graph.ContainsKey(left))
                    {
                        graph[left].Add(r);
                    }
                    else
                    {
                        graph[left] = new() { r };
                    }
                }
            }

            var cuts = new List<(string, string)>();

            if ( filename.Contains("example")) 
            {
                cuts.Add(("hfx", "pzl"));
                cuts.Add(("bvb", "cmg"));
                cuts.Add(("nvd", "jqt"));
            }
            else
            {
                // Determined via neato
                cuts.Add(("hqq", "xxq"));
                cuts.Add(("vkd", "qfb"));
                cuts.Add(("xzz", "kgl"));
            }

            foreach( var (a,b) in cuts )
            {
                graph[a].Remove(b);
                graph[b].Remove(a);
            }

            var group0 = new HashSet<string>();
            var open = new Queue<string>();
            open.Enqueue(graph.First().Key);
            
            while( open.Count > 0)
            {
                var item = open.Dequeue();
                group0.Add(item);

                foreach( var n in graph[item] )
                {
                    if( !group0.Contains(n) )
                    {
                        open.Enqueue(n);
                    }
                }
            }

            return group0.Count * (graph.Count - group0.Count);
        }

        protected override object Solve2(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
