using System;
using System.Collections.Generic;
using UnityEngine;

namespace Geometry
{
    public static class Grid
    {
        public static readonly Vector3Int North = new Vector3Int(0, 1, 0);
        public static readonly Vector3Int South = new Vector3Int(0, -1, 0);
        public static readonly Vector3Int East = new Vector3Int(1, 0, 0);
        public static readonly Vector3Int West = new Vector3Int(-1, 0, 0);
        public static readonly List<Vector3Int> AllDirections = new List<Vector3Int> { North, South, East, West };

        public static bool AreNeighbors(Vector3Int a, Vector3Int b)
        {
            return GetNeighbors(a).Contains(b);
        }

        /*
         * Finds the nearest position to the target from a list of options. Or an arbitrarily far
         * position if that list is empty.
         */
        public static Vector3Int NearestPosition(Vector3Int target, List<Vector3Int> options)
        {
            Vector3Int closest_option = new Vector3Int(int.MaxValue, int.MaxValue);
            foreach (Vector3Int option in options)
            {
                if ((target - option).magnitude < (target - closest_option).magnitude)
                {
                    closest_option = option;
                }
            }

            return closest_option;
        }

        public static List<Vector3Int> GetNeighbors(Vector3Int origin)
        {
            List<Vector3Int> neighbors = new List<Vector3Int> ();
            foreach (Vector3Int direction in AllDirections)
            {
                neighbors.Add(origin + direction);
            }
            return neighbors;
        }
        
        public static Vector3Int BestDirection(Vector3 v)
        {
            if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            {
                if (v.x > 0)
                {
                    return East;
                }
                else
                {
                    return West;
                }
            }
            else
            {
                if (v.y > 0)
                {
                    return North;
                }
                else
                {
                    return South;
                }
            }
        }
    }

    public static class Angle
    {
        public static Quaternion QuaternionFromVector2(Vector2 input)
        {
            float angle = Vector2.SignedAngle(Vector2.right, input);
            return Quaternion.Euler(0, 0, angle);
        }
    }

    public class PathSearchPoint : IComparable<PathSearchPoint>
    {
        public Vector3Int location;
        public float estimated_cost;

        public int CompareTo(PathSearchPoint other)
        {
            return this.estimated_cost.CompareTo(other.estimated_cost);
        }
    }
}