using AoC.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day9 : AoC.DayBase
    {
        record struct Node(int X, int Y)
        {
            public static Node Parse(string line)
            {
                var a = line.Split(',').Select(int.Parse).ToArray();
                return new Node(a[0], a[1]);
            }
        }

        private long CalcArea(Node a, Node b)
        {
            return (Math.Abs(a.X - b.X) + 1) * (long)(Math.Abs(a.Y - b.Y) + 1);
        }

        protected override object Solve1(string filename)
        {
            var points = File.ReadAllLines(filename).Select(Node.Parse).ToList();

            return points.Pairwise().Select(t => CalcArea(t.Left, t.Right)).Max();
        }

        private bool LineIntersectsSquare((Node A, Node B) square, (Node A, Node B) line)
        {
            int stop = Math.Min(square.A.Y, square.B.Y);
            int sbottom = Math.Max(square.A.Y, square.B.Y);
            int sleft = Math.Min(square.A.X, square.B.X);
            int sright = Math.Max(square.A.X, square.B.X);

            if( line.A.X == line.B.X )
            {
                if (line.A.X <= sleft)
                    return false;
                if (line.A.X >= sright)
                    return false;

                int top = Math.Min(line.A.Y, line.B.Y);
                if (top >= sbottom)
                    return false;
                int bottom = Math.Max(line.A.Y, line.B.Y);
                if (bottom <= stop)
                    return false;

                return true;
            }
            else if( line.A.Y == line.B.Y )
            {
                if (line.A.Y <= stop)
                    return false;
                if (line.A.Y >= sbottom)
                    return false;

                int left = Math.Min(line.A.X, line.B.X);
                if (left >= sright)
                    return false;
                int right = Math.Max(line.A.X, line.B.X);
                if (right <= sleft)
                    return false;

                return true;
            }
            else
            {
                throw new InvalidOperationException("wtf?");
            }
        }

        protected override object Solve2(string filename)
        {
            var nodes = File.ReadAllLines(filename).Select(Node.Parse).ToList();

            var squares = nodes.Pairwise().Select(n => (Nodes: n, Size: CalcArea(n.Left, n.Right))).OrderByDescending(t => t.Size);

            return squares.First(sq => !nodes.Window2Wrap().Any(line => LineIntersectsSquare(sq.Nodes, line))).Size;
        }

        public override object SolutionExample1 => 50L;
        public override object SolutionPuzzle1 => 4763040296L;
        public override object SolutionExample2 => 24L;
        public override object SolutionPuzzle2 => 1396494456L;
    }
}
