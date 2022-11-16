using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAction : Action
{
    public NoAction(AgentEntity agentEntity) : base(agentEntity)
    {
        this.name = "NoAction";
        this.Status = "success";
    }

    public override bool ApplyAction(RegionModel regionModel)
    {
        return true;
    }
}