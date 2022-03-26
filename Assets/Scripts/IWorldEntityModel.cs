using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Super-class for all entities that exist in the world, including resource deposits, items, people, etc.
 */
public interface IWorldEntityModel
{
    public Vector3Int Position { get; }
}
