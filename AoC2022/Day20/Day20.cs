using System.Diagnostics;
using System.Globalization;

namespace AoC2022
{
    public class Day20 : AoC.DayBase
    {
        class Number
        {
            public long Value { get; init; }

            public Number Next { get; set; }
            public Number Prev { get; set; }

            public override string ToString()
            {
                return $" {Prev.Value}<- {Value} ->{Next.Value}";
            }
        }

        private void Mix(List<Number> numbers, Dictionary<Number, int> numberToIndex, Dictionary<int, Number> indexToNumber)
        {
            Func<int, int> next = (i) => (i + 1) % numbers.Count;
            Func<int, int> prev = (i) => (i + numbers.Count - 1) % numbers.Count;

            foreach (var n in numbers)
            {
                int index = numberToIndex[n];
                long move = Math.Abs(n.Value) % (numbers.Count - 1) * Math.Sign(n.Value);

                if (index + move <= 0)
                {
                    move = move + (numbers.Count - 1);
                }
                else if (index + move >= numbers.Count)
                {
                    move = move - (numbers.Count - 1);
                }

                if (move >= 0)
                {
                    for (int i = 0; i < move; ++i)
                    {
                        var neighbor = indexToNumber[next(index)];

                        numberToIndex[neighbor] = index;
                        numberToIndex[n] = next(index);

                        indexToNumber[index] = neighbor;
                        indexToNumber[next(index)] = n;

                        index = next(index);
                    }
                }
                else
                {
                    for (int i = 0; i < -move; ++i)
                    {
                        var neighbor = indexToNumber[prev(index)];

                        numberToIndex[neighbor] = index;
                        numberToIndex[n] = prev(index);

                        indexToNumber[index] = neighbor;
                        indexToNumber[prev(index)] = n;

                        index = prev(index);
                    }
                }
            }
        }


        protected override object Solve1(string filename)
        {
            var numbers = File.ReadAllLines(filename).Select(int.Parse).Select(n => new Number { Value = n }).ToList();
            var inputWithIndex = numbers.Select((n, i) => new { Number = n, Index = i });
            var numberToIndex = inputWithIndex.ToDictionary(s => s.Number, s => s.Index);
            var indexToNumber = inputWithIndex.ToDictionary(s => s.Index, s => s.Number);

            Mix(numbers, numberToIndex, indexToNumber);

            var p0 = numberToIndex[numbers.Single(n => n.Value == 0)];
            var a = indexToNumber[(p0 + 1000) % numbers.Count];
            var b = indexToNumber[(p0 + 2000) % numbers.Count];
            var c = indexToNumber[(p0 + 3000) % numbers.Count];

            return a.Value + b.Value + c.Value;
        }

        protected override object Solve2(string filename)
        {
            var numbers = File.ReadAllLines(filename).Select(long.Parse).Select(n => new Number { Value = n * 811589153 }).ToList();
            var inputWithIndex = numbers.Select((n, i) => new { Number = n, Index = i });
            var numberToIndex = inputWithIndex.ToDictionary(s => s.Number, s => s.Index);
            var indexToNumber = inputWithIndex.ToDictionary(s => s.Index, s => s.Number);

            for (int i = 0; i < 10; ++i)
            {
                Mix(numbers, numberToIndex, indexToNumber);
            }

            var p0 = numberToIndex[numbers.Single(n => n.Value == 0)];
            var a = indexToNumber[(p0 + 1000) % numbers.Count];
            var b = indexToNumber[(p0 + 2000) % numbers.Count];
            var c = indexToNumber[(p0 + 3000) % numbers.Count];

            return a.Value + b.Value + c.Value;
        }

        public override object SolutionExample1 => 3L;
        public override object SolutionPuzzle1 => 8028L;
        public override object SolutionExample2 => 1623178306L;
        public override object SolutionPuzzle2 => 8798438007673L;
    }
}
