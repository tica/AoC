using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022
{
    record class NodeInfo(string Name, int FlowRate, string[] NeightborNames);
    record class Node(string Name, int FlowRate, List<(Node, int)> Neighbors)
    {
        public bool IsOpen { get; set; } = false;

        public override string ToString()
        {
            return $"{Name} ({FlowRate}, {(IsOpen ? "open" : "closed")}) ({string.Join(',', Neighbors.Select(n => $"{n.Item1.Name}:{n.Item2}"))})";
        }
    }
}
