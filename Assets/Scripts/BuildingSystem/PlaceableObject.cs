using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed;

    public BoundsInt Area;
    public int GridBuildingID;

    [ReadOnly()] public PlaceableObjectData data = new PlaceableObjectData();

    [HideInInspector] public FadeInOut DisplayFadeInOut;

    [SerializeField] private GameObject _display;
    [SerializeField] private GameObject _main;

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

    private PlaceableObjectItem _placeableObjectItem;
    private Selectable _selectable;
    private Button _editButton;
    private Vector3 _origin;
    private bool _placedFromBeginning = false;

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
                Place();
                _placedFromBeginning = true;
            }
        }

        _editButton = FindObjectOfType<EditButton>(true).GetComponent<Button>();
        _editButton.onClick.AddListener(StartEditing);

        _selectable = _main.GetComponent<Selectable>();
    }

    private void OnApplicationQuit()
    {
        if (!_placedFromBeginning)
        {
            if (Placed)
            {
                data.Position = transform.position;
                SaveManager.Current.saveData.AddData(data);
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

    public void Place()
    {
        InitializeDisplayObjects(false);

        if (!Placed)
            _main.GetComponent<Selectable>().PlayPlacementSound();

        Vector3Int positionInt = GridBuildingSystem.Current.GridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = Area;
        areaTemp.position = positionInt;
        Placed = true;

        transform.position = GridBuildingSystem.Current.GridLayout.CellToLocalInterpolated(positionInt);

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
        _main.SetActive(!isBuildingEnabled);
    }

    public void Initialize(PlaceableObjectItem placeableObjectItem)
    {
        _placeableObjectItem = placeableObjectItem;
        data.ItemName = placeableObjectItem.name;
        data.ID = SaveData.GenerateId();
        _main.name = _main.name + data.ID;
    }

    public void Initialize(PlaceableObjectItem placeableObjectItem, PlaceableObjectData placeableObjectData)
    {
        _placeableObjectItem = placeableObjectItem;
        data = placeableObjectData;
        _main.name = _main.name + data.ID;
        transform.position = data.Position;
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
        Place();
    }

    #endregion
}
