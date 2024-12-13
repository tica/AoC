using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day13 : AoC.DayBase
    {
        record class Machine(int AX, int AY, int BX, int BY, int PrizeX, int PrizeY);

        private IEnumerable<Machine> ParseInput(string filename)
        {
            var e = ((IEnumerable<string>)File.ReadAllLines(filename)).GetEnumerator();

            while( e.MoveNext())
            {
                var m = Regex.Match(e.Current, @"Button A: X\+(\d+), Y\+(\d+)");
                int ax = int.Parse(m.Groups[1].Value);
                int ay = int.Parse(m.Groups[2].Value);
                e.MoveNext();
                m = Regex.Match(e.Current, @"Button B: X\+(\d+), Y\+(\d+)");
                int bx = int.Parse(m.Groups[1].Value);
                int by = int.Parse(m.Groups[2].Value);
                e.MoveNext();
                m = Regex.Match(e.Current, @"Prize: X=(\d+), Y=(\d+)");
                int px = int.Parse(m.Groups[1].Value);
                int py = int.Parse(m.Groups[2].Value);
                yield return new Machine(ax, ay, bx, by, px, py);

                e.MoveNext();
            }
        }

        private (int a, int b)? FindBestSolution(Machine m)
        {
            (int a, int b)? result = null;

            for ( int a = 0; a <= 100; ++a)
            {
                int remainingX = m.PrizeX - a * m.AX;
                int remainingY = m.PrizeY - a * m.AY;
                if (remainingX < 0 || remainingY < 0)
                {
                    return result;
                }

                if (remainingX % m.BX != 0 || remainingY % m.BY != 0)
                    continue;

                int b = remainingX / m.BX;

                if (remainingY / m.BY != b)
                    continue;

                if(result == null || a +b < result.Value.a + result.Value.b)
                {
                    result = (a, b);
                }
            }

            return result;
        }

        protected override object Solve1(string filename)
        {
            int sum = 0;

            foreach( Machine m in ParseInput(filename))
            {
                var solution = FindBestSolution(m);
                if( solution != null )
                {
                    sum += solution.Value.a * 3 + solution.Value.b;
                }
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            return null!;
        }

        public override object SolutionExample1 => null!;
        public override object SolutionPuzzle1 => null!;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;
    }
}
