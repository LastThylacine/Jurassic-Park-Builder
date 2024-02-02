using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyChangeGameEvent : GameEvent
{
    public int Amount;
    public CurrencyType CurrencyType;

    public CurrencyChangeGameEvent(int amount, CurrencyType currencyType)
    {
        Amount = amount;
        CurrencyType = currencyType;
    }
}