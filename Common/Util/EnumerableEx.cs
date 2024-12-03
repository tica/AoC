
namespace AoC.Util
{
    public static class EnumerableEx
    {
        public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> source)
        {
            var prev = source.First();

            foreach (var next in source.Skip(1))
            {
                yield return (prev, next);

                prev = next;
            }
        }
    }
}
