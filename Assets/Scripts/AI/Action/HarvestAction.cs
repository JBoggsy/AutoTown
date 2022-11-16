using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestAction : Action
{
    public Vector3Int TargetPos { get; private set; }
    private int amount;

    public HarvestAction(AgentEntity agentEntity, Vector3Int target_pos, int amount) : base(agentEntity)
    {
        TargetPos = target_pos;
        this.amount = amount;
        this.name = "Harvest";
    }

    public override bool ApplyAction(RegionModel regionModel)
    {
        bool result = true;

        ResourceDepositEntity deposit = TownSceneManager.Instance.Region.GetResourceAt(TargetPos);
        if (deposit != null && Geometry.Grid.AreNeighbors(TargetPos, AgentEntity.Position) && deposit.AmountRemaining > 0)
        {
            deposit.ExtractAmount(amount);
            ItemType item = Constants.ItemFromDeposit(deposit.Type);
            result &= AgentEntity.InsertItemIntoInventory(item);
        }
        else
        {
            result = false;
        }

        if (result) { Status = "success"; } 
        else { Status = "error"; }
        return result;
    }
}