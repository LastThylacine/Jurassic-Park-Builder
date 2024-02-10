using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> _texts;

    private static Dictionary<CurrencyType, int> _currencyAmounts = new Dictionary<CurrencyType, int>();

    private static Dictionary<CurrencyType, TextMeshProUGUI> _currencyTexts = new Dictionary<CurrencyType, TextMeshProUGUI>();

    private void Awake()
    {
        for (int i = 0; i < _texts.Count; i++)
        {
            _currencyAmounts.Add((CurrencyType)i, 0);
            _currencyTexts.Add((CurrencyType)i, _texts[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }

    private void Start()
    {
        EventManager.Instance.AddListener<CurrencyChangeGameEvent>(OnCurrencyChange);
        EventManager.Instance.AddListener<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);
    }

    private void OnCurrencyChange(CurrencyChangeGameEvent info)
    {
        _currencyAmounts[info.CurrencyType] += info.Amount;

        if (_currencyAmounts[info.CurrencyType] < 0)
            _currencyAmounts[info.CurrencyType] = 0;

        SaveCurrency(info.CurrencyType, _currencyAmounts[info.CurrencyType]);

        _currencyTexts[info.CurrencyType].text = _currencyAmounts[info.CurrencyType].ToString("#,#", new CultureInfo("en-US"));
    }

    private void OnNotEnoughCurrency(NotEnoughCurrencyGameEvent info)
    {
        Debug.Log($"You dont have enough amount of {info.Amount} {info.CurrencyType}");
    }

    private void SaveCurrency(CurrencyType currencyType, int amount)
    {
        PlayerPrefs.SetInt(currencyType.ToString(), amount);
    }
}

public enum CurrencyType
{
    Coins,
    Bucks
}