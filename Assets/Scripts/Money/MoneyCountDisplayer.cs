using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCountDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void DisplayCount(int moneyCount)
    {
        _text.text = moneyCount.ToString();
    }
}