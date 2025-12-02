using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day1 : AoC.DayBase
    {
        static int ParseLine(string line)
        {
            if( line.StartsWith("L"))
            {
                return -int.Parse(line.Substring(1));
            }
            else
            {
                return int.Parse(line.Substring(1));
            }
        }

        protected override object Solve1(string filename)
        {
            int p = 50;
            int r = 0;

            foreach( var d in File.ReadAllLines(filename).Select(ParseLine) )
            {
                p += d;
                p %= 100;

                if (p == 0)
                    r += 1;
            }

            return r;
        }

        protected override object Solve2(string filename)
        {
            int p = 50;
            int r = 0;

            foreach (var d in File.ReadAllLines(filename).Select(ParseLine))
            {
                p += d;

                if (p <= 0)
                { 
                    r += (p / -100);
                    if (p != d)
                        r += 1;
                }
                if (p >= 100) r += (p / 100);

                p += 100000;
                p %= 100;
            }

            return r;
        }

        public override object SolutionExample1 => 3;
        public override object SolutionPuzzle1 => 1018;
        public override object SolutionExample2 => 6;
        public override object SolutionPuzzle2 => 5815;
    }
}
