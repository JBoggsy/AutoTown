using UnityEngine;

public abstract class AgentControlInterface
{
    protected AgentEntity agentEntity;

    public AgentControlInterface(AgentEntity agentEntity)
    {
        this.agentEntity = agentEntity;
    }

    public abstract Action DecideNextAction(Percept percept);
}

public class RandomWalkACI : AgentControlInterface
{
    public RandomWalkACI(AgentEntity agentEntity) : base(agentEntity) { }

    public override Action DecideNextAction(Percept percept)
    {
        Vector3Int direction = Direction.All[Random.Range(0, 4)];
        Action next_action = new WalkAction(agentEntity, direction);
        return next_action;
    }
}