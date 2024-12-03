using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2023
{
    public class Day6 : AoC.DayBase
    {
        public override object SolutionExample1 => 288;
        public override object SolutionPuzzle1 => 800280;
        public override object SolutionExample2 => 71503L;
        public override object SolutionPuzzle2 => 45128024L;

        protected override object Solve1(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename).ToList();

            var times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
            var distances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();

            int res = 1;

            for( int i = 0; i < times.Count; ++i)
            {
                int num = 0;

                for( int pressTime = 0; pressTime <= times[i]; ++pressTime)
                {
                    int remainingTime = times[i] - pressTime;

                    int distance = pressTime * remainingTime;

                    if( distance > distances[i])
                    {
                        num += 1;
                    }
                }

                res *= num;
            }

            return res;
        }

        protected override object Solve2(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename).ToList();

            var time = int.Parse(String.Concat(lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));
            var raceDistance = long.Parse(String.Concat(lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));

            long num = 0;

            for (int pressTime = 0; pressTime <= time; ++pressTime)
            {
                long remainingTime = time - pressTime;

                long distance = pressTime * remainingTime;

                if (distance > raceDistance)
                {
                    num += 1;
                }
            }
                       

            return num;
        }
    }
}
