﻿using System;
using System.Collections.Generic;
using UnityEngine;

public static class Geometry
{
    public static readonly Vector3Int North = new Vector3Int(0, 1, 0);
    public static readonly Vector3Int South = new Vector3Int(0, -1, 0);
    public static readonly Vector3Int East = new Vector3Int(1, 0, 0);
    public static readonly Vector3Int West = new Vector3Int(-1, 0, 0);
    public static readonly List<Vector3Int> AllDirections = new List<Vector3Int> { North, South, East, West };

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

    public static Quaternion QuaternionFromVector2(Vector2 input)
    {
        float angle = Vector2.SignedAngle(Vector2.right, input);
        return Quaternion.Euler(0, 0, angle);
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