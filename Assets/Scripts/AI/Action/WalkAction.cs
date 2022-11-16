using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAction : Action
{
    public Vector3Int Direction { get; private set; }

    public WalkAction(AgentEntity agentEntity, Vector3Int direction) : base(agentEntity)
    {
        Direction = direction;
        this.name = "Walk";
    }

    public override bool ApplyAction(RegionModel regionModel)
    {
        bool success;
        Vector3Int destination = AgentEntity.Position + Direction;
        if (regionModel.IsPassable(destination))
        {
            AgentEntity.Move(Direction);
            success = true;
        }
        else
        {
            success = false;
        }

        if (success) { Status = "success"; }
        else { Status = "error"; }
        return success;
    }
}