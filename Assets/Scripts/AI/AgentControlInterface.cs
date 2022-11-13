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
        Vector3Int direction = Util.RandomElement(Geometry.AllDirections);
        Action next_action = new WalkAction(agentEntity, direction);
        return next_action;
    }
}

public class UserInputACI : AgentControlInterface
{
    private Action ScheduledAction;

    public UserInputACI(AgentEntity agentEntity) : base(agentEntity) { }

    public override Action DecideNextAction(Percept percept)
    {
        Action result;

        if (ScheduledAction != null)
        {
            result = ScheduledAction;
            ScheduledAction = null;
        }
        else
        {
            result = new NoAction(agentEntity);
        }

        return result;
    }

    public void HandleUserInput(Vector2Int coords)
    {
        Vector2 v = coords - (Vector2Int)agentEntity.Position;
        Vector3Int direction = Geometry.BestDirection(v);
        ScheduledAction = new WalkAction(agentEntity, direction);
    }
}

public class CollectWood : AgentControlInterface
{
    public CollectWood(AgentEntity agentEntity) : base(agentEntity) { }

    public override Action DecideNextAction(Percept percept)
    {
        RegionModel region = percept.Region;
        Vector3Int nearest_wood = region.GetNearestResource(percept.Position, ResourceDepositType.Tree);
        if (nearest_wood.x == -1) { return new WalkAction(agentEntity, new Vector3Int(0, 0, 0)); }


    }
}