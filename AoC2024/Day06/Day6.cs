using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;
using AoC.Util;
using Grid = AoC.Util.Grid<char>;
using Coord = AoC.Util.Grid<char>.Coord;

namespace AoC2024
{
    public class Day6 : AoC.DayBase
    {
        private List<Coord> FollowPath(Grid grid)
        {
            var p = grid.AllCoordinates.Single(p => p.Value == '^');
            var dir = Direction.Up;

            var visited = new HashSet<Coord>();

            while (p.IsValid)
            {
                visited.Add(p);

                var q = p.Neighbor(dir);
                while (q.IsValid && q.Value == '#')
                {
                    dir = dir.TurnRight();
                    q = p.Neighbor(dir);
                }

                p = p.Neighbor(dir);
            }

            return visited.ToList();
        }

        protected override object Solve1(string filename)
        {
            var grid = GridHelper.Load(filename);

            var visited = FollowPath(grid);

            return visited.Count();
        }

        private bool DetectCircle(Grid grid)
        {
            var p = grid.AllCoordinates.Single(p => p.Value == '^');
            var dir = Direction.Up;

            var visited = new HashSet<(Coord, Direction)>();

            while (p.IsValid)
            {
                if (visited.Contains((p, dir)))
                {
                    return true;
                }

                visited.Add((p, dir));

                var q = p.Neighbor(dir);
                while (q.IsValid && q.Value == '#')
                {
                    dir = dir.TurnRight();
                    q = p.Neighbor(dir); 
                }

                grid.Set(p, 'X');

                p = p.Neighbor(dir);
            }

            return false;
        }

        protected override object Solve2(string filename)
        {
            var grid = GridHelper.Load(filename);
            var start = grid.AllCoordinates.Single(p => p.Value == '^');

            var candidates = FollowPath(grid).Where(p => p != start);

            int count = 0;

            foreach (var p in candidates)
            {
                grid = GridHelper.Load(filename);
                grid.Set(p, '#');

                if (DetectCircle(grid))
                {
                    count += 1;
                    //grid.Set(p, 'O');
                    //grid.Print();
                }

                Console.Write(".");
            }

            return count;
        }

        public override object SolutionExample1 => 41;
        public override object SolutionPuzzle1 => 5239;
        public override object SolutionExample2 => 6;
        public override object SolutionPuzzle2 => 1753;
    }
}
