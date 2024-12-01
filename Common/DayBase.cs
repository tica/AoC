using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC
{
    public abstract class DayBase
    {
        private int Day { get; init; }

        protected DayBase(int day)
        {
            Day = day;
        }

        protected abstract object Solve1(string filename);
        protected abstract object Solve2(string filename);

        private string ExampleFile1 => $"Day{Day}/example1.txt";
        private string ExampleFile2 => $"Day{Day}/example2.txt";
        private string InputFile => $"Day{Day}/input.txt";

        public object SolveExample1()
        {
            return Solve1(ExampleFile1);
        }
        public object SolvePuzzle1()
        {
            return Solve1(InputFile);
        }
        public object SolveExample2()
        {
            if (System.IO.File.Exists(ExampleFile2))
                return Solve2(ExampleFile2);

            return Solve2(ExampleFile1);
        }
        public object SolvePuzzle2()
        {
            return Solve2(InputFile);
        }

        public void PrintAll()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine(SolveExample1());
            Console.WriteLine(SolvePuzzle1());
            Console.WriteLine(SolveExample2());
            Console.WriteLine(SolvePuzzle2());

            sw.Stop();
            Console.WriteLine();
            Console.WriteLine($"Total run time: {sw.ElapsedMilliseconds} ms");
        }

        private void PrintResult(Func<object> fn)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var res = fn();
            sw.Stop();
            Console.WriteLine($"{res}\t({sw.ElapsedMilliseconds} ms)");
        }

        public void PrintAllDetail()
        {
            PrintResult(SolveExample1);
            PrintResult(SolvePuzzle1);
            PrintResult(SolveExample2);
            PrintResult(SolvePuzzle2);
        }

        public abstract object SolutionExample1 { get; }
        public abstract object SolutionPuzzle1 { get; }
        public abstract object SolutionExample2 { get; }
        public abstract object SolutionPuzzle2 { get; }
    }
}
