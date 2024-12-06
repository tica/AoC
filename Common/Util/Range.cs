using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public record class Range(long Begin, long Length)
    {
        public long End => Begin + Length;

        public bool Contains(long val) => val >= Begin && val < End;

        public Range Intersect(Range other)
        {
            var begin = Math.Max(Begin, other.Begin);
            var end = Math.Min(End, other.End);
            if (begin >= end)
                throw new Exception("No intersection");

            return new Range(begin, end - begin);
        }

        public bool Intersects(Range other)
        {
            var begin = Math.Max(Begin, other.Begin);
            var end = Math.Min(End, other.End);
            return (begin < end);
        }

        public bool Touches(Range other)
        {
            if (other.Begin >= End)
                return false;
            if (Begin >= other.End)
                return false;

            return true;
        }

        public bool IsBehind(Range other)
        {
            return Begin > other.End;
        }

        public Range Merge(Range other)
        {
            var begin = Math.Min(Begin, other.Begin);
            var end = Math.Max(End, other.End);

            return new Range(begin, end - begin);
        }

        public IEnumerable<Range> Subtract(Range other)
        {
            if (!Intersects(other))
                throw new Exception("No intersection");

            if (other.Begin > Begin)
            {
                yield return new Range(Begin, other.Begin - Begin);
            }
            if (other.End < End)
            {
                yield return new Range(other.End, End - other.End);
            }
        }
    }
}
