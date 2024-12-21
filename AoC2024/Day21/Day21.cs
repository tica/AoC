using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;
using System.Text;

namespace AoC2024
{
    public class Day21 : AoC.DayBase
    {
        IEnumerable<string> EnumKeySequences(Coord from, Coord to)
        {
            var dirs = new List<Direction>();

            int dx = to.X - from.X;
            if (dx > 0)
                dirs.AddRange(Enumerable.Repeat(Direction.Right, dx));
            else if( dx < 0 )
                dirs.AddRange(Enumerable.Repeat(Direction.Left, -dx));
            int dy = to.Y - from.Y;
            if (dy > 0)
                dirs.AddRange(Enumerable.Repeat(Direction.Down, dy));
            else if (dy < 0)
                dirs.AddRange(Enumerable.Repeat(Direction.Up, -dy));

            foreach ( var perm in dirs.Permute())
            {
                var p = from;
                bool valid = true;
                foreach (var d in perm)
                {
                    p = p.Neighbor(d);
                    valid &= (p.Value != ' ');
                }

                if( valid )
                {
                    var seq = new string(perm.Select(d => d.ToChar()).ToArray());
                    yield return seq + 'A';
                }
            }
        }

        IEnumerable<string> BuildKeySequences(Grid pad, string output)
        {
            var pos = pad.SingleValue('A');

            return BuildKeySequences(pad, pos, "", output);
        }

        IEnumerable<string> BuildKeySequences(Grid pad, Coord pos, string seq, string output)
        {
            if (output.Length == 0)
            {
                yield return seq;
                yield break;
            }

            var next = pad.SingleValue(output.First());

            var variants = EnumKeySequences(pos, next).Distinct().ToList();
            foreach (var variant in variants)
            {
                foreach (var ks in BuildKeySequences(pad, next, seq + variant, output.Substring(1)))
                {
                    yield return ks;
                }
            }
        }

        protected override object Solve1(string filename)
        {
            var numpad = GridHelper.Load(Path.Combine(Path.GetDirectoryName(filename)!, "numpad.txt"));
            var dirpad = GridHelper.Load(Path.Combine(Path.GetDirectoryName(filename)!, "dirpad.txt"));

            int sum = 0;

            foreach (var output in File.ReadAllLines(filename))
            {
                Console.WriteLine(output);
                var seq1 = BuildKeySequences(numpad, output).ToList();
                var minLength1 = seq1.Min(s => s.Length);
                var shortest1 = seq1.Where(s => s.Length == minLength1).ToList();

                var seq2 = shortest1.SelectMany(s => BuildKeySequences(dirpad, s)).ToList();
                var minLength2 = seq2.Min(s => s.Length);
                var shortest2 = seq2.Where(s => s.Length == minLength2).ToList();

                var seq3 = shortest2.SelectMany(s => BuildKeySequences(dirpad, s)).ToList();
                var minLength3 = seq3.Min(s => s.Length);
                //var shortest3 = seq3.Where(s => s.Length == minLength3).ToList();

                int numericPart = int.Parse(output.Substring(0, 3));
                sum += numericPart * minLength3;
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            return null!;
        }

        public override object SolutionExample1 => 126384;
        public override object SolutionPuzzle1 => 125742;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;
    }
}
