using UnityEngine;

public class PlaceableObjectMovementListener : MonoBehaviour
{
    public bool IsMoving;

    private PlaceableStructure _placeableObject;
    private bool _isCanMove;

    private void Start()
    {
        _placeableObject = GetComponentInParent<PlaceableStructure>();
    }

    private void OnMouseDown()
    {
        if (GridManager.Instance.TempPlaceableObject != _placeableObject || PointerOverUIChecker.Current.IsPointerOverUIObject())
        {
            _isCanMove = false;
            return;
        }

        GridManager.Instance.SaveObjectOffset();
        _isCanMove = true;
    }

    private void OnMouseDrag()
    {
        if (!_isCanMove)
            return;

        GridManager.Instance.MoveObjectWithOffset();
        IsMoving = true;
    }

    private void OnMouseUp()
    {
        if (!_isCanMove)
            return;

        IsMoving = false;
    }
}
