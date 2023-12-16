using AoC2023.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AoC2023
{
    public class Day15 : DayBase
    {
        public Day15(): base(15) { }

        public override object SolutionExample1 => 1320L;
        public override object SolutionPuzzle1 => 517551L;
        public override object SolutionExample2 => 145L;
        public override object SolutionPuzzle2 => 286097L;

        protected override object Solve1(string filename)
        {
            byte cur = 0;
            long sum = 0;
            foreach(var b in System.IO.File.ReadAllBytes(filename).Skip(3))
            {
                if( (char)b == ',')
                {
                    //Console.WriteLine($" -> {cur}");
                    sum += cur;
                    cur = 0;
                }
                else
                {
                    //Console.Write((char)b);
                    cur = (byte)((cur + b) * 17);
                }
            }

            sum += cur;
            cur = 0;

            return sum;
        }
        
        private byte Hash(string txt)
        {
            byte cur = 0;
            foreach( var ch in txt)
            {
                byte b = (byte)ch;
                cur = (byte)((cur + b) * 17);
            }
            return cur;
        }

        protected override object Solve2(string filename)
        {
            var input = System.IO.File.ReadAllText(filename);

            var boxes = Enumerable.Range(0, 256).Select(_ => new List<(string, int)>()).ToArray();

            foreach( var cmd in input.Split(','))
            {
                if( cmd.EndsWith('-') )
                {
                    var name = cmd.Substring(0, cmd.Length - 1);
                    var h = Hash(name);

                    boxes[h].RemoveAll(x => x.Item1 == name);
                }
                else
                {
                    var m = Regex.Match(cmd, @"^(.+)=(\d+)$");
                    var name = m.Groups[1].Value;
                    var value = int.Parse(m.Groups[2].Value);

                    var h = Hash(name);

                    var i = boxes[h].FindIndex(x => x.Item1 == name);
                    if( i >= 0)
                    {
                        boxes[h][i] = (name, value);
                    }
                    else
                    {
                        boxes[h].Add((name, value));
                    }
                }

                /*
                for( int i = 0; i < 256; ++i )
                {
                    if (boxes[i].Count > 0)
                    {
                        Console.Write($"Box {i}: ");
                        foreach( var x in boxes[i])
                        {
                            Console.Write($"[{x.Item1} {x.Item2}] ");
                        }
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
                */
            }

            long sum = 0;
            for( int i = 0; i < 256; ++i)
            {
                for( int j = 0; j < boxes[i].Count; ++j)
                {
                    sum += (i + 1) * (j + 1) * boxes[i][j].Item2;
                }
            }

            return sum;
        }
    }
}
