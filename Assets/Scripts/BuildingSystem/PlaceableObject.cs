using UnityEngine;
using UnityEngine.Rendering;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }

    public BoundsInt Area;

    [HideInInspector] public FadeInOut DisplayFadeInOut;

    [SerializeField] private GameObject _display;
    [SerializeField] private GameObject _paddock;

    #region Unity Methods

    private void Awake()
    {
        DisplayFadeInOut = _display.GetComponent<FadeInOut>();
    }

    #endregion

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

        _paddock.GetComponent<Selectable>().PlayPlacementSound();
    }

    #endregion

    #region Initialization Methods

    public void InitializeGridBuilding()
    {
        _display.SetActive(true);
        _paddock.SetActive(false);
    }

    public void PlaceGridBuildingParameters(int gridBuildingID)
    {
        _paddock.name = _paddock.name + gridBuildingID;

        _display.SetActive(false);
        _paddock.SetActive(true);
    }

    #endregion
}
