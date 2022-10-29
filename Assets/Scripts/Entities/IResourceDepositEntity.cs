﻿using UnityEngine;

public interface IResourceDepositEntity: IWorldEntity
{
    public ResourceDepositType Type { get; }
    public int AmountRemaining { get; }

    /**
     * Returns the actual amount retrieved from the deposit, which may be less than the
     * given argument if the deposit doesn't have enough resources left.
     */
    public int ExtractAmount(int amount);

    public void DestroyDeposit();
}

