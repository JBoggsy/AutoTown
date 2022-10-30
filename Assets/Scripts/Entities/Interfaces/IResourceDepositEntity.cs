public interface IResourceDepositEntity
{
    public ResourceDepositType Type { get; }
    public int AmountRemaining { get; }

    /// <summary>
    /// Handle the extraction of a given amount of resource from the deposit. The amoung provided
    /// should be the unmodified amount to be extracted, any modifications should be handled by
    /// the implementation of the concrete class.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>The actual amount retrieved from the deposit</returns>
    public int ExtractAmount(int amount);
}

