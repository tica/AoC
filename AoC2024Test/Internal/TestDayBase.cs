using AoC2024;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2024Test.Internal
{
    public abstract class TestDayBase
    {
        private AoC.DayBase? Day;

        private static System.Reflection.Assembly AoCAssembly = typeof(AoC2024.Day1).Assembly;

        protected TestDayBase(AoC.DayBase? day = null)
        {
            if (day != null)
            {
                Day = day;
            }
            else
            {
                var m = Regex.Match(GetType().Name, @"Day(\d+)");
                var number = int.Parse(m.Groups[1].Value);

                Day = (AoC.DayBase?)AoCAssembly.CreateInstance($"AoC2024.Day{number}");
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if (Day == null)
                Assert.Inconclusive("Day implementation not loaded");
        }

        [TestMethod]
        public void Example1()
        {
            Assert.AreEqual(Day!.SolutionExample1, Day.SolveExample1());
        }
        [TestMethod]
        public void Puzzle1()
        {
            Assert.AreEqual(Day!.SolutionPuzzle1, Day.SolvePuzzle1());
        }
        [TestMethod]
        public void Example2()
        {
            Assert.AreEqual(Day!.SolutionExample2, Day.SolveExample2());
        }
        [TestMethod]
        public void Puzzle2()
        {
            Assert.AreEqual(Day!.SolutionPuzzle2, Day.SolvePuzzle2());
        }
    }
}
