using AoC.Util;

namespace AoC2025
{
    public class Day8 : AoC.DayBase
    {
        record struct Node(int X, int Y, int Z)
        {
            public static Node Parse(string line)
            {
                var a = line.Split(',').Select(int.Parse).ToArray();
                return new Node(a[0], a[1], a[2]);
            }
        }

        private double CalcDistance(Node a, Node b)
        {
            return Math.Sqrt(MathFunc.Sqr(a.X - b.X) + MathFunc.Sqr(a.Y - b.Y) + MathFunc.Sqr(a.Z - b.Z));
        }

        protected override object Solve1(string filename)
        {
            var all = File.ReadAllLines(filename).Select(Node.Parse).ToList();

            var lookupGroup = Enumerable.Range(0, all.Count).ToDictionary(n => all[n], n => n);

            var distances = all.Pairwise()
                .Select(p => (Nodes: p, Distance: CalcDistance(p.Left, p.Right)))
                .OrderBy(t => t.Distance)
                .Select(t => t.Nodes)
                .ToList();

            foreach (var i in distances.Take(filename.Contains("example") ? 10 : 1000))
            {
                var groupLeft = lookupGroup[i.Left];
                var groupRight = lookupGroup[i.Right];

                if (groupLeft == groupRight)
                    continue;

                foreach (var x in all)
                {
                    if (lookupGroup[x] == groupRight)
                        lookupGroup[x] = groupLeft;
                }
            }

            var counter = new CountingSet<int>();

            all.ForEach(x => counter.Add(lookupGroup[x], 1));

            return counter.Values.OrderByDescending(x => x).Take(3).Product();
        }

        protected override object Solve2(string filename)
        {
            var all = File.ReadAllLines(filename).Select(Node.Parse).ToList();

            var lookupGroup = Enumerable.Range(0, all.Count).ToDictionary(n => all[n], n => n);

            var distances = all.Pairwise()
                .Select(p => (Nodes: p, Distance: CalcDistance(p.Item1, p.Item2)))
                .OrderBy(t => t.Distance)
                .Select(t => t.Nodes)
                .ToList();

            int groupsLeft = all.Count;

            foreach (var i in distances)
            {
                var groupLeft = lookupGroup[i.Left];
                var groupRight = lookupGroup[i.Right];

                if (groupLeft == groupRight)
                    continue;

                foreach (var x in all)
                {
                    if (lookupGroup[x] == groupRight)
                        lookupGroup[x] = groupLeft;
                }

                if (--groupsLeft == 1)
                {
                    return i.Left.X * i.Right.X;
                }
            }

            throw new InvalidOperationException("oops?!");
        }

        public override object SolutionExample1 => 40L;
        public override object SolutionPuzzle1 => 131150L;
        public override object SolutionExample2 => 25272;
        public override object SolutionPuzzle2 => 2497445;
    }
}
