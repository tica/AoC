using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tests
{
    public abstract class TestDayBase
    {
        private static Type Day1Type = typeof(AoC2023.Day1);

        private AoC.DayBase? Day;

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

                var typeName = $"{Day1Type.Namespace}.Day{number}";
                Day = (AoC.DayBase?)Day1Type.Assembly.CreateInstance(typeName);
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
    [TestClass] public class Day01 : TestDayBase { }
    [TestClass] public class Day02 : TestDayBase { }
    [TestClass] public class Day03 : TestDayBase { }
    [TestClass] public class Day04 : TestDayBase { }
    [TestClass] public class Day05 : TestDayBase { }
    [TestClass] public class Day06 : TestDayBase { }
    [TestClass] public class Day07 : TestDayBase { }
    [TestClass] public class Day08 : TestDayBase { }
    [TestClass] public class Day09 : TestDayBase { }
    [TestClass] public class Day10 : TestDayBase { }
    [TestClass] public class Day11 : TestDayBase { }
    [TestClass] public class Day12 : TestDayBase { }
    [TestClass] public class Day13 : TestDayBase { }
    [TestClass] public class Day14 : TestDayBase { }
    [TestClass] public class Day15 : TestDayBase { }
    [TestClass] public class Day16 : TestDayBase { }
    [TestClass] public class Day17 : TestDayBase { }
    [TestClass] public class Day18 : TestDayBase { }
    [TestClass] public class Day19 : TestDayBase { }
    [TestClass] public class Day20 : TestDayBase { }
    [TestClass] public class Day21 : TestDayBase { }
    [TestClass] public class Day22 : TestDayBase { }
    [TestClass] public class Day23 : TestDayBase { }
    [TestClass] public class Day24 : TestDayBase { }
    [TestClass] public class Day25 : TestDayBase { }
}