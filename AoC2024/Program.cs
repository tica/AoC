// See https://aka.ms/new-console-template for more information
using AoC2024;
using System.Text.RegularExpressions;


static AoC.DayBase CreateLatest()
{
    var assembly = typeof(Program).Assembly;

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

CreateLatest().PrintAllDetail();