using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day19 : AoC.DayBase
    {
        (List<string> Fragments, List<String> Patterns) ParseInput(string filename)
        {
            var fragments = File.ReadAllLines(filename).First().Split(',', StringSplitOptions.TrimEntries).ToList();
            var patterns = File.ReadAllLines(filename).Skip(2).ToList();

            return (fragments, patterns);
        }

        bool CanBuildPattern(Dictionary<char, List<string>> dict, string pattern, HashSet<string> impossible)
        {
            if (pattern.Length == 0)
                return true;

            if (impossible.Contains(pattern))
                return false;

            if( dict.TryGetValue(pattern[0], out var fragments) )
            {
                foreach (var frag in fragments.Where(f => pattern.StartsWith(f)))
                {
                    if (CanBuildPattern(dict, pattern.Substring(frag.Length), impossible))
                        return true;
                }
            }

            impossible.Add(pattern);

            return false;
        }

        protected override object Solve1(string filename)
        {
            var (fragments, patterns) = ParseInput(filename);
            var dict = fragments.GroupBy(f => f[0]).ToDictionary(g => g.Key, g => g.ToList());

            var impossible = new HashSet<string>();
            return patterns.Count(p => CanBuildPattern(dict, p, impossible));
        }

        long CountVariants(Dictionary<char, List<string>> dict, string pattern, Dictionary<string, long> cache)
        {
            if (pattern.Length == 0)
                return 1;

            if (cache.TryGetValue(pattern, out var count))
                return count;

            long sum = 0;

            if (dict.TryGetValue(pattern[0], out var fragments))
            {
                foreach (var frag in fragments.Where(f => pattern.StartsWith(f)))
                {
                    sum += CountVariants(dict, pattern.Substring(frag.Length), cache);
                }
            }

            cache.Add(pattern, sum);

            return sum;
        }

        protected override object Solve2(string filename)
        {
            var (fragments, patterns) = ParseInput(filename);
            var dict = fragments.GroupBy(f => f[0]).ToDictionary(g => g.Key, g => g.ToList());

            var cache = new Dictionary<string, long>();
            return patterns.Sum(p => CountVariants(dict, p, cache));
        }

        public override object SolutionExample1 => 6;
        public override object SolutionPuzzle1 => 300;
        public override object SolutionExample2 => 16L;
        public override object SolutionPuzzle2 => 624802218898092L;
    }
}
