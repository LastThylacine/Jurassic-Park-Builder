using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlaceableObjectMovementListener : MonoBehaviour
{
    public bool IsMoving;

    private PlaceableObject _placeableObject;
    private bool _isCanMove;

    private void Start()
    {
        _placeableObject = GetComponentInParent<PlaceableObject>();
    }

    private void OnMouseDown()
    {
        if (GridBuildingSystem.Current.TempPlaceableObject != _placeableObject || PointerOverUIChecker.Current.IsPointerOverUIObject())
        {
            _isCanMove = false;
            return;
        }

        GridBuildingSystem.Current.SaveObjectOffset();
        _isCanMove = true;
    }

    private void OnMouseDrag()
    {
        if (!_isCanMove)
            return;

        GridBuildingSystem.Current.MoveObjectWithOffset();
        IsMoving = true;
    }

    private void OnMouseUp()
    {
        if (!_isCanMove)
            return;

        IsMoving = false;
    }
}
