using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaddockInfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _moneyPerTimeText;
    [SerializeField] private TextMeshProUGUI _nameText;

    public void Display(string dinosaurName, int currentLevel, int maximumMinutes, float maximumMoney)
    {
        _levelText.text = "LVL " + currentLevel;
        _moneyPerTimeText.text = "<sprite=0>" + maximumMoney + " / " + maximumMinutes + " min";
        _nameText.text = dinosaurName;
    }
}
