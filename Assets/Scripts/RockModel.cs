using UnityEngine;

public class RockModel : IResourceDeposit
{
    // PUBLIC VARS
    public RegionModel.ResourceDepositType Type { get; private set; }
    public int AmountRemaining { get; private set; }
    public Vector3Int Position { get; private set; }

    // PRIVATE VARS
    private RockMonobehaviour Monobehaviour;

    // PUBLIC METHODS
    public RockModel(int amount, int x, int y)
    {
        Type = RegionModel.ResourceDepositType.Rock;
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

    public void SetMonobehaviour(RockMonobehaviour monobehaviour) { Monobehaviour = monobehaviour; }
}