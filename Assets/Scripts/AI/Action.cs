using UnityEngine;

public abstract class Action
{
    public const string Name = "Action";

    public AgentEntity AgentEntity { get; protected set; }

    public Action(AgentEntity agentEntity)
    {
        AgentEntity = agentEntity;
    }

    /// <summary>
    /// Attempt to apply the action to the region model.
    /// </summary>
    /// <param name="regionModel"></param>
    /// <returns>True if action was successful</returns>
    public abstract bool ApplyAction(RegionModel regionModel);
}

public class WalkAction : Action
{
    public new const string Name = "Walk";
    public Vector3Int Direction { get; private set; }
    public Vector3Int Destination { get; private set; }

    public WalkAction(AgentEntity agentEntity, Vector3Int direction) : base(agentEntity) 
    {
        Direction = direction;
        Destination = agentEntity.Position + direction;
    }

    public override bool ApplyAction(RegionModel regionModel)
    {
        if (!regionModel.IsPassable(Destination)) { return false; }
        AgentEntity.Move(Direction);
        return true;
    }
}

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