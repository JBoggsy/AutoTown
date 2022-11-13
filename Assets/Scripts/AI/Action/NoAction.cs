using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAction : Action
{
    public new const string Name = "NoAction";

    public NoAction(AgentEntity agentEntity) : base(agentEntity)
    {

    }

    public override bool ApplyAction(RegionModel regionModel)
    {
        return true;
    }
}