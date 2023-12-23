using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed;

    public BoundsInt Area;
    public int GridBuildingID;

    [HideInInspector] public FadeInOut DisplayFadeInOut;

    [SerializeField] private GameObject _display;
    [SerializeField] private GameObject _paddock;

    public GameObject Display
    {
        get
        {
            return _display;
        }
        set
        {
            _display = value;
        }
    }

    private Vector3 _origin;

    #region Unity Methods

    private void Awake()
    {
        DisplayFadeInOut = _display.GetComponent<FadeInOut>();
    }

    private void Start()
    {
        if (Placed)
        {
            if (CanBePlaced())
            {
                GridBuildingSystem.Current.IncreacePlacedObjectsAmount();
                Place(GridBuildingSystem.Current.PlacedObjectsAmount);
            }
        }
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

        if (GridBuildingID != gridBuildingID)
        {
            GridBuildingID = gridBuildingID;

            _paddock.name = _paddock.name + GridBuildingID;
        }

        InitializeDisplayObjects(false);

        GridBuildingSystem.Current.TakeArea(areaTemp);
        SelectablesManager.Current.CheckForSelectables();

        _paddock.GetComponent<Selectable>().PlayPlacementSound();
        _origin = transform.position;

        CameraObjectFollowing.Current.SetTarget(null);
    }

    #endregion

    #region Initialization Methods

    public void InitializeDisplayObjects(bool isBuildingEnabled)
    {
        _display.SetActive(isBuildingEnabled);
        _paddock.SetActive(!isBuildingEnabled);
    }

    #endregion

    #region Editing Mode

    public void StartEditing()
    {
        GridBuildingSystem.Current.TempPlaceableObject = this;
        InitializeDisplayObjects(true);
        CameraObjectFollowing.Current.SetTarget(transform);
        GridBuildingSystem.Current.TempTilemap.gameObject.SetActive(true);

        Vector3Int positionInt = GridBuildingSystem.Current.GridLayout.WorldToCell(transform.position);
        BoundsInt areaTemp = Area;
        areaTemp.position = positionInt;

        GridBuildingSystem.Current.SetAreaWhite(areaTemp, GridBuildingSystem.Current.MainTilemap);

        GridBuildingSystem.Current.FollowBuilding();
        GridBuildingSystem.Current.ReloadUI();
    }

    public void CancelEditing()
    {
        transform.position = _origin;
        Place(GridBuildingID);
    }

    #endregion
}
