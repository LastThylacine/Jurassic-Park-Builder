using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoneyObject : MonoBehaviour
{
    [SerializeField] private GameObject _notification;
    [SerializeField] private GameObject _tapVFX;
    [SerializeField] private GameObject _moneyCounter;
    [SerializeField] private MoneyCountDisplayer _moneyCountDisplayer;
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private AudioSource _tapSound;
    [SerializeField] private float _moneyPerSecond = 0.33f;
    [SerializeField] private CollectMoneyDisplay _collectMoneyDisplay;
    [SerializeField] private Button _collectMoneyButton;

    public float MaximumMoney = 100;
    public int CurrentMoneyInteger;
    public int MaximumMinutes = 5;

    private Selectable _selectable;
    private Paddock _paddock;
    private float _maximumSeconds;
    private int _currentSecond;
    private float _currentMoneyFloated;
    private float _timeFromLastMoneyAdding;

    private void Start()
    {
        _moneyManager = FindObjectOfType<MoneyManager>();
        _collectMoneyDisplay = FindObjectOfType<CollectMoneyDisplay>(true);
        _collectMoneyButton = FindObjectOfType<CollectMoneyButton>(true).GetComponent<Button>();

        _maximumSeconds = MaximumMinutes * 60;
        _collectMoneyButton.onClick.AddListener(GetMoneyIfAvaliableByButton);
        _selectable = GetComponent<Selectable>();
        if (GetComponent<Paddock>())
            _paddock = GetComponent<Paddock>();

        if (PlayerPrefs.HasKey("CurrentMoney" + gameObject.name))
        {
            CurrentMoneyInteger = PlayerPrefs.GetInt("CurrentMoney" + gameObject.name);
        }
        else
        {
            CurrentMoneyInteger = 0;
        }

        InitializeMoneyPerSecond();

        DateTime lastSaveTime = Utils.GetDateTime("LastSaveTime", DateTime.UtcNow);
        TimeSpan timePassed = DateTime.UtcNow - lastSaveTime;
        int secondsPassed = (int)timePassed.TotalSeconds;

        CurrentMoneyInteger += Mathf.FloorToInt(_moneyPerSecond * secondsPassed);

        _currentMoneyFloated = CurrentMoneyInteger;
    }

    private void Update()
    {
        if (CurrentMoneyInteger >= MaximumMoney)
        {
            _currentMoneyFloated = MaximumMoney;
            CurrentMoneyInteger = Mathf.FloorToInt(_currentMoneyFloated);
            _notification.SetActive(true);

            return;
        }

        if (_selectable.IsSelected)
        {
            _collectMoneyDisplay.Display(CurrentMoneyInteger);
        }

        _timeFromLastMoneyAdding += Time.deltaTime;

        if (_timeFromLastMoneyAdding >= 1)
        {
            _currentMoneyFloated += _moneyPerSecond;
            CurrentMoneyInteger = Mathf.FloorToInt(_currentMoneyFloated);
            PlayerPrefs.SetInt("CurrentMoney" + gameObject.name, CurrentMoneyInteger);
            Utils.SetDateTime("LastSaveTime", DateTime.UtcNow);
            _currentSecond++;

            _timeFromLastMoneyAdding = 0;
        }
    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GetMoneyIfAvaliable();
        }
    }

    public void InitializeMoneyPerSecond()
    {
        _moneyPerSecond = MaximumMoney / _maximumSeconds;
    }

    private void InitializeData()
    {

    }

    private void GetMoneyIfAvaliable()
    {
        GetMoney();
    }

    private void GetMoneyIfAvaliableByButton()
    {
        if (_selectable.IsSelected && !GridBuildingSystem.Current.TempGridBuilding)
        {
            if (_paddock)
            {
                _paddock.Select();
            }

            GetMoney();
        }
    }

    private void GetMoney()
    {
        if (CurrentMoneyInteger != 0 && !_moneyCounter.activeInHierarchy && !GridBuildingSystem.Current.TempGridBuilding)
        {
            _notification.SetActive(false);
            _tapVFX.SetActive(true);
            _moneyCounter.SetActive(true);
            _moneyCountDisplayer.DisplayCount(CurrentMoneyInteger);
            _moneyManager.AddMoney(CurrentMoneyInteger);
            _currentMoneyFloated = 0;
            CurrentMoneyInteger = 0;
            _tapSound.Play();
        }
    }
}