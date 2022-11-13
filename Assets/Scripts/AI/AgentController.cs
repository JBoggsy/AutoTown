using System.Collections.Generic;
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

public class RandomWalkAC : AgentController
{
    public RandomWalkAC(AgentEntity agentEntity) : base(agentEntity) { }

    public override Action DecideNextAction(Percept percept)
    {
        Vector3Int direction = Util.RandomElement(Geometry.AllDirections);
        Action next_action = new WalkAction(agentEntity, direction);
        return next_action;
    }
}

public class UserInputAC : AgentController
{
    private Action ScheduledAction;

    public UserInputAC(AgentEntity agentEntity) : base(agentEntity) { }

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

public class CollectWoodAC : AgentController
{
    private bool started;
    private Vector3Int nearest_wood;

    public CollectWoodAC(AgentEntity agentEntity) : base(agentEntity) 
    {
        started = false;
        nearest_wood = new Vector3Int(-1, -1, -1);
    }

    private void _Begin(Percept percept)
    {
        RegionModel region = percept.Region;
        nearest_wood = region.GetNearestResource(percept.Position, ResourceDepositType.Tree);
        started = true;
    }

    public override Action DecideNextAction(Percept percept)
    {
        if (!started) 
        {
            _Begin(percept); 
        }
        else
        {
            List<Vector3Int> nbors = Geometry.GetNeighbors(percept.Position);
            if (nbors.Contains(nearest_wood))
            {
                started = false;
                return new WalkAction(agentEntity, new Vector3Int(0, 0, 0));
            }
        }
        Vector3Int heading_vector = nearest_wood - percept.Position;
        Vector3Int direction = Geometry.BestDirection(heading_vector);
        return new WalkAction(agentEntity, direction);
    }
}