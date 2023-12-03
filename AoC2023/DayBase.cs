using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
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
            Console.WriteLine(SolveExample1());
            Console.WriteLine(SolvePuzzle1());
            Console.WriteLine(SolveExample2());
            Console.WriteLine(SolvePuzzle2());
        }

        public abstract object SolutionExample1 { get; }
        public abstract object SolutionPuzzle1 { get; }
        public abstract object SolutionExample2 { get; }
        public abstract object SolutionPuzzle2 { get; }
    }
}
