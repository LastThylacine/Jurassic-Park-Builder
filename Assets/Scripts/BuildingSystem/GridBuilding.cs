using UnityEngine;
using UnityEngine.Rendering;

public class GridBuilding : MonoBehaviour
{
    public bool Placed { get; private set; }

    public BoundsInt Area;

    [SerializeField] private GameObject _gridBuildingSpriteObject;
    [SerializeField] private GameObject _selectableObject;

    #region Build Methods

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.Current.GridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = Area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.Current.CanTakeArea(areaTemp))
        {
            return true;
        }

        return false;
    }

    public void Place(int gridBuildingID)
    {
        Vector3Int positionInt = GridBuildingSystem.Current.GridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = Area;
        areaTemp.position = positionInt;
        Placed = true;

        PlaceGridBuildingParameters(gridBuildingID);
        GridBuildingSystem.Current.TakeArea(areaTemp);
        SelectablesManager.Current.CheckForSelectables();

        _selectableObject.GetComponent<Selectable>().PlayPlacementSound();
    }

    #endregion

    #region Initialization Methods

    public void InitializeGridBuilding()
    {
        _gridBuildingSpriteObject.SetActive(true);
        _selectableObject.SetActive(false);
    }

    public void PlaceGridBuildingParameters(int gridBuildingID)
    {
        _selectableObject.name = _selectableObject.name + gridBuildingID;

        _gridBuildingSpriteObject.SetActive(false);
        _selectableObject.SetActive(true);
    }

    #endregion
}
