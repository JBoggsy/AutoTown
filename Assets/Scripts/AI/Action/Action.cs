using UnityEngine;

public abstract class Action
{
    protected string name = "Action";

    public AgentEntity AgentEntity { get; protected set; }
    public string Status { get; protected set; }

    public Action(AgentEntity agentEntity)
    {
        AgentEntity = agentEntity;
        Status = "not started";
    }

    public string GetName()
    {
        return name;
    }

    /// <summary>
    /// Attempt to apply the action to the region model.
    /// </summary>
    /// <param name="regionModel"></param>
    /// <returns>True if action was successful</returns>
    public abstract bool ApplyAction(RegionModel regionModel);
}