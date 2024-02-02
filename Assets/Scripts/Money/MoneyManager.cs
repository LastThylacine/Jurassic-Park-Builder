using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int _defaulMoneyAmount = 1000;

    private void Start()
    {
        //if (PlayerPrefs.HasKey("Money"))
        //{
        //    _moneyCount = PlayerPrefs.GetInt("Money");
        //    DisplayMoney();
        //}
        //else
        //{
        //    _moneyCount = 1000;
        //}

        AddCoins(_defaulMoneyAmount);
    }

    public void AddCoins(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(amount, CurrencyType.Coins);

        EventManager.Instance.QueueEvent(info);
    }

    public void RemoveCoins(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(-amount, CurrencyType.Coins);

        EventManager.Instance.QueueEvent(info);
    }
}
