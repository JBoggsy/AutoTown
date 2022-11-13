using UnityEngine;

public abstract class AgentController
{
    protected AgentEntity agentEntity;

    public AgentController(AgentEntity agentEntity)
    {
        this.agentEntity = agentEntity;
    }

    public abstract Action DecideNextAction(Percept percept);
}

public class RandomWalkACI : AgentController
{
    public RandomWalkACI(AgentEntity agentEntity) : base(agentEntity) { }

    public override Action DecideNextAction(Percept percept)
    {
        Vector3Int direction = Util.RandomElement(Geometry.AllDirections);
        Action next_action = new WalkAction(agentEntity, direction);
        return next_action;
    }
}

public class UserInputACI : AgentController
{
    private bool HasActivity;
    private Vector3Int Destination;

    public UserInputACI(AgentEntity agentEntity) : base(agentEntity) { }

    public override Action DecideNextAction(Percept percept)
    {
        Action action = new NoAction(agentEntity);

        if (HasActivity)
        {
            if (agentEntity.Position == Destination)
            {
                HasActivity = false;
            }
            else
            {
                Vector3 v = Destination - agentEntity.Position;
                Vector3Int direction = Geometry.BestDirection(v);
                action = new WalkAction(agentEntity, direction);
            }
        }

        return action;
    }

    public void HandleUserInput(Vector2Int coords)
    {
        Destination = (Vector3Int)coords;
        HasActivity = true;
    }
}

public class CollectWood : AgentController
{
    public CollectWood(AgentEntity agentEntity) : base(agentEntity) { }

    public override Action DecideNextAction(Percept percept)
    {
        Action action = new NoAction(agentEntity);

        RegionModel region = percept.Region;
        Vector3Int nearest_wood = region.GetNearestResource(percept.Position, ResourceDepositType.Tree);
        Vector3Int nearest_access = Geometry.NearestPosition(percept.Position, Geometry.GetNeighbors(nearest_wood));

        if (nearest_access != percept.Position)
        {
            Vector3 v = nearest_access - agentEntity.Position;
            Vector3Int direction = Geometry.BestDirection(v);
            action = new WalkAction(agentEntity, direction);
        }
        else
        {
            action = new HarvestAction(agentEntity, nearest_wood);
        }

        return action;
    }
}