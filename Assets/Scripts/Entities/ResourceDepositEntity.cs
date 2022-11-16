using UnityEngine;

public abstract class ResourceDepositEntity : WorldEntity
{
    public ResourceDepositType Type { get; protected set; }
    public int AmountRemaining { get; protected set; }

    public ResourceDepositEntity(RegionModel region) : base(region)
    {

    }

    /// <summary>
    /// Handle the extraction of a given amount of resource from the deposit. The amoung provided
    /// should be the unmodified amount to be extracted, any modifications should be handled by
    /// the implementation of the concrete class.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>The actual amount retrieved from the deposit</returns>
    public int ExtractAmount(int amount)
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

    public override void Destroy()
    {
        MonoBehaviour.Destroy(gameObject);
        regionModel.DestroyResourceDeposit(Position);
    }
}

