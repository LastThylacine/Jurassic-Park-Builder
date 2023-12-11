using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingInfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyPerTimeText;
    [SerializeField] private TextMeshProUGUI _nameText;

    public void Display(string buildingName, int maximumMinutes, float maximumMoney)
    {
        _moneyPerTimeText.text = "<sprite=0>" + maximumMoney + " / " + maximumMinutes + " min";
        _nameText.text = buildingName;
    }
}
