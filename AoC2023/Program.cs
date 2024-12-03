using System.Text.RegularExpressions;

namespace AoC2023
{
    internal class Program
    {
        static AoC.DayBase CreateLatest()
        {
            var assembly = typeof(Program).Assembly;

            Type? latest = null;
            int latestDay = 0;

            foreach( var type in assembly.GetTypes() )
            {
                var m = Regex.Match(type.Name, @"^Day(\d+)$");
                if (!m.Success)
                    continue;

                int day = int.Parse(m.Groups[1].Value);

                if(day > latestDay)
                {
                    latestDay = day;
                    latest = type;
                }
            }

            if (latest != null)
                return (AoC.DayBase)Activator.CreateInstance(latest)!;

            throw new Exception("Not found");
        }

        static void Main(string[] args)
        {
            new Day21().PrintAllDetail();
        }
    }
}
