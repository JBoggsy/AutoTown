using UnityEngine;

public interface IResourceDeposit
{
    public RegionModel.ResourceDepositType Type { get; }
    public int AmountRemaining { get; }
    public Vector3Int Position { get; }

    /**
     * Returns the actual amount retrieved from the deposit, which may be less than the
     * given argument if the deposit doesn't have enough resources left.
     */
    public int ExtractAmount(int amount);

    public void DestroyDeposit();
}

