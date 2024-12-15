using System.Net.Mime;
using System.Text.RegularExpressions;

namespace AoC2015
{
    public class Day14 : AoC.DayBase
    {
        record class Reindeer(string Name, int Speed, int SprintDuration, int Rest)
        {
            public static Reindeer Parse(string line)
            {
                var m = Regex.Match(line, @"^(\w+) can fly (\d+) km\/s for (\d+) seconds, but then must rest for (\d+) seconds.$");

                return new Reindeer(
                    m.Groups[1].Value,
                    int.Parse(m.Groups[2].Value),
                    int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value)
                );
            }

            public int Simulate(int time)
            {
                int distance = 0;

                while (time > 0)
                {
                    int t = Math.Min(time, SprintDuration);
                    distance += t * Speed;
                    time -= t;

                    t = Math.Min(time, Rest);
                    time -= t;
                }

                return distance;
            }
        }

        protected override object Solve1(string filename)
        {
            var reindeers = File.ReadAllLines(filename).Select(Reindeer.Parse).ToList();

            int time = filename.Contains("example") ? 1000 : 2503;

            return reindeers.Max(r => r.Simulate(time));
        }

        protected override object Solve2(string filename)
        {
            var reindeers = File.ReadAllLines(filename).Select(Reindeer.Parse).ToList();

            var distance = reindeers.ToDictionary(r => r, r => 0);
            var sprintRemaining = reindeers.ToDictionary(r => r, r => r.SprintDuration);
            var restRemaining = reindeers.ToDictionary(r => r, r => r.Rest);
            var score = reindeers.ToDictionary(r => r, r => 0);

            int time = filename.Contains("example") ? 1000 : 2503;

            for (int t = 0; t < time; ++t)
            {
                foreach (var r in reindeers)
                {
                    if (sprintRemaining[r] > 0)
                    {
                        sprintRemaining[r] -= 1;
                        distance[r] += r.Speed;

                        if (sprintRemaining[r] == 0)
                        {
                            restRemaining[r] = r.Rest;
                        }
                    }
                    else if (restRemaining[r] > 0)
                    {
                        restRemaining[r] -= 1;
                        if (restRemaining[r] == 0)
                        {
                            sprintRemaining[r] = r.SprintDuration;
                        }
                    }
                }

                var maxDistance = distance.Values.Max();

                distance.Where(kvp => kvp.Value == maxDistance).ToList().ForEach(r => { score[r.Key] += 1; });
            }

            return score.Values.Max();
        }

        public override object SolutionExample1 => 1120;
        public override object SolutionPuzzle1 => 2640;
        public override object SolutionExample2 => 689;
        public override object SolutionPuzzle2 => 1102;
    }
}
