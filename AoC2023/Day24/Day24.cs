﻿using AoC2023.Util;
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
    public class Day24 : DayBase
    {
        public Day24(): base(24) { }

        public override object SolutionExample1 => 2L;
        public override object SolutionPuzzle1 => 18651L;
        public override object SolutionExample2 => throw new NotImplementedException();
        public override object SolutionPuzzle2 => throw new NotImplementedException();

        record class Hailstone(long X, long Y, long Z, long dX, long dY, long dZ)
        {
            public static Hailstone Parse(string line)
            {
                var pv = line.Split('@');
                var p = pv.First().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                var v = pv.Last().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                return new Hailstone(p[0], p[1], p[2], v[0], v[1], v[2]);
            }
        }

        private bool DoIntersectXY(Hailstone h0, Hailstone h1, long begin, long end)
        {
            // f(x) = mx + b
            // m0 * ix + b0 = m1 * ix + b1
            // (m0 * ix) - (m1 - ix) = b1 - b0
            // ix * (m0 - m1) = b1 - b0
            // ix = (b1 - b0) / (m0 - b1)

            double m0 = h0.dY / (double)h0.dX;
            double b0 = h0.Y - m0 * h0.X;
            double m1 = h1.dY / (double)h1.dX;
            double b1 = h1.Y - m1 * h1.X;

            if (m0 == m1) return false;

            var ix = (b1 - b0) / (m0 - m1);
            var iy = m0 * ix + b0;

            if (ix < begin) return false;            
            if (ix > end) return false;
            if (iy < begin) return false;
            if (iy > end) return false;

            if ((ix - h0.X) / h0.dX < 0) return false;
            if ((ix - h1.X) / h1.dX < 0) return false;

            return true;
        }

        protected override object Solve1(string filename)
        {
            long begin = filename.Contains("example") ? 7 : 200000000000000;
            long end = filename.Contains("example") ? 27 : 400000000000000;

            var data = System.IO.File.ReadAllLines(filename).Select(Hailstone.Parse).ToList();

            long count = 0;

            for (int i = 0; i < data.Count; ++i)
            {
                for (int j = i + 1; j < data.Count; ++j)
                {
                    var a = data[i];
                    var b = data[j];

                    count += DoIntersectXY(a, b, begin, end) ? 1 : 0;
                }
            }

            return count;
        }

        protected override object Solve2(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
