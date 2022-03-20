interface IResourceDeposit
{
    public int AmountRemaining { get; }
    public void ExtractAmount(int amount);
}

