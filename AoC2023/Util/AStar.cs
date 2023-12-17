using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023.Util
{
    internal static class AStar
    {
        private static List<TNode> ReconstructPath<TNode>(Dictionary<TNode, TNode> cameFrom, TNode current)
            where TNode: notnull
        {
            var path = new List<TNode>() { current };
            while( cameFrom.Keys.Contains(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            return path;
        }

        private static TValue ReadWithDefault<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue def)
            where TKey : notnull
        {
            if (dict.ContainsKey(key))
                return dict[key];

            dict[key] = def;
            return def;
        }

        public static List<TNode> FindPath<TNode, TDistance>(TNode start, TNode goal, Func<TNode, IEnumerable<(TNode, TDistance)>> findNeighbors, Func<TNode, TDistance> h)
            where TDistance : IMinMaxValue<TDistance>, INumber<TDistance>
            where TNode : notnull
        {
            var openSet = new List<TNode> { start };
            var cameFrom = new Dictionary<TNode, TNode>();

            var gScore = new Dictionary<TNode, TDistance>()
            {
                { start, default(TDistance)! }
            };
            var fScore = new Dictionary<TNode, TDistance>()
            {
                { start, h(start) }
            };

            while( openSet.Any() )
            {
                var current = openSet.OrderBy(x => fScore[x]).First();
                if(current.Equals(goal))
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);

                foreach (var (neighbor, weight) in findNeighbors(current))
                {
                    var tentativeScore = gScore[current] + weight;
                    var gScoreNeighbor = ReadWithDefault(gScore, neighbor, TDistance.MaxValue);
                    if (tentativeScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeScore;
                        fScore[neighbor] = tentativeScore + h(neighbor);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            throw new Exception("no path");
        }
    }
}
