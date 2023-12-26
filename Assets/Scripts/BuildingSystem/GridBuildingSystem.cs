using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Current;

    public GridLayout GridLayout;
    public int PlacedObjectsAmount;

    [SerializeField] private GameObject[] _buildingUI;
    [SerializeField] private GameObject[] _notBuildingUI;
    [SerializeField] private GameObject[] _defaultUI;
    [SerializeField] private Camera _camera;
    [SerializeField] private string _highestLayerName;

    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    [HideInInspector] public PlaceableObject TempPlaceableObject;

    private Vector3 _startTouchPosition;
    private float _deltaX, _deltaY;

    private Vector3 _prevPosition;
    private BoundsInt _prevArea;

    #region Unity Methods

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));

        ReloadUI();
    }

    private void Update()
    {
        if (!TempPlaceableObject)
            return;

        if (TempPlaceableObject.DisplayFadeInOut != null)
            TempPlaceableObject.DisplayFadeInOut.SetFade(TempPlaceableObject && TempPlaceableObject.CanBePlaced());
    }

    #endregion

    #region Tilemap Management

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int position = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(position);
            counter++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] array, TileType type)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = tileBases[type];
        }
    }

    public void SetAreaWhite(BoundsInt area, Tilemap tilemap)
    {
        SetTilesBlock(area, TileType.White, tilemap);
    }

    #endregion

    #region Building Placement

    public void InitializeWithBuilding(GameObject building)
    {
        TempPlaceableObject = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<PlaceableObject>();
        TempPlaceableObject.InitializeDisplayObjects(true);
        ReloadUI();

        Vector3 screenMiddlePoint = _camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3Int cellPosition = GridLayout.WorldToCell(screenMiddlePoint);

        TempPlaceableObject.transform.localPosition = GridLayout.CellToLocalInterpolated(new Vector3(cellPosition.x, cellPosition.y, 0f));
        _prevPosition = cellPosition;
        FollowBuilding();

        CameraObjectFollowing.Current.SetTarget(TempPlaceableObject.transform);
        TempTilemap.gameObject.SetActive(true);
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[_prevArea.size.x * _prevArea.size.y * _prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(_prevArea, toClear);
    }

    public void FollowBuilding()
    {
        ClearArea();

        TempPlaceableObject.Area.position = GridLayout.WorldToCell(TempPlaceableObject.transform.position);
        BoundsInt buildingArea = TempPlaceableObject.Area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        _prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var tileBase in baseArray)
        {
            if (tileBase != tileBases[TileType.White])
            {
                Debug.Log("Cannot place here");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }

    public void IncreacePlacedObjectsAmount()
    {
        PlacedObjectsAmount++;
    }

    #endregion

    #region Building Movement

    public void SaveObjectOffset()
    {
        if (!TempPlaceableObject)
            return;

        _startTouchPosition = Input.mousePosition;
        _startTouchPosition = Camera.main.ScreenToWorldPoint(_startTouchPosition);

        _deltaX = _startTouchPosition.x - TempPlaceableObject.transform.position.x;
        _deltaY = _startTouchPosition.y - TempPlaceableObject.transform.position.y;
    }

    public void MoveObjectWithOffset()
    {
        if (!TempPlaceableObject)
            return;

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = new Vector3(touchPosition.x - _deltaX, touchPosition.y - _deltaY);

        Vector3Int cellPosition = GridLayout.WorldToCell(position);

        if (_prevPosition != cellPosition)
        {
            TempPlaceableObject.transform.position = GridLayout.CellToLocalInterpolated(cellPosition);
            _prevPosition = cellPosition;
            FollowBuilding();
        }
    }

    #endregion

    #region Interface

    public void ReloadUI()
    {
        if (TempPlaceableObject)
        {
            for (int i = 0; i < _notBuildingUI.Length; i++)
            {
                _notBuildingUI[i].SetActive(false);
            }

            for (int i = 0; i < _buildingUI.Length; i++)
            {
                _buildingUI[i].SetActive(true);
            }

            SelectablesManager.Current.UnselectAll();
        }
        else
        {
            for (int i = 0; i < _buildingUI.Length; i++)
            {
                _buildingUI[i].SetActive(false);
            }

            for (int i = 0; i < _defaultUI.Length; i++)
            {
                _defaultUI[i].SetActive(true);
            }
        }
    }

    #endregion

    #region Building Ending Buttons

    public void Decline()
    {
        if (!TempPlaceableObject)
            return;

        if (!TempPlaceableObject.Placed)
        {
            ClearArea();
            Destroy(TempPlaceableObject.gameObject);
            TempPlaceableObject = null;
            ReloadUI();
            TempTilemap.gameObject.SetActive(false);
        }
        else
        {
            TempPlaceableObject.CancelEditing();
            ClearArea();
            TempPlaceableObject = null;
            ReloadUI();
            TempTilemap.gameObject.SetActive(false);
        }
    }

    public void Accept()
    {
        if (!TempPlaceableObject)
            return;

        if (!TempPlaceableObject.Placed)
        {
            if (TempPlaceableObject.CanBePlaced())
            {
                IncreacePlacedObjectsAmount();
                TempPlaceableObject.Place(PlacedObjectsAmount);
                TempPlaceableObject = null;
                ReloadUI();
                TempTilemap.gameObject.SetActive(false);
            }
        }
        else
        {
            if (TempPlaceableObject.CanBePlaced())
            {
                TempPlaceableObject.Place(TempPlaceableObject.GridBuildingID);
                TempPlaceableObject = null;
                ReloadUI();
                TempTilemap.gameObject.SetActive(false);
            }
        }
    }

    #endregion
}

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}