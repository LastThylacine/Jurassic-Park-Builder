using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaddockInfo : MonoBehaviour
{
    [SerializeField] private string _dinosaurName;
    [SerializeField] private int _currentLevel;
    [SerializeField] private int _maximumMinutes;
    [SerializeField] private float _maximumMoney;

    private DinosaurLevelManager _dinosaurLevelManager;
    private MoneyObject _moneyObject;
    private Paddock _paddock;
    private PaddockInfoDisplay _display;

    private void Start()
    {
        _display = FindObjectOfType<PaddockInfoDisplay>(true);
        _dinosaurLevelManager = GetComponent<DinosaurLevelManager>();
        _moneyObject = GetComponent<MoneyObject>();
        _paddock = GetComponent<Paddock>();
    }

    private void Update()
    {
		if (!_paddock.IsSelected)
            return;

        _currentLevel = _dinosaurLevelManager.CurrentLevel;
        _maximumMinutes = _moneyObject.MaximumMinutes;
        _maximumMoney = _moneyObject.MaximumMoney;

        _display.Display(_dinosaurName, _currentLevel, _maximumMinutes, _maximumMoney);
    }
}
