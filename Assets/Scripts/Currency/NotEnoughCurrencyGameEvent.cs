using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughCurrencyGameEvent : GameEvent
{
    public int Amount;
    public CurrencyType CurrencyType;

    public NotEnoughCurrencyGameEvent(int amount, CurrencyType currencyType)
    {
        Amount = amount;
        CurrencyType = currencyType;
    }
}