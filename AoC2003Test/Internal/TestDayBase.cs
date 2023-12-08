using AoC2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2003Test.Internal
{
    public abstract class TestDayBase
    {
        private DayBase Day;
        protected TestDayBase(DayBase day)
        {
            Day = day;
        }

        [TestMethod]
        public void Example1()
        {
            Assert.AreEqual(Day.SolutionExample1, Day.SolveExample1());
        }
        [TestMethod]
        public void Puzzle1()
        {
            var day = new AoC2023.Day1();

            Assert.AreEqual(Day.SolutionPuzzle1, Day.SolvePuzzle1());
        }
        [TestMethod]
        public void Example2()
        {
            var day = new AoC2023.Day1();

            Assert.AreEqual(Day.SolutionExample2, Day.SolveExample2());
        }
        [TestMethod]
        public void Puzzle2()
        {
            var day = new AoC2023.Day1();

            Assert.AreEqual(Day.SolutionPuzzle2, Day.SolvePuzzle2());
        }
    }
}
