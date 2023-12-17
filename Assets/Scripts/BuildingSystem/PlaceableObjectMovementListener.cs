using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlaceableObjectMovementListener : MonoBehaviour
{
    private PlaceableObject _placeableObject;

    private void Start()
    {
        _placeableObject = GetComponentInParent<PlaceableObject>();
    }

    private void OnMouseDown()
    {
        if(GridBuildingSystem.Current.TempPlaceableObject != _placeableObject)
        {
            return;
        }

        GridBuildingSystem.Current.SaveOffset();
    }

    private void OnMouseDrag()
    {
        if (GridBuildingSystem.Current.TempPlaceableObject != _placeableObject)
        {
            return;
        }

        GridBuildingSystem.Current.MoveGridBuildingWithOffset();
    }
}
