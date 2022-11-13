using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestAction : Action
{
    public new const string Name = "Harvest";

    public Vector3Int Target { get; private set; }

    public HarvestAction(AgentEntity agentEntity, Vector3Int target) : base(agentEntity)
    {
        Target = target;
    }

    public override bool ApplyAction(RegionModel regionModel)
    {
        bool result = true;

        IResourceDepositEntity deposit = TownSceneManager.Instance.Region.GetResourceAt(Target);
        if (deposit != null && Geometry.AreNeighbors(Target, AgentEntity.Position) && deposit.AmountRemaining > 0)
        {
            deposit.ExtractAmount(1);
            ItemType item = Constants.ItemFromDeposit(deposit.Type);
            result &= AgentEntity.InsertItemIntoInventory(item);
        }
        else
        {
            result = false;
        }

        return result;
    }
}