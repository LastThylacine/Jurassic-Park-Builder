using UnityEngine;

public class SelectablesManager : MonoBehaviour
{
    public static SelectablesManager Current;

    [SerializeField] private bool _isSomethingSelected;
    [SerializeField] private GameObject[] _paddockSelectedUI;
    [SerializeField] private GameObject[] _buildingSelectedUI;
    [SerializeField] private GameObject[] _nothingIsSelected;
    [SerializeField] private Selectable[] _selectables;

    private Selectable _currentSelectable;

    private void Start()
    {
        Current = this;

        CheckForSelectables();

        UnselectAll();

        InitializeUI();
    }

    public void CheckForSelectables()
    {
        _selectables = FindObjectsOfType<Selectable>();
    }

    public void SetIsSomethingSelected(bool isSomethingSelected)
    {
        _isSomethingSelected = isSomethingSelected;

        FindCurrentSelectable();

        InitializeUI();
    }

    private void FindCurrentSelectable()
    {
        for (int i = 0; i < _selectables.Length; i++)
        {
            if (_selectables[i].IsSelected)
                _currentSelectable = _selectables[i];
        }
    }

    public void UnselectAll()
    {
        for (int i = 0; i < _selectables.Length; i++)
        {
            _selectables[i].Unselect();
        }
    }

    public void InitializeUI()
    {
        if (GridBuildingSystem.Current.TempGridBuilding || !_currentSelectable)
            return;

        if (_isSomethingSelected)
        {
            if (_currentSelectable.GetComponent<Paddock>())
            {
                for (int i = 0; i < _buildingSelectedUI.Length; i++)
                {
                    _buildingSelectedUI[i].SetActive(false);
                }

                for (int i = 0; i < _nothingIsSelected.Length; i++)
                {
                    _nothingIsSelected[i].SetActive(false);
                }

                for (int i = 0; i < _paddockSelectedUI.Length; i++)
                {
                    _paddockSelectedUI[i].SetActive(true);
                }
            }
            else if (_currentSelectable.GetComponent<Building>())
            {
                for (int i = 0; i < _paddockSelectedUI.Length; i++)
                {
                    _paddockSelectedUI[i].SetActive(false);
                }

                for (int i = 0; i < _nothingIsSelected.Length; i++)
                {
                    _nothingIsSelected[i].SetActive(false);
                }

                for (int i = 0; i < _buildingSelectedUI.Length; i++)
                {
                    _buildingSelectedUI[i].SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < _paddockSelectedUI.Length; i++)
            {
                _paddockSelectedUI[i].SetActive(false);
            }

            for (int i = 0; i < _buildingSelectedUI.Length; i++)
            {
                _buildingSelectedUI[i].SetActive(false);
            }

            for (int i = 0; i < _nothingIsSelected.Length; i++)
            {
                _nothingIsSelected[i].SetActive(true);
            }
        }
    }
}

public enum SelectableType
{
    Paddock,
    Building
}