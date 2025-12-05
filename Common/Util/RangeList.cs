using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public class RangeList
    {
        public List<Range> Ranges { get; private set; } = new List<Range>();

        public RangeList() { }

        public RangeList(IEnumerable<Range> ranges)
        {
            foreach (Range r in ranges)
            {
                Add(r);
            }
        }

        public void Add(Range r)
        {
            Ranges.Add(r);
            Consolidate();
        }

        public void Subtract(Range r)
        {
            var result = new List<Range>();

            foreach (var x in Ranges)
            {
                if (x.Intersects(r))
                {
                    result.AddRange(x.Subtract(r));
                }
                else
                {
                    result.Add(x);
                }
            }

            Ranges = result;
        }

        private void Consolidate()
        {
            Ranges = Ranges.OrderBy(r => r.Begin).ToList();

            var result = new List<Range>();

            var open = Ranges.First();
            foreach (var r in Ranges.Skip(1))
            {
                if (r.Touches(open))
                {
                    open = open.Merge(r);
                }
                else
                {
                    result.Add(open);
                    open = r;
                }
            }
            result.Add(open);

            Ranges = result;
        }
    }

}
