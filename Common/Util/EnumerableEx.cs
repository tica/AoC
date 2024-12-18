
namespace AoC.Util
{
    public static class EnumerableEx
    {
        public static IEnumerable<(T Left, T Right)> Window2<T>(this IEnumerable<T> source)
        {
            var prev = source.First();

            foreach (var next in source.Skip(1))
            {
                yield return (prev, next);

                prev = next;
            }
        }

        public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> source)
        {
            foreach( var (a, i) in source.Select((a, i) => (a, i)) )
            {
                foreach (var b in source.Skip(i + 1))
                {
                    yield return (a, b);
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
        {
            if (length == 1)
                return list.Select(t => new[] { t });

            return GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat([t2]));
        }
    }
}
