using System.Collections.Generic;

namespace MyCSharpLibrary
{
    internal class ShortestPath
    {
        //source, other
        private readonly Dictionary<int, int[]> _points = new Dictionary<int, int[]>();

        //source, destination, path
        private readonly Dictionary<int, Dictionary<int, List<int>>> _path = new Dictionary<int, Dictionary<int, List<int>>>();

        public IEnumerable<int> GetAllPoint()
        {
            return _points.Keys;
        }

        public IEnumerable<int> GetPath(int start, int end)
        {
            _path.TryGetValue(start, out var pstart);
            if (pstart == null) return new List<int>();
            pstart.TryGetValue(end, out var path);
            if (path == null) return new List<int>();

            return path;
        }

        public ShortestPath()
        {
            _points[1] = new[] { 24 };
            _points[2] = new[] { 22 };
            _points[3] = new[] { 21 };
            _points[4] = new[] { 21 };
            _points[5] = new[] { 26 };
            _points[6] = new[] { 26 };
            _points[21] = new[] { 22, 3, 4 };
            _points[22] = new[] { 21, 23, 2 };
            _points[23] = new[] { 22, 24 };
            _points[24] = new[] { 23, 25, 1 };
            _points[25] = new[] { 24, 26 };
            _points[26] = new[] { 25, 5, 6 };

            //_points[1] = new[] { 2,3 };
            //_points[2] = new[] { 1 };
            //_points[3] = new[] { 4,1 };
            //_points[4] = new[] { 3 };

            Calculate();
        }

        private void Calculate()
        {
            Dictionary<int, int> prior = new Dictionary<int, int>();
            foreach (var startPoint in _points.Keys)
            {
                prior = Dijkstra(startPoint);
                _path.Add(startPoint, new Dictionary<int, List<int>>());

                foreach (var endPoint in _points.Keys)
                {
                    if (startPoint == endPoint) continue;
                    _path[startPoint].Add(endPoint, new List<int>());
                    AddPathPoint(endPoint, startPoint, _path[startPoint][endPoint]);
                }
            }
            //List包含start/end ，若List為空 表示不通
            void AddPathPoint(int end, int start, List<int> path)
            {
                if (end == start) return;
                if (prior.TryGetValue(end, out var next))
                {
                    if (next == 0) return;
                    path.Add(end);
                    if (next == start)
                    {
                        path.Add(start);
                        path.Reverse();
                        return;
                    }
                    AddPathPoint(next, start, path);
                }
            }
        }

        private Dictionary<int, int> Dijkstra(int start)
        {
            //point, point, cost
            var cost = new Dictionary<int, Dictionary<int, int>>();
            foreach (var p in _points)
            {
                cost.Add(p.Key, new Dictionary<int, int>());
                foreach (var o in p.Value)
                {
                    cost[p.Key].Add(o, 1);
                }
            }

            //point, dist
            var dist = new Dictionary<int, int>();
            foreach (var p in _points)
            {
                dist[p.Key] = 0;
            }

            //point, prior
            var prior = new Dictionary<int, int>();
            foreach (var p in _points)
            {
                prior[p.Key] = 0;
            }

            //point, decided
            var decided = new Dictionary<int, bool>();
            foreach (var p in _points)
            {
                decided[p.Key] = false;
            }

            foreach (var point in _points)
            {
                var key = point.Key;
                if (cost[start].TryGetValue(key, out var value))
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
            int Vx = 0;
            foreach (var point in _points)
            {
                find_min(ref Vx);
                if (Vx == 0)
                {
                    foreach (var d in decided)
                    {
                        if (d.Value == false)
                            prior[d.Key] = 0;
                    }
                    return prior;
                }
                decided[Vx] = true;
                foreach (var w in cost[Vx])
                {
                    var key = w.Key;
                    var couldCost = 0;
                    if (cost[Vx].TryGetValue(key, out couldCost) == false)
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
                    prior[d.Key] = 0;
            }
            return prior;

            void find_min(ref int vx)
            {
                int low = 0;
                int lowest = int.MaxValue;
                foreach (var point in _points.Keys)
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