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
        record class Machine(int AX, int AY, int BX, int BY, long PrizeX, long PrizeY);

        private IEnumerable<Machine> ParseInput(string filename, long addToPrize = 0)
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
                yield return new Machine(ax, ay, bx, by, px + addToPrize, py + addToPrize);

                e.MoveNext();
            }
        }

        private (long a, long b)? FindBestSolution(Machine m)
        {
            // AX * a + BX * b = PX
            // AY * a + BY * b = PY

            // a + BX/AX * b = PX/AX
            // a = PX/AX - BX/AX * b
            // a = (PX - BX * b) / AX

            // AY * (PX/AX - BX/AX * b) + BY * b = PY
            // AY*PX/AX - AY*BX/AX * b + BY * b = PY
            // BY * b - AY*BX/AX * b = PY - AY*PX/AX
            // AX*BY * b - AY*BX * b = PY*AX - AY*PX
            // b * (AX*BY - AY*BX) = PY*AX - AY*PX
            // b = (PY*AX - AY*PX) / (AX*BY - AY*BX)            

            if ((m.PrizeY * m.AX - m.AY * m.PrizeX) % (m.AX * m.BY - m.AY * m.BX) != 0)
                return null;

            long b = (m.PrizeY * m.AX - m.AY * m.PrizeX) / (m.AX * m.BY - m.AY * m.BX);

            if (((m.PrizeX - m.BX * b) % m.AX) != 0)
                return null;

            long a = (m.PrizeX - m.BX * b) / m.AX;

            return (a, b);            
        }

        protected override object Solve1(string filename)
        {
            long sum = 0;

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
            long sum = 0;

            foreach (Machine m in ParseInput(filename, 10000000000000))
            {
                var solution = FindBestSolution(m);
                if (solution != null)
                {
                    sum += solution.Value.a * 3 + solution.Value.b;
                }
            }

            return sum;
        }

        public override object SolutionExample1 => 480L;
        public override object SolutionPuzzle1 => 29201L;
        public override object SolutionExample2 => 875318608908L;
        public override object SolutionPuzzle2 => 104140871044942L;
    }
}
