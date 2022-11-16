using UnityEngine;

public abstract class BuildingEntity : WorldEntity
{
    public readonly BuildingType buildingType;

    public BuildingEntity(RegionModel region) : base(region)
    {

    }
}