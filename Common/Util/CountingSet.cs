using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace AoC.Util
{
    public class CountingSet<T, U> : IReadOnlyDictionary<T, U> where T : notnull where U : IAdditionOperators<U, U, U>
    {
        private readonly Dictionary<T, U> counts = new();

        public U this[T key] => ((IReadOnlyDictionary<T, U>)counts)[key];

        public IEnumerable<T> Keys => ((IReadOnlyDictionary<T, U>)counts).Keys;

        public IEnumerable<U> Values => ((IReadOnlyDictionary<T, U>)counts).Values;

        public int Count => ((IReadOnlyCollection<KeyValuePair<T, U>>)counts).Count;

        public void Add(T key, U count)
        {
            if (counts.ContainsKey(key))
            {
                counts[key] += count;
            }
            else
            {
                counts[key] = count;
            }
        }

        public bool ContainsKey(T key)
        {
            return ((IReadOnlyDictionary<T, U>)counts).ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<T, U>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T, U>>)counts).GetEnumerator();
        }

        public bool TryGetValue(T key, [MaybeNullWhen(false)] out U value)
        {
            return ((IReadOnlyDictionary<T, U>)counts).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)counts).GetEnumerator();
        }
    }

    public class CountingSet<T> : CountingSet<T, long> where T : notnull
    {
    }
}
