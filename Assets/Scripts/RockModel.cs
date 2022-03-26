using UnityEngine;

public class RockModel : IResourceDepositModel
{
    // PUBLIC VARS
    public ResourceDepositType Type { get; protected set; }
    public int AmountRemaining { get; protected set; }
    public Vector3Int Position { get; protected set; }
    public RockMonobehaviour Monobehaviour { protected get; set; }

    // PUBLIC METHODS
    public RockModel(int amount, int x, int y)
    {
        Type = ResourceDepositType.Rock;
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