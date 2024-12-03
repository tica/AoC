namespace AoC2023
{
    public class Day25 : AoC.DayBase
    {
        public override object SolutionExample1 => 54;
        public override object SolutionPuzzle1 => 514794;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;

        protected override object Solve1(string filename)
        {
            Console.WriteLine(filename);

            var graph = new Dictionary<string, List<string>>();

            foreach ( var line in System.IO.File.ReadAllLines(filename))
            {
                var split = line.Split(':');
                var left = split.First().Trim();
                var right = split.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

                foreach( var r in right)
                {
                    if( graph.ContainsKey(r) )
                    {
                        graph[r].Add(left);
                    }
                    else
                    {
                        graph[r] = new() { left };
                    }
                    if (graph.ContainsKey(left))
                    {
                        graph[left].Add(r);
                    }
                    else
                    {
                        graph[left] = new() { r };
                    }
                }
            }

            var cuts = new List<(string, string)>();

            if ( filename.Contains("example")) 
            {
                cuts.Add(("hfx", "pzl"));
                cuts.Add(("bvb", "cmg"));
                cuts.Add(("nvd", "jqt"));
            }
            else
            {
                // Determined via neato
                cuts.Add(("hqq", "xxq"));
                cuts.Add(("vkd", "qfb"));
                cuts.Add(("xzz", "kgl"));
            }

            foreach( var (a,b) in cuts )
            {
                graph[a].Remove(b);
                graph[b].Remove(a);
            }

            var group0 = new HashSet<string>();
            var open = new Queue<string>();
            open.Enqueue(graph.First().Key);
            
            while( open.Count > 0)
            {
                var item = open.Dequeue();
                group0.Add(item);

                foreach( var n in graph[item] )
                {
                    if( !group0.Contains(n) )
                    {
                        open.Enqueue(n);
                    }
                }
            }

            return group0.Count * (graph.Count - group0.Count);
        }

        protected override object Solve2(string filename)
        {
            return null!;
        }
    }
}
