using TMPro;
using UnityEngine;

public class CollectMoneyDisplay : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Display(int money)
    {
        if (_text)
            _text.text = "<sprite index=0>" + money;
    }
}
