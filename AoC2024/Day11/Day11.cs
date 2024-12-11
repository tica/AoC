using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day11 : AoC.DayBase
    {
        private IEnumerable<BigInteger> ProcessStep(BigInteger n)
        {
            if (n == 0)
            {
                yield return 1;
            }
            else
            {
                var s = n.ToString();
                if (s.Length % 2 == 0)
                {
                    yield return BigInteger.Parse(s.Substring(0, s.Length / 2));
                    yield return BigInteger.Parse(s.Substring(s.Length / 2));
                }
                else
                {
                    yield return checked(n * 2024);
                }
            }
        }

        private long CountResultingNumbers(BigInteger num, int steps, Dictionary<(BigInteger n, int s), long> cache)
        {
            if (steps == 0)
                return 1;

            if (cache.TryGetValue((num, steps), out long cached))
                return cached;

            long count = 0;

            foreach (var r in ProcessStep(num))
            {
                count += CountResultingNumbers(r, steps - 1, cache);
            }

            cache.Add((num, steps), count);

            return count;
        }

        protected override object Solve1(string filename)
        {
            var input = File.ReadAllText(filename).Split(' ').Select(BigInteger.Parse);

            return input.Sum(n => CountResultingNumbers(n, 25, new()));
        }

        protected override object Solve2(string filename)
        {
            var input = File.ReadAllText(filename).Split(' ').Select(BigInteger.Parse);

            return input.Sum(n => CountResultingNumbers(n, 75, new()));
        }

        public override object SolutionExample1 => 55312L;
        public override object SolutionPuzzle1 => 194482L;
        public override object SolutionExample2 => 65601038650482L;
        public override object SolutionPuzzle2 => 232454623677743L;
    }
}
