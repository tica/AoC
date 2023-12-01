using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023.Day1
{
    internal class Day1
    {
        private static int Solve1(string filename)
        {
            int sum = 0;

            foreach (var line in System.IO.File.ReadAllLines(filename))
            {
                var first = line.First(char.IsDigit);
                var last = line.Last(char.IsDigit);

                var combined = "" + first + last;
                var num = int.Parse(combined);

                sum += num;
            }

            return sum;
        }

        private static int FirstIndex( string line, Dictionary<string, int> values)
        {
            int minIndex = int.MaxValue;
            int value = -1;

            foreach( var val in values.Keys )
            {
                int index = line.IndexOf(val);
                if (index == -1)
                    continue;

                if( index < minIndex )
                {
                    minIndex = index;
                    value = values[val];
                }
            }

            return value;
        }

        private static string Reverse(string text)
        {
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }

        private static int LastIndex(string line, Dictionary<string, int> values)
        {
            int maxIndex = int.MinValue;
            int value = -1;

            foreach (var val in values.Keys)
            {
                int index = line.LastIndexOf(val);
                if (index == -1)
                    continue;

                if (index > maxIndex)
                {
                    maxIndex = index;
                    value = values[val];
                }
            }

            return value;
        }

        private static int Solve2(string filename)
        {
            int sum = 0;

            var numbers = new Dictionary<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 },
                { "1", 1 },
                { "2", 2 },
                { "3", 3 },
                { "4", 4 },
                { "5", 5 },
                { "6", 6 },
                { "7", 7 },
                { "8", 8 },
                { "9", 9 },
            };

            foreach (var line in System.IO.File.ReadAllLines(filename))
            {
                var first = FirstIndex(line, numbers);
                var last = LastIndex(line, numbers);
                var num = first * 10 + last;
                sum += num;
            }

            return sum;
        }

        public static void Solve()
        {
            int x = Solve2("Day1/input2.txt");
            Console.WriteLine(x);
        }
    }
}
