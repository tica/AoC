using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Common.Test
{
    public abstract class TestDayBase
    {
        private AoC.DayBase? Day;

        protected TestDayBase(Type day1Type)
        {
            var m = Regex.Match(GetType().Name, @"Day(\d+)");
            var number = int.Parse(m.Groups[1].Value);

            var typeName = $"{day1Type.Namespace}.Day{number}";
            Day = (AoC.DayBase?)day1Type.Assembly.CreateInstance(typeName);
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
