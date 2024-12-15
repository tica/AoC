using AoC.Util;

namespace AoC2015
{
    public class Day3 : AoC.DayBase
    {
        (int x, int y) Move((int x, int y) p, Direction dir)
        {
            return dir switch
            {
                Direction.Left => (p.x - 1, p.y),
                Direction.Right => (p.x + 1, p.y),
                Direction.Up => (p.x, p.y - 1),
                Direction.Down => (p.x, p.y + 1),
                _ => throw new NotSupportedException()
            };
        }

        protected override object Solve1(string filename)
        {
            var input = File.ReadAllText(filename).Select(DirectionHelper.Parse);

            var p = (0, 0);
            var visited = new HashSet<(int, int)>([p]);

            foreach ( var dir in input )
            {
                p = Move(p, dir);
                visited.Add(p);
            }

            return visited.Count;
        }

        protected override object Solve2(string filename)
        {
            var input = File.ReadAllText(filename).Select(DirectionHelper.Parse);

            var p = (0, 0);
            var q = p;
            var visited = new HashSet<(int, int)>([p]);

            foreach (var dirs in input.Chunk(2))
            {
                p = Move(p, dirs.First());
                visited.Add(p);
                q = Move(q, dirs.Last());
                visited.Add(q);
            }

            return visited.Count;
        }

        public override object SolutionExample1 => 4;
        public override object SolutionPuzzle1 => 2592;
        public override object SolutionExample2 => 3;
        public override object SolutionPuzzle2 => 2360;
    }
}
