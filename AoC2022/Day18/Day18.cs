namespace AoC2022
{
    public class Day18 : AoC.DayBase
    {
        private (int x, int y, int z) ParseLine(string line)
        {
            var a = line.Split(',').Select(int.Parse).ToArray();

            return (a[0], a[1], a[2]);
        }

        protected override object Solve1(string filename)
        {
            var input = File.ReadAllLines(filename).Select(ParseLine).ToList();

            var all = new HashSet<(int x, int y, int z)>(input);

            int count = 0;
            foreach( var a in all )
            {
                if (!all.Contains((a.x + 1, a.y, a.z)))
                    count += 1;
                if (!all.Contains((a.x - 1, a.y, a.z)))
                    count += 1;
                if (!all.Contains((a.x, a.y + 1, a.z)))
                    count += 1;
                if (!all.Contains((a.x, a.y - 1, a.z)))
                    count += 1;
                if (!all.Contains((a.x, a.y, a.z + 1)))
                    count += 1;
                if (!all.Contains((a.x, a.y, a.z - 1)))
                    count += 1;
            }

            return count;
        }

        protected override object Solve2(string filename)
        {
            var input = File.ReadAllLines(filename).Select(ParseLine).ToList();

            var all = new HashSet<(int x, int y, int z)>(input);

            var maxX = input.Max(i => i.x);
            var maxY = input.Max(i => i.y);
            var maxZ = input.Max(i => i.z);

            var outside = new HashSet<(int x, int y, int z)>();
            for( int x = 0; x <= maxX; ++x)
            {
                for( int y = 0; y <= maxY; ++y)
                {
                    outside.Add((x, y, -1));
                    outside.Add((x, y, maxZ + 1));
                }
            }
            for (int x = 0; x <= maxX; ++x)
            {
                for (int z = 0; z <= maxZ; ++z)
                {
                    outside.Add((x, -1, z));
                    outside.Add((x, maxY + 1, z));
                }
            }
            for (int y = 0; y <= maxY; ++y)
            {
                for (int z = 0; z <= maxZ; ++z)
                {
                    outside.Add((-1, y, z));
                    outside.Add((maxX + 1, y, z));
                }
            }

            for( int x = 0; x <= maxX; ++x)
            {
                for( int y = 0; y <= maxY; ++y)
                {
                    for (int z = 0; z <= maxZ; ++z)
                    {
                        if (all.Contains((x, y, z)))
                            continue;

                        var visited = new HashSet<(int x, int y, int z)>();
                        var path = Dfs((x, y, z), c => EnumCoordNeighbors(c).Where(cc => !all.Contains(cc)), c => outside.Contains(c), visited);
                        if( path != null )
                        {
                            foreach (var v in visited)
                            {
                                outside.Add(v);
                            }
                        }
                    }
                }
            }

            int count = 0;
            foreach (var a in all)
            {
                if (outside.Contains((a.x + 1, a.y, a.z)))
                    count += 1;
                if (outside.Contains((a.x - 1, a.y, a.z)))
                    count += 1;
                if (outside.Contains((a.x, a.y + 1, a.z)))
                    count += 1;
                if (outside.Contains((a.x, a.y - 1, a.z)))
                    count += 1;
                if (outside.Contains((a.x, a.y, a.z + 1)))
                    count += 1;
                if (outside.Contains((a.x, a.y, a.z - 1)))
                    count += 1;
            }

            return count;
        }

        private IEnumerable<(int, int, int)> EnumCoordNeighbors((int x, int y, int z) p)
        {
            yield return (p.x + 1, p.y, p.z);
            yield return (p.x - 1, p.y, p.z);
            yield return (p.x, p.y + 1, p.z);
            yield return (p.x, p.y - 1, p.z);
            yield return (p.x, p.y, p.z + 1);
            yield return (p.x, p.y, p.z - 1);
        }

        private List<T>? Dfs<T>(T from, Func<T, IEnumerable<T>> enumNeighbors, Func<T, bool> isGoal, HashSet<T> visited)
        {
            if (isGoal(from))
            {
                return new List<T> { from };
            }

            if (visited.Contains(from))
                return null;

            visited.Add(from);

            foreach( var n in enumNeighbors(from) )
            {
                var path = Dfs(n, enumNeighbors, isGoal, visited);
                if( path != null )
                {
                    path.Append(from);
                    return path;
                }
            }

            return null;
        }

        public override object SolutionExample1 => 64;
        public override object SolutionPuzzle1 => 3374;
        public override object SolutionExample2 => 58;
        public override object SolutionPuzzle2 => 2010;
    }
}
