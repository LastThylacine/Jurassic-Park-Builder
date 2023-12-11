using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class GridBuildingMovementListener : MonoBehaviour
{
    private GridBuilding _gridBuilding;

    private void Start()
    {
        _gridBuilding = GetComponentInParent<GridBuilding>();
    }

    private void OnMouseDown()
    {
        if(GridBuildingSystem.Current.TempGridBuilding != _gridBuilding)
        {
            return;
        }

        GridBuildingSystem.Current.SaveOffset();
    }

    private void OnMouseDrag()
    {
        if (GridBuildingSystem.Current.TempGridBuilding != _gridBuilding)
        {
            return;
        }

        GridBuildingSystem.Current.MoveGridBuildingWithOffset();
    }
}
