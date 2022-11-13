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