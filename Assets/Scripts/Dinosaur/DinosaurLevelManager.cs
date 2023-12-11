using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurLevelManager : MonoBehaviour
{
    [SerializeField] private int _currentMaximumMoneyForTime;

    private MoneyObject _moneyObject;
    private DinosaurLevelResourcesManager _dinosaurLevelResourcesManager;

    public int CurrentLevel;

    private void Start()
    {
        _moneyObject = GetComponent<MoneyObject>();
        _dinosaurLevelResourcesManager = GetComponent<DinosaurLevelResourcesManager>();

        Initialize();
    }

    public void LevelUp()
    {
        CurrentLevel++;

        Initialize();
    }

    public void SetLevel(int level)
    {
        CurrentLevel = level;

        Initialize();
    }

    public void Initialize()
    {
        _currentMaximumMoneyForTime = _dinosaurLevelResourcesManager.GetMaximumMoneyByLevel(CurrentLevel - 1);

        _moneyObject.MaximumMoney = _currentMaximumMoneyForTime;

        _moneyObject.InitializeMoneyPerSecond();
    }
}
