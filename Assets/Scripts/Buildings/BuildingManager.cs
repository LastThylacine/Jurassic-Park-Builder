using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Building[] _buildings;
    [SerializeField] private bool _isBuildingSelected;
    [SerializeField] private GameObject[] _buildingSelectedUI;
    [SerializeField] private GameObject[] _buildingIsNotSelectedUI;

    private void Start()
    {
        InitializeUI();
    }

    public void SetIsBuildingSelected(bool isBuildingSelected)
    {
        _isBuildingSelected = isBuildingSelected;

        InitializeUI();
    }

    public void InitializeUI()
    {
        if (_isBuildingSelected)
        {
            for (int i = 0; i < _buildingSelectedUI.Length; i++)
            {
                _buildingSelectedUI[i].SetActive(true);
            }

            for (int i = 0; i < _buildingIsNotSelectedUI.Length; i++)
            {
                _buildingIsNotSelectedUI[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _buildingSelectedUI.Length; i++)
            {
                _buildingSelectedUI[i].SetActive(false);
            }

            for (int i = 0; i < _buildingIsNotSelectedUI.Length; i++)
            {
                _buildingIsNotSelectedUI[i].SetActive(true);
            }
        }
    }

    public void UnselectAll()
    {
        for (int i = 0; i < _buildings.Length; i++)
        {
            _buildings[i].Deselect();
        }
    }
}
