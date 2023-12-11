using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    [SerializeField] private string _buildingName;
    [SerializeField] private int _maximumMinutes;
    [SerializeField] private float _maximumMoney;

    private MoneyObject _moneyObject;
    private Building _building;
    private BuildingInfoDisplay _display;

    private void Start()
    {
        _display = FindObjectOfType<BuildingInfoDisplay>(true);
        _moneyObject = GetComponent<MoneyObject>();
        _building = GetComponent<Building>();
    }

    private void Update()
    {
        if (!_building.IsSelected)
            return;

        _maximumMinutes = _moneyObject.MaximumMinutes;
        _maximumMoney = _moneyObject.MaximumMoney;

        _display.Display(_buildingName, _maximumMinutes, _maximumMoney);
    }
}
