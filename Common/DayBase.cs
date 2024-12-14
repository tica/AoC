using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC
{
    public abstract class DayBase
    {
        private int Day { get; init; }

        protected DayBase(int day = 0)
        {
            if (day != 0)
            {
                Day = day;
            }
            else
            {
                var m = Regex.Match(GetType().Name, @"Day(\d+)");
                Day = int.Parse(m.Groups[1].Value);
            }
        }

        protected abstract object Solve1(string filename);
        protected abstract object Solve2(string filename);

        private string ExampleFile1 => $"Day{Day:00}/example1.txt";
        private string ExampleFile2 => $"Day{Day:00}/example2.txt";
        private string InputFile => $"Day{Day:00}/input.txt";

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



        public static AoC.DayBase CreateLatest(Assembly assembly)
        {
            Type? latest = null;
            int latestDay = 0;

            foreach (var type in assembly.GetTypes())
            {
                var m = Regex.Match(type.Name, @"^Day(\d+)$");
                if (!m.Success)
                    continue;

                int day = int.Parse(m.Groups[1].Value);

                if (day > latestDay)
                {
                    latestDay = day;
                    latest = type;
                }
            }

            if (latest != null)
                return (AoC.DayBase)Activator.CreateInstance(latest)!;

            throw new Exception("Not found");
        }
    }
}
