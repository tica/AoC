﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public static class AStar
    {
        private static List<TNode> ReconstructPath<TNode>(Dictionary<TNode, TNode> cameFrom, TNode current)
            where TNode: notnull
        {
            IEnumerable<TNode> path = new List<TNode>() { current };
            while( cameFrom.Keys.Contains(current))
            {
                current = cameFrom[current];
                path = path.Prepend(current);
            }
            return path.ToList();
        }

        public static List<TNode> FindPath<TNode, TDistance>(TNode start, TNode goal, Func<TNode, IEnumerable<(TNode, TDistance)>> findNeighbors, Func<TNode, TDistance> heuristic)
            where TDistance : INumber<TDistance>
            where TNode : notnull
        {
            return FindPath(start, n => n.Equals(goal), findNeighbors, heuristic);
        }

        public static List<TNode> FindPath<TNode, TDistance>(TNode start, Func<TNode, bool> isGoal, Func<TNode, IEnumerable<(TNode, TDistance)>> findNeighbors, Func<TNode, TDistance> heuristic)
            where TDistance : INumber<TDistance>
            where TNode : notnull
        {
            var openSet = new PriorityQueue<TNode, TDistance>();
            openSet.Enqueue(start, heuristic(start));

            var cameFrom = new Dictionary<TNode, TNode>();

            var distanceToNode = new Dictionary<TNode, TDistance>()
            {
                { start, TDistance.Zero }
            };

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                if (isGoal(current))
                {
                    return ReconstructPath(cameFrom, current);
                }

                foreach (var (neighbor, distance) in findNeighbors(current))
                {
                    var tentativeScore = distanceToNode[current] + distance;

                    if (!distanceToNode.ContainsKey(neighbor) || tentativeScore < distanceToNode[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        distanceToNode[neighbor] = tentativeScore;

                        // This would be the correct way to save memory, but not doing this is much faster
                        //if (!openSet.UnorderedItems.Any(x => x.Element.Equals(neighbor)))

                        openSet.Enqueue(neighbor, tentativeScore + heuristic(neighbor));
                    }
                }
            }

            return null!;
        }
    }
}
