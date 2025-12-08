using AoC.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day6 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var lines = File.ReadAllLines(filename).ToList();
            var rows = lines.Take(lines.Count - 1).Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()).ToList();
            var operations = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            long sum = 0;

            for( int i = 0; i < operations.Count; ++i)
            {
                switch(operations[i])
                {
                    case "+":
                        sum += rows.Select(r => r[i]).Sum();
                        break;
                    case "*":
                        sum += rows.Select(r => r[i]).Product();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            var lines = File.ReadLines(filename).ToList();
            var transposed = Enumerable.Range(0, lines[0].Length).Reverse().Select(i => new string(lines.Select(line => line[i]).ToArray()).Trim());

            long sum = 0;
            List<int> numbers = new();
            foreach( var line in transposed)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if(line.EndsWith('+'))
                {
                    numbers.Add(int.Parse(line.Substring(0, line.Length - 1)));
                    sum += numbers.Sum();
                    numbers.Clear();
                }
                else if(line.EndsWith('*'))
                {
                    numbers.Add(int.Parse(line.Substring(0, line.Length - 1)));
                    sum += numbers.Product();
                    numbers.Clear();
                }
                else
                {
                    numbers.Add(int.Parse(line));
                }
            }

            // 48650431226 low

            return sum;
        }

        public override object SolutionExample1 => 4277556L;
        public override object SolutionPuzzle1 => 6299564383938L;
        public override object SolutionExample2 => 3263827L;
        public override object SolutionPuzzle2 => 11950004808442L;
    }
}
