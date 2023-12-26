using UnityEngine;
using UnityEngine.UI;

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

    private Selectable _selectable;
    private Button _editButton;
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

        _editButton = FindObjectOfType<EditButton>(true).GetComponent<Button>();
        _editButton.onClick.AddListener(StartEditing);

        _selectable = _paddock.GetComponent<Selectable>();
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
        InitializeDisplayObjects(false);

        if (!Placed)
            _paddock.GetComponent<Selectable>().PlayPlacementSound();

        Vector3Int positionInt = GridBuildingSystem.Current.GridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = Area;
        areaTemp.position = positionInt;
        Placed = true;

        transform.position = GridBuildingSystem.Current.GridLayout.CellToLocalInterpolated(positionInt);

        if (GridBuildingID != gridBuildingID)
        {
            GridBuildingID = gridBuildingID;

            _paddock.name = _paddock.name + GridBuildingID;
        }
        GridBuildingSystem.Current.TakeArea(areaTemp);
        SelectablesManager.Current.CheckForSelectables();

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
        if (_selectable.IsSelected)
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
    }

    public void CancelEditing()
    {
        transform.position = _origin;
        Place(GridBuildingID);
    }

    #endregion
}
