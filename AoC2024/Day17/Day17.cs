using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024
{
    public class Day17 : AoC.DayBase
    {
        record class Input(long A, long B, long C, List<int> Program)
        {
            public static Input Parse(string filename)
            {
                var lines = File.ReadAllLines(filename);

                var a = long.Parse(lines[0].Split(' ')[2]);
                var b = long.Parse(lines[1].Split(' ')[2]);
                var c = long.Parse(lines[2].Split(' ')[2]);

                var p = lines[4].Split(' ')[1].Split(',').Select(int.Parse).ToList();

                return new Input(a, b, c, p);
            }
        }



        protected override object Solve1(string filename)
        {
            var input = Input.Parse(filename);

            long a = input.A;
            long b = input.B;
            long c = input.C;

            Func<int, long> SelectCombo = (int operand) =>
            {
                if (operand <= 3)
                {
                    return operand;
                }
                switch( operand )
                {
                    case 4: return a;
                    case 5: return b;
                    case 6: return c;
                }

                throw new InvalidOperationException();
            };

            int ip = 0;

            var output = new List<long>();

            while( ip < input.Program.Count )
            {
                int opcode = input.Program[ip];
                int operand = input.Program[ip + 1];

                switch (opcode)
                {
                    case 0: // adv
                        long divisor = (long)Math.Pow(2, SelectCombo(operand));
                        a /= divisor;
                        break;
                    case 1: // bxl
                        b ^= operand;
                        break;
                    case 2: // bst
                        b = SelectCombo(operand) % 8;
                        break;
                    case 3: // jnz
                        if (a != 0)
                        {
                            ip = operand;
                            continue;
                        }
                        break;
                    case 4: // bxc
                        b ^= c;
                        break;
                    case 5: // out
                        var val = SelectCombo(operand) % 8;
                        output.Add(val);
                        break;
                    case 6: // bdv
                        b = a / (long)Math.Pow(2, SelectCombo(operand));
                        break;
                    case 7: // cdv
                        c  = a / (long)Math.Pow(2, SelectCombo(operand));
                        break;
                    default:
                        throw new NotImplementedException();
                }

                ip += 2;
            }

            return string.Join(',', output);
        }

        private List<int> Simulate(long initialA)
        {
            List<int> result = new List<int>();

            long a = initialA;
            long b = 0;
            long c = 0;
            while (a > 0)
            {
                // This is what the example program does
                b = a % 8;
                b ^= 2;
                c = (a / (1 << (int)b)) % 8;
                b ^= c;
                b ^= 7;

                result.Add((int)(b % 8));

                a /= 8;
            }

            return result;
        }

        protected override object Solve2(string filename)
        {
            if (filename.Contains("example"))
                return null!;

            var input = Input.Parse(filename);
            var expected = input.Program;

            var test = "1000000000000000";

            for ( int i = 0; i < test.Length; ++i)
            {
                while (true)
                {
                    long a = Convert.ToInt64(test, 8);
                    var result = Simulate(a);
                    Assert.AreEqual(expected.Count, result.Count);

                    if (result[result.Count - 1 - i] == expected[result.Count - 1 - i])
                    {
                        break;
                    }

                    test = test.Substring(0, i) + (char)(test[i] + 1) + test.Substring(i + 1);
                }
            }

            return Convert.ToInt64(test, 8);

            /*
            var input = Input.Parse(filename);
            long a = Convert.ToInt64("5322350134036017", 8);
            long b = 0;
            long c = 0;
            while (a > 0)
            {
                b = a % 8;
                b ^= 2;
                c = (a / (1 << (int)b)) % 8;
                b ^= c;
                b ^= 7;

                Console.Write($"{b%8},");

                a /= 8;
            }

            Console.WriteLine();
            Console.WriteLine();
            var ps = string.Join(',', input.Program);
            Console.WriteLine($"{ps}");
            */
        }

        public override object SolutionExample1 => "4,6,3,5,6,3,5,2,1,0";
        public override object SolutionPuzzle1 => "7,1,3,7,5,1,0,3,4";
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => 190384113204239L;
    }
}
