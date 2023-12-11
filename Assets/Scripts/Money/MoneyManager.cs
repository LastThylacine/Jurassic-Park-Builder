using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int _moneyCount = 1000;
    [SerializeField] private TextMeshProUGUI _textMoney;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            _moneyCount = PlayerPrefs.GetInt("Money");
            DisplayMoney();
        }
        else
        {
            _moneyCount = 1000;
        }
    }

    public void AddMoney(int money)
    {
        _moneyCount += money;
        SaveMoney(_moneyCount);
        DisplayMoney();
    }

    public void RemoveMoney(int money)
    {
        _moneyCount -= money;
        DisplayMoney();
        SaveMoney(_moneyCount);
    }

    private void DisplayMoney()
    {
        _textMoney.text = _moneyCount.ToString();
    }

    private void SaveMoney(int money)
    {
        PlayerPrefs.SetInt("Money", money);
    }
}
