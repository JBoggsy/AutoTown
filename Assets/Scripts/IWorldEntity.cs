using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Super-class for all entities that exist in the world, including resource deposits, items, people, etc.
 */
public interface IWorldEntity
{
    public Vector3Int Position { get; }
}
