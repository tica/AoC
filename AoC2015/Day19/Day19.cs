namespace AoC2015
{
    public class Day19 : AoC.DayBase
    {
        (List<(string, string)> Replacements, string Molecule) ParseInput(string filename)
        {
            var input = File.ReadAllLines(filename).ToList();

            var replacements = new List<(string, string)>();

            foreach( var r in input.TakeWhile(s => !string.IsNullOrEmpty(s)) )
            {
                var s = r.Split(" => ");
                replacements.Add((s[0], s[1]));
            }

            var molecule = input.SkipWhile(s => !string.IsNullOrEmpty(s)).Skip(1).Single();

            return (replacements, molecule);
        }

        HashSet<string> BuildPossibleVariants(string molecule, List<(string, string)> replacements)
        {
            var results = new HashSet<string>();

            foreach (var (from, to) in replacements)
            {
                int p = 0;
                while (true)
                {
                    p = molecule.IndexOf(from, p);

                    if (p < 0)
                        break;

                    results.Add(molecule.Substring(0, p) + to + molecule.Substring(p + from.Length));

                    p += from.Length;
                }
            }

            return results;
        }

        protected override object Solve1(string filename)
        {
            var (replacements, molecule) = ParseInput(filename);

            return BuildPossibleVariants(molecule, replacements).Count;
        }

        int BuildMolecule(string target, List<(string, string)> replacements, string current, int currentSteps, Dictionary<string, int> cache)
        {
            if (cache.TryGetValue(current, out var steps))
                return steps;

            if (current.Length < target.Length)
                return int.MaxValue;

            if (current == target)
                return currentSteps;

            var variants = new HashSet<string>();
            int min = int.MaxValue;

            foreach (var (from, to) in replacements)
            {
                int p = 0;
                while (true)
                {
                    p = current.IndexOf(from, p);

                    if (p < 0)
                        break;

                    var variant = current.Substring(0, p) + to + current.Substring(p + from.Length);
                    if( !variants.Contains(variant) )
                    {
                        int m = BuildMolecule(target, replacements, variant, currentSteps + 1, cache);
                        min = Math.Min(min, m);

                        variants.Add(variant);
                    }

                    p += from.Length;
                }
            }

            cache.Add(current, min);

            return min;
        }

        protected override object Solve2(string filename)
        {
            return 0;

            /*
            var (replacements, molecule) = ParseInput(filename);

            replacements = replacements.Select(t => (t.Item2, t.Item1)).OrderByDescending(t => t.Item1.Length).ToList();
         
            return BuildMolecule("e", replacements, molecule, 0, new());
            */
        }

        public override object SolutionExample1 => null!;
        public override object SolutionPuzzle1 => null!;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;
    }
}
