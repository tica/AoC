using System.Text.RegularExpressions;

namespace AoC2015
{
    public class Day15 : AoC.DayBase
    {
        record class Ingredient(string Name, int Capacity, int Durability, int Flavor, int Texture, int Calories)
        {
            public static Ingredient Parse(string line)
            {
                var m = Regex.Match(line, @"^(\w+): capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)$");

                return new Ingredient(
                    m.Groups[1].Value,
                    int.Parse(m.Groups[2].Value),
                    int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value),
                    int.Parse(m.Groups[5].Value),
                    int.Parse(m.Groups[6].Value)
                );
            }
        }

        int CalculateMaxScore(List<Ingredient> ingredients, List<int> amounts, int amountRemaining, int? fixedCalories)
        {
            if( amounts.Count == ingredients.Count )
            {
                int capacity = 0;
                int durability = 0;
                int flavor = 0;
                int texture = 0;
                int calories = 0;
                for ( int i = 0; i < amounts.Count; ++i )
                {
                    capacity += ingredients[i].Capacity * amounts[i];
                    durability += ingredients[i].Durability * amounts[i];
                    flavor += ingredients[i].Flavor* amounts[i];
                    texture += ingredients[i].Texture * amounts[i];
                    calories += ingredients[i].Calories * amounts[i];
                }
                if (capacity < 0) capacity = 0;
                if (durability < 0) durability = 0;
                if (flavor < 0) flavor = 0;
                if (texture < 0) texture = 0;

                if (fixedCalories.HasValue && calories != fixedCalories.Value)
                    return 0;

                return capacity * durability * flavor * texture;
            }
            else if( amounts.Count == ingredients.Count-1)
            {
                return CalculateMaxScore(ingredients, amounts.Append(amountRemaining).ToList(), 0, fixedCalories);
            }

            int max = 0;

            for( int a = 0; a < amountRemaining; ++a)
            {
                int m = CalculateMaxScore(ingredients, amounts.Append(a).ToList(), amountRemaining - a, fixedCalories);
                max = Math.Max(max, m);
            }

            return max;
        }

        protected override object Solve1(string filename)
        {
            var ingredients = File.ReadAllLines(filename).Select(Ingredient.Parse).ToList();

            return CalculateMaxScore(ingredients, new(), 100, null);
        }

        protected override object Solve2(string filename)
        {
            var ingredients = File.ReadAllLines(filename).Select(Ingredient.Parse).ToList();

            return CalculateMaxScore(ingredients, new(), 100, 500);
        }

        public override object SolutionExample1 => 62842880;
        public override object SolutionPuzzle1 => 21367368;
        public override object SolutionExample2 => 57600000;
        public override object SolutionPuzzle2 => 1766400;
    }
}
