
using System.Numerics;

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

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this List<T> list)
        {
            return GetPermutations(list, list.Count);
        }

        // https://stackoverflow.com/a/43033661
        public static IEnumerable<List<T>> Permute<T>(this List<T> items)
        {
            List<T> ApplyTransform(List<T> values, (int First, int Second)[] tx)
            {
                var permutation = new List<T>(values.Count);
                for (var i = 0; i < tx.Length; i++)
                    permutation.Add(values[tx[i].Second]);
                return permutation;
            }

            void Swap<U>(ref U x, ref U y)
            {
                var tmp = x;
                x = y;
                y = tmp;
            }

            var length = items.Count;

            // Build identity transform
            var transform = new (int First, int Second)[length];
            for (var i = 0; i < length; i++)
                transform[i] = (i, i);

            yield return ApplyTransform(items, transform);

            while (true)
            {
                // Ref: E. W. Dijkstra, A Discipline of Programming, Prentice-Hall, 1997
                // Find the largest partition from the back that is in decreasing (non-increasing) order
                var decreasingpart = length - 2;
                while (decreasingpart >= 0 && transform[decreasingpart].First >= transform[decreasingpart + 1].First)
                    --decreasingpart;

                // The whole sequence is in decreasing order, finished
                if (decreasingpart < 0)
                    yield break;

                // Find the smallest element in the decreasing partition that is
                // greater than (or equal to) the item in front of the decreasing partition
                var greater = length - 1;
                while (greater > decreasingpart && transform[decreasingpart].First >= transform[greater].First)
                    greater--;

                // Swap the two
                Swap(ref transform[decreasingpart], ref transform[greater]);

                // Reverse the decreasing partition
                Array.Reverse(transform, decreasingpart + 1, length - decreasingpart - 1);

                yield return ApplyTransform(items, transform);
            }
        }

        public static IEnumerable<T> GenerateConsecutive<T>(int count, T initial, Func<T, T> generateNext)
        {
            var accu = initial;
            yield return accu;
            for ( int i = 0; i < count; ++i)
            {
                accu = generateNext(accu);
                yield return accu;
            }
        }

        public static (T, int) MaxIndex<T>(this IEnumerable<T> items) where T: IMinMaxValue<T>, IComparisonOperators<T, T, bool>
        {
            T maxVal = T.MinValue;
            int maxPos = -1;
            int pos = 0;

            foreach( var item in items)
            {
                if( item > maxVal )
                {
                    maxVal = item;
                    maxPos = pos;
                }

                pos += 1;
            }

            return (maxVal, maxPos);
        }

        public static long Product(this IEnumerable<int> numbers) 
        {
            return numbers.Aggregate(1L, (a, x) => a * x);
        }
    }
}
