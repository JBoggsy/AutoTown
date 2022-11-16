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
        Vector3Int direction = Util.RandomElement(Geometry.Grid.AllDirections);
        Action next_action = new WalkAction(agentEntity, direction);
        return next_action;
    }
}

public class UserInputAC : AgentController
{
    private bool HasActivity;
    private Vector3Int Destination;

    public UserInputAC(AgentEntity agentEntity) : base(agentEntity) { }

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
                Vector3Int direction = Geometry.Grid.BestDirection(v);
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

public class CollectWoodAC : AgentController
{
    private enum State
    {
        FindingWood,
        MovingToWood,
        AtWood
    }
    private State state;
    private Vector3Int nearestWoodLoc;
    private ResourceDepositEntity targetDeposit;
    private Action previousAction;

    public CollectWoodAC(AgentEntity agentEntity) : base(agentEntity) 
    {
        state = State.FindingWood;
        nearestWoodLoc = new Vector3Int(-1, -1, -1);
        previousAction = new NoAction(agentEntity);
    }

    private void _FindNearestWood(Percept percept)
    {
        RegionModel region = percept.Region;
        nearestWoodLoc = region.GetNearestResourceDeposit(percept.Position, ResourceDepositType.Tree);
        targetDeposit = region.GetResourceAt(nearestWoodLoc);
    }

    public override Action DecideNextAction(Percept percept)
    {
        Debug.Log(string.Format("State: {0} | Last action name: {0}", state, previousAction.GetName()));
        Action next_action = new NoAction(agentEntity);
        if (state == State.FindingWood)
        {
            _FindNearestWood(percept);
            state = State.MovingToWood;
        }
        if (state == State.MovingToWood)
        {
            List<Vector3Int> nbors = Geometry.Grid.GetNeighbors(percept.Position);
            if (nbors.Contains(nearestWoodLoc))
            {
                state = State.AtWood;
            }
            else
            {
                Vector3Int heading_vector = nearestWoodLoc - percept.Position;
                Vector3Int direction = Geometry.Grid.BestDirection(heading_vector);
                next_action = new WalkAction(agentEntity, direction);
            }
        }
        if (state == State.AtWood)
        {
            if (previousAction.GetName().Equals("Walk") || previousAction.Status.Equals("success"))
            {
                next_action = new HarvestAction(agentEntity, nearestWoodLoc, 10);
            } else
            {
                state = State.FindingWood;
            }
        }

        previousAction = next_action;
        return next_action;
    }
}