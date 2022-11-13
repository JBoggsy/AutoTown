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
    private enum State
    {
        FindingWood,
        MovingToWood,
        AtWood
    }
    private State state;
    private Vector3Int nearest_wood_loc;
    private IResourceDepositEntity target_deposit;

    public CollectWoodAC(AgentEntity agentEntity) : base(agentEntity) 
    {
        state = State.FindingWood;
        nearest_wood_loc = new Vector3Int(-1, -1, -1);
    }

    private Vector3Int _FindNearestWood(Percept percept)
    {
        RegionModel region = percept.Region;
        nearest_wood_loc = region.GetNearestResourceDeposit(percept.Position, ResourceDepositType.Tree);
        target_deposit = region.GetResourceAt(nearest_wood_loc);
    }

    public override Action DecideNextAction(Percept percept)
    {
        if (state == State.FindingWood) 
        {
             _FindNearestWood(percept);
            state = State.MovingToWood;
        }
        if (state == State.MovingToWood)
        {
            List<Vector3Int> nbors = Geometry.GetNeighbors(percept.Position);
            if (nbors.Contains(nearest_wood_loc))
            {
                state = State.AtWood;
            } else
            {
                Vector3Int heading_vector = nearest_wood_loc - percept.Position;
                Vector3Int direction = Geometry.BestDirection(heading_vector);
                return new WalkAction(agentEntity, direction);
            }
        if (state == State.AtWood)
            {

                return new NoAction(agentEntity);
            }
    }
}