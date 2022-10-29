using UnityEngine;

public class TreeEntity : IResourceDepositEntity
{
    public TreeMonobehaviour Monobehaviour { protected get; set; }

    public Vector3Int Position { get; protected set; }

    public ResourceDepositType Type { get; protected set; }
    public int AmountRemaining { get; protected set; }


    public TreeEntity(int amount, int x, int y)
    {
        Type = ResourceDepositType.Tree;
        AmountRemaining = amount;
        Position = new Vector3Int(x, y, 0);
    }
    
    public int ExtractAmount(int amount)
    {
        int extracted_amount = 0;
        if (amount >= AmountRemaining)
        {
            extracted_amount = amount;
            AmountRemaining = AmountRemaining - amount;
        } else
        {
            extracted_amount = AmountRemaining;
            AmountRemaining = 0;
        }
        if (AmountRemaining == 0)
        {
            DestroyDeposit();
        }
        return extracted_amount;
    }

    public void DestroyDeposit()
    {
        return;
    }
}