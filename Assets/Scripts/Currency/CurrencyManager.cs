using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int _moneyCount = 1000;
    [SerializeField] private string _currencyName;
    [SerializeField] private TextMeshProUGUI _textMoney;

    private void Start()
    {
        if (PlayerPrefs.HasKey(_currencyName))
        {
            _moneyCount = PlayerPrefs.GetInt(_currencyName);
            DisplayCurrency();
        }
        else
        {
            _moneyCount = 1000;
        }
    }

    public void AddCurrency(int money)
    {
        _moneyCount += money;
        SaveCurrency(_moneyCount);
        DisplayCurrency();
    }

    public void RemoveCurrency(int money)
    {
        _moneyCount -= money;
        DisplayCurrency();
        SaveCurrency(_moneyCount);
    }

    private void DisplayCurrency()
    {
        _textMoney.text = _moneyCount.ToString();
    }

    private void SaveCurrency(int value)
    {
        PlayerPrefs.SetInt(_currencyName, value);
    }
}
