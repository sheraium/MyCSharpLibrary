using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyCSharpLibrary
{
    public class ShortestPath<TNode>
    {
        //source, other
        private readonly Dictionary<TNode, List<TNode>> _nodeMap = new Dictionary<TNode, List<TNode>>();

        private Dictionary<TNode, Dictionary<TNode, int>> _cost = new Dictionary<TNode, Dictionary<TNode, int>>();

        public ShortestPath()
        {
        }

        public void Reset()
        {
            _nodeMap.Clear();
            _cost.Clear();
        }

        public void AddPath(TNode node, TNode other)
        {
            if (_nodeMap.TryGetValue(node, out var list) == false)
            {
                _nodeMap.Add(node, new List<TNode>());
            }
            _nodeMap[node].Add(other);
        }

        public void AddPath(TNode node, IEnumerable<TNode> otherNodes)
        {
            var list = new List<TNode>();
            list.AddRange(otherNodes);
            _nodeMap.Add(node, list);
        }

        public void SetCost(TNode node, TNode otherNode, int cost)
        {
            if (_cost.TryGetValue(node, out var costList) == false)
            {
                _cost.Add(node, new Dictionary<TNode, int>());
            }
            _cost[node][otherNode] = cost;
        }

        public IEnumerable<TNode> GetPath(TNode startNode, TNode endNode)
        {
            var path = new List<TNode>();
            try
            {
                var prior = Dijkstra(startNode);
                AddPathPoint(endNode, startNode);

                //List包含start/end ，若List為空 表示不通
                void AddPathPoint(TNode end, TNode start)
                {
                    if (end.Equals(start)) return;
                    if (prior.TryGetValue(end, out var next))
                    {
                        if (next.Equals(default(TNode))) return;
                        path.Add(end);
                        if (next.Equals(start))
                        {
                            path.Add(start);
                            path.Reverse();
                            return;
                        }
                        AddPathPoint(next, start);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }

            return path;
        }

        private Dictionary<TNode, TNode> Dijkstra(TNode start)
        {
            //point, dist
            var dist = new Dictionary<TNode, int>();
            foreach (var p in _nodeMap)
            {
                dist[p.Key] = 0;
            }

            //point, prior
            var prior = new Dictionary<TNode, TNode>();
            foreach (var p in _nodeMap)
            {
                prior[p.Key] = default(TNode);
            }

            //point, decided
            var decided = new Dictionary<TNode, bool>();
            foreach (var p in _nodeMap)
            {
                decided[p.Key] = false;
            }

            foreach (var point in _nodeMap)
            {
                var key = point.Key;
                if (_cost[start].TryGetValue(key, out var value))
                {
                    dist[key] = value;
                }
                else
                {
                    dist[key] = int.MaxValue;
                }
                prior[key] = start;
                decided[key] = false;
            }

            //Dijkstra
            decided[start] = true;
            TNode Vx = default(TNode);
            foreach (var point in _nodeMap)
            {
                find_min(ref Vx);
                if ((typeof(TNode).IsValueType && Vx.Equals(default(TNode))) || Vx == null)
                {
                    foreach (var d in decided)
                    {
                        if (d.Value == false)
                            prior[d.Key] = default(TNode);
                    }
                    return prior;
                }
                decided[Vx] = true;
                foreach (var w in _cost[Vx])
                {
                    var key = w.Key;
                    var couldCost = 0;
                    if (_cost[Vx].TryGetValue(key, out couldCost) == false)
                    {
                        couldCost = int.MaxValue;
                    }

                    if (w.Value < int.MaxValue && !decided[key]
                                               && (dist[key] > (dist[Vx] + couldCost)))
                    {
                        dist[key] = dist[Vx] + couldCost;
                        prior[key] = Vx;
                    }
                }
            }

            foreach (var d in decided)
            {
                if (d.Value == false)
                    prior[d.Key] = default(TNode);
            }
            return prior;

            void find_min(ref TNode vx)
            {
                TNode low = default(TNode);
                int lowest = int.MaxValue;
                foreach (var point in _nodeMap.Keys)
                {
                    if (decided[point] == false && dist[point] < lowest)
                    {
                        lowest = dist[point];
                        low = point;
                    }
                }
                vx = low;
            }
        }
    }
}