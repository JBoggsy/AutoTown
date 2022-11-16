using System;
using UnityEngine;

public class TreeEntity : ResourceDepositEntity
{
    public TreeEntity(RegionModel region, int amount, int x, int y) : base(region)
    {
        Type = ResourceDepositType.Tree;
        AmountRemaining = amount;
        Position = new Vector3Int(x, y, 0);
    }
    
    public new int ExtractAmount(int amount)
    {
        int extracted_amount = 0;
        if (amount <= AmountRemaining)
        {
            extracted_amount = amount;
            AmountRemaining = AmountRemaining - amount;
        }
        else
        {
            extracted_amount = AmountRemaining;
            AmountRemaining = 0;
        }
        if (AmountRemaining <= 0)
        {
            Destroy();
        }
        return extracted_amount;
    }

    public override bool ApplyDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public override bool ApplyHeal(float heal)
    {
        throw new System.NotImplementedException();
    }
}