using UnityEngine;

public abstract class BuildingModel : IWorldEntity
{
    public Vector3Int Position { get; protected set; }

    public readonly BuildingType buildingType;
    public int Health { get; protected set; }
}