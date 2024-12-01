using AoC2024;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2024Test.Internal
{
    public abstract class TestDayBase
    {
        private AoC.DayBase Day;
        protected TestDayBase(AoC.DayBase day)
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
            Assert.AreEqual(Day.SolutionPuzzle1, Day.SolvePuzzle1());
        }
        [TestMethod]
        public void Example2()
        {
            Assert.AreEqual(Day.SolutionExample2, Day.SolveExample2());
        }
        [TestMethod]
        public void Puzzle2()
        {
            Assert.AreEqual(Day.SolutionPuzzle2, Day.SolvePuzzle2());
        }
    }
}
