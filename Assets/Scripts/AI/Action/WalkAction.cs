using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAction : Action
{
    public new const string Name = "Walk";

    public Vector3Int Direction { get; private set; }

    public WalkAction(AgentEntity agentEntity, Vector3Int direction) : base(agentEntity)
    {
        Direction = direction;
    }

    public override bool ApplyAction(RegionModel regionModel)
    {
        Vector3Int destination = AgentEntity.Position + Direction;
        if (regionModel.IsPassable(destination))
        {
            AgentEntity.Move(Direction);
            return true;
        }
        else
        {
            return false;
        }
    }
}