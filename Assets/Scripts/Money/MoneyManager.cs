using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int _defaulMoneyAmount = 1000;

    private void Start()
    {
        if (PlayerPrefs.HasKey(CurrencyType.Coins.ToString()))
        {
            AddCoins(PlayerPrefs.GetInt(CurrencyType.Coins.ToString()));
        }
        else
        {
            AddCoins(_defaulMoneyAmount);
        }

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
