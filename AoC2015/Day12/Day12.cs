using System.Text.Json;
using System.Text.Json.Nodes;

namespace AoC2015
{
    public class Day12 : AoC.DayBase
    {
        private int SumJson(JsonElement elem, bool ignoreRed = false)
        {
            switch( elem.ValueKind )
            {
                case JsonValueKind.Object:
                    if (ignoreRed && elem.EnumerateObject().Any(o => o.Value.ToString() == "red"))
                        return 0;
                    return elem.EnumerateObject().Sum(o => SumJson(o.Value, ignoreRed));
                case JsonValueKind.Array:
                    return elem.EnumerateArray().Sum(a => SumJson(a, ignoreRed));
                case JsonValueKind.Number:
                    return elem.GetInt32();
                default:
                    return 0;
            }
        }

        protected override object Solve1(string filename)
        {
            var text = File.ReadAllText(filename);
            var doc = JsonDocument.Parse(text);

            return SumJson(doc.RootElement);
        }

        protected override object Solve2(string filename)
        {
            var text = File.ReadAllText(filename);
            var doc = JsonDocument.Parse(text);

            return SumJson(doc.RootElement, true);
        }

        public override object SolutionExample1 => null!;
        public override object SolutionPuzzle1 => null!;
        public override object SolutionExample2 => null!;
        public override object SolutionPuzzle2 => null!;
    }
}
