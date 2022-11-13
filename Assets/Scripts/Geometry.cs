using System.Collections.Generic;
using UnityEngine;

public static class Geometry
{
    public static readonly Vector3Int North = new Vector3Int(0, 1, 0);
    public static readonly Vector3Int South = new Vector3Int(0, -1, 0);
    public static readonly Vector3Int East = new Vector3Int(1, 0, 0);
    public static readonly Vector3Int West = new Vector3Int(-1, 0, 0);
    public static readonly List<Vector3Int> All = new List<Vector3Int> { North, South, East, West };

    public static List<Vector3Int> GetNeighbors(Vector3Int origin)
    {
        return new List<Vector3Int>
        {
            origin + North,
            origin + South,
            origin + East,
            origin + West
        };
    }

    public struct PathSearchPoint
    {
        public Vector3Int location;
        public int cost;
        public float heuristic_val;
    }

    /// <summary>
    /// Uses the A* algorithm to find a path from the origin to the destination.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static Stack<Vector3Int> FindPath(Vector3Int origin, Vector3Int destination)
    {
        List<PathSearchPoint> fringe = new List<PathSearchPoint>();
        Dictionary<Vector3, int> exploration = new Dictionary<Vector3, int>();

        PathSearchPoint origin_search_point = new PathSearchPoint
        {
            location = origin,
            cost = 0,
            heuristic_val = (destination-origin).magnitude
        };
        fringe.Add(origin_search_point);
        bool success = false;
        while (fringe.Count > 0)
        {
            PathSearchPoint current_point = fringe[0];
            fringe.RemoveAt(0);
            Vector3Int current_pos = current_point.location;
            List<Vector3Int> neighbors = Geometry.GetNeighbors(current_pos);
            foreach (Vector3Int nbor in neighbors)
            {
                if (nbor.Equals(destination)) { success = true; break; }
                if (exploration.ContainsKey(nbor)) { continue; }

                PathSearchPoint new_search_point = new PathSearchPoint
                {
                    location = nbor,
                    cost = current_point.cost + 1,
                    heuristic_val = (destination - nbor).magnitude
                };
                int insert_idx = 0;
                for (; insert_idx < fringe.Count; insert_idx++)
                {
                    if ((fringe[insert_idx].cost + fringe[insert_idx].heuristic_val) > (new_search_point.cost + new_search_point.heuristic_val)) { break; }
                }
                fringe.Insert(insert_idx, new_search_point);
            }
        }

        if (!success) { return null; }

        Stack<Vector3Int> path = new Stack<Vector3Int>();
        Vector3Int point = origin;

    }
}