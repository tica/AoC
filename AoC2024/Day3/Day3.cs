using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day3 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var input = System.IO.File.ReadAllText(filename);

            int sum = 0;

            foreach(Match m in Regex.Matches(input, @"mul\((\d\d?\d?),(\d\d?\d?)\)"))
            {
                int a = int.Parse(m.Groups[1].Value);
                int b = int.Parse(m.Groups[2].Value);

                sum += a * b;
            }

            return sum;
        }

        protected override object Solve2(string filename)
        {
            var input = System.IO.File.ReadAllText(filename);

            int sum = 0;

            bool enable = true;

            foreach (Match m in Regex.Matches(input, @"mul\((\d\d?\d?),(\d\d?\d?)\)|(do\(\))|(don't\(\))"))
            {
                var cmd = m.Groups[0].Value;
                if (cmd.StartsWith("mul(") && enable)
                {
                    int a = int.Parse(m.Groups[1].Value);
                    int b = int.Parse(m.Groups[2].Value);

                    sum += a * b;
                }
                else if (cmd == "do()")
                {
                    enable = true;
                }
                else if (cmd == "don't()")
                {
                    enable = false;
                }
            }

            return sum;
        }

        public override object SolutionExample1 => 161;
        public override object SolutionPuzzle1 => 166630675;
        public override object SolutionExample2 => 48;
        public override object SolutionPuzzle2 => 93465710;
    }
}
