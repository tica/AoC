using Microsoft.Z3;

namespace AoC2023
{
    public class Day24 : AoC.DayBase
    {
        public override object SolutionExample1 => 2L;
        public override object SolutionPuzzle1 => 18651L;
        public override object SolutionExample2 => 47L;
        public override object SolutionPuzzle2 => 546494494317645L;

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

                    var test = DoIntersectXY(a, b, begin, end);

                    count += test ? 1 : 0;
                }
            }

            return count;
        }

        protected override object Solve2(string filename)
        {
            var data = System.IO.File.ReadAllLines(filename)
                .Select(Hailstone.Parse)
                .OrderBy(h => h.X)
                .ToList();

            using (Context ctx = new Context())
            {
                Solver solver = ctx.MkSolver();

                IntExpr x = ctx.MkIntConst("x");
                IntExpr y = ctx.MkIntConst("y");
                IntExpr z = ctx.MkIntConst("z");
                IntExpr dx = ctx.MkIntConst("dx");
                IntExpr dy = ctx.MkIntConst("dy");
                IntExpr dz = ctx.MkIntConst("dz");

                for( int i = 0; i < 5; ++i)
                {
                    IntExpr t = ctx.MkIntConst($"t{i}");
                                        
                    var hx = ctx.MkInt(data[i].X);
                    var hy = ctx.MkInt(data[i].Y);                    
                    var hz = ctx.MkInt(data[i].Z);
                    var dhx = ctx.MkInt(data[i].dX);
                    var dhy = ctx.MkInt(data[i].dY);
                    var dhz = ctx.MkInt(data[i].dZ);

                    solver.Assert(t > 0);
                    solver.Assert(ctx.MkEq(x + t * dx, hx + t * dhx));
                    solver.Assert(ctx.MkEq(y + t * dy, hy + t * dhy));
                    solver.Assert(ctx.MkEq(z + t * dz, hz + t * dhz));
                }

                solver.Check();

                Expr r = solver.Model.Evaluate(ctx.MkAdd(x, y, z));

                return long.Parse(r.ToString());
            }
        }
    }
}
