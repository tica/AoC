using System.Security.Cryptography;
using System.Text;

namespace AoC2015
{
    public class Day4 : AoC.DayBase
    {
        protected override object Solve1(string filename)
        {
            var key = File.ReadAllText(filename);

            int n = 0;
            while( true )
            {
                var data = Encoding.ASCII.GetBytes(key + n.ToString());
                var hash = MD5.HashData(data);
                var hashString = BitConverter.ToString(hash);

                if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
                    return n;

                n += 1;
            }
        }

        protected override object Solve2(string filename)
        {
            var key = File.ReadAllText(filename);

            int n = 0;
            while (true)
            {
                var data = Encoding.ASCII.GetBytes(key + n.ToString());
                var hash = MD5.HashData(data);

                if (hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
                    return n;

                n += 1;
            }
        }

        public override object SolutionExample1 => 609043;
        public override object SolutionPuzzle1 => 282749;
        public override object SolutionExample2 => 6742839;
        public override object SolutionPuzzle2 => 9962624;
    }
}
