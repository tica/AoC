using System.Text;

namespace AoC2024
{
    public class Day9 : AoC.DayBase
    {
        private List<int> ParseInput(string filename)
        {
            bool file = true;

            int id = 0;

            var result = new List<int>();

            foreach (var n in File.ReadAllText(filename).Select(ch => ch - '0'))
            {
                if (file)
                {
                    result.AddRange(Enumerable.Repeat(id, n));
                    id += 1;
                }
                else if (n > 0)
                {
                    result.AddRange(Enumerable.Repeat(-1, n));
                }

                file = !file;
            }

            return result;
        }

        protected override object Solve1(string filename)
        {
            var disk = ParseInput(filename);

            int p = 0;
            int q = disk.Count - 1;

            while( true )
            {
                while (p < disk.Count && disk[p] > -1)
                    p += 1;

                while (q > 0 && disk[q] == -1)
                    q -= 1;

                if (p >= q)
                    break;

                disk[p] = disk[q];
                disk[q] = -1;
            }

            return disk.Select((b, i) => (b, i)).Where(t => t.b >= 0).Sum(t => t.b * (long)t.i);
        }

        private int BlockLengthLeft(List<int> lst, int p)
        {
            int v = lst[p];

            int q = p;
            do
            {
            }
            while (--q >= 0 && lst[q] == v);

            return p - q;
        }

        private int BlockLengthRight(List<int> lst, int p)
        {
            int v = lst[p];

            int q = p;
            do
            {
            }
            while (++q < lst.Count && lst[q] == v);

            return q - p;
        }

        int FindFreeBlock(List<int> disk, int from, int to, int size)
        {
            int p = from;

            while (p < to)
            {
                while (p < to && disk[p] > -1)
                    p += 1;

                if (disk[p] == -1)
                {
                    int freelen = BlockLengthRight(disk, p);

                    if (freelen >= size)
                        return p;

                    p += freelen;
                }
            }

            return -1;
        }

        protected override object Solve2(string filename)
        {
            var disk = ParseInput(filename);

            int q = disk.Count - 1;

            while (true)
            {
                while (q > 0 && disk[q] == -1)
                    q -= 1;

                if (q < 0)
                    break;

                int len = BlockLengthLeft(disk, q);

                int free = FindFreeBlock(disk, 0, q, len);

                if( free >= 0 )
                {
                    for( int i = 0; i < len; ++i)
                    {
                        disk[free + i] = disk[q - len + 1 + i];
                        disk[q - len + 1 + i] = -1;
                    }
                }
                else
                {
                    q -= len;
                }

                if (q == 0)
                    break;
            }

            return disk.Select((b, i) => (b, i)).Where(t => t.b >= 0).Sum(t => t.b * (long)t.i);
        }

        public override object SolutionExample1 => 1928L;
        public override object SolutionPuzzle1 => 6435922584968L;
        public override object SolutionExample2 => 2858L;
        public override object SolutionPuzzle2 => 6469636832766L;
    }
}
