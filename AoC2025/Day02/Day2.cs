using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day2 : AoC.DayBase
    {
        (long, long) ParseLine(string line)
        {
            var r = line.Split('-').Select(long.Parse).ToArray();

            return (r[0], r[1]);
        }

        bool IsInvalidId(long id)
        {
            var s = id.ToString();
            var len = s.Length;

            if (len % 2 != 0)
                return false;

            return s.Substring(0, len / 2) == s.Substring(len / 2);
        }

        long AddInvalidIDs((long from, long to) range)
        {
            long r = 0;
            for(long i = range.from; i <= range.to; ++i)
            {
                if (IsInvalidId(i))
                    r += i;
            }
            return r;
        }

        protected override object Solve1(string filename)
        {
            return File.ReadAllText(filename).Split(',').Select(ParseLine).Sum(AddInvalidIDs);
        }

        private string Multiply(string s, int n)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < n; ++i)
                sb.Append(s);

            return sb.ToString();
        }

        bool IsInvalidId2(long id)
        {
            var s = id.ToString();
            var len = s.Length;

            for (int l = 1; l < len; ++l)
            {
                if (len % l != 0)
                    continue;

                string part = s.Substring(0, l);

                if (Multiply(part, len / l) == s)
                    return true;
            }

            return false;
        }

        long AddInvalidIDs2((long from, long to) range)
        {
            long r = 0;
            for (long i = range.from; i <= range.to; ++i)
            {
                if (IsInvalidId2(i))
                    r += i;
            }
            return r;
        }

        protected override object Solve2(string filename)
        {
            return File.ReadAllText(filename).Split(',').Select(ParseLine).Sum(AddInvalidIDs2);
        }

        public override object SolutionExample1 => 1227775554L;
        public override object SolutionPuzzle1 => 19219508902L;
        public override object SolutionExample2 => 4174379265L;
        public override object SolutionPuzzle2 => 27180728081L;
    }
}
