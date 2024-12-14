using System.Text.RegularExpressions;

namespace AoC2015
{
    public class Day9 : AoC.DayBase
    {
        (string, string, int) ParseLine(string line)
        {
            var m = Regex.Match(line, @"(\w+) to (\w+) = (\d+)");
            return (m.Groups[1].Value, m.Groups[2].Value, int.Parse(m.Groups[3].Value));
        }

        Dictionary<string, Dictionary<string, int>> ParseInput(string filename)
        {
            var routes = new Dictionary<string, Dictionary<string, int>>();

            foreach (var (a, b, distance) in File.ReadAllLines(filename).Select(ParseLine))
            {
                if (routes.ContainsKey(a))
                {
                    routes[a][b] = distance;
                }
                else
                {
                    routes.Add(a, new Dictionary<string, int> { [b] = distance });
                }
                if (routes.ContainsKey(b))
                {
                    routes[b][a] = distance;
                }
                else
                {
                    routes.Add(b, new Dictionary<string, int> { [a] = distance });
                }
            }

            return routes;
        }

        protected override object Solve1(string filename)
        {
            var routes = ParseInput(filename);

            var queue = new Queue<(List<string> path, int length)>(routes.Keys.Select(k => (new List<string>([k]), 0)));

            int shortestLength = int.MaxValue;

            while(queue.TryDequeue(out var state))
            {
                if (state.length > shortestLength)
                    continue;

                if (state.path.Count == routes.Keys.Count && state.length < shortestLength)
                {
                    shortestLength = state.length;
                    continue;
                }

                foreach( var (dest, dist) in routes[state.path.Last()])
                {
                    if (state.path.Contains(dest))
                        continue;

                    queue.Enqueue((state.path.Append(dest).ToList(), state.length + dist));
                }
            }

            return shortestLength;
        }

        protected override object Solve2(string filename)
        {
            var routes = ParseInput(filename);

            var queue = new Queue<(List<string> path, int length)>(routes.Keys.Select(k => (new List<string>([k]), 0)));

            int longestLength = int.MinValue;

            while (queue.TryDequeue(out var state))
            {
                if (state.path.Count == routes.Keys.Count && state.length > longestLength)
                {
                    longestLength = state.length;
                    continue;
                }

                foreach (var (dest, dist) in routes[state.path.Last()])
                {
                    if (state.path.Contains(dest))
                        continue;

                    queue.Enqueue((state.path.Append(dest).ToList(), state.length + dist));
                }
            }

            return longestLength;
        }

        public override object SolutionExample1 => 605;
        public override object SolutionPuzzle1 => 251;
        public override object SolutionExample2 => 982;
        public override object SolutionPuzzle2 => 898;
    }
}
