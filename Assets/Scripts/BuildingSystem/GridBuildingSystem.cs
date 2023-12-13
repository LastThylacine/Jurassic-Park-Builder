using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Current;

    public GridLayout GridLayout;

    [SerializeField] private GameObject[] _buildingUI;
    [SerializeField] private GameObject[] _notBuildingUI;
    [SerializeField] private GameObject[] _defaultUI;
    [SerializeField] private Tilemap _mainTilemap;
    [SerializeField] private Tilemap _tempTilemap;
    [SerializeField] private Camera _camera;
    [SerializeField] private string _highestLayerName;
    [SerializeField] private Color _gridBuildingColor;
    [SerializeField] private int _placedObjectsAmount;
    
    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public GridBuilding TempGridBuilding { get; private set; }

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
        _tempTilemap.gameObject.SetActive(TempGridBuilding);
        _mainTilemap.gameObject.SetActive(TempGridBuilding);

        //if (!TempGridBuilding)
        //{
        //    return;
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        //if (!TempGridBuilding.Placed)
        //{
        //Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3Int cellPosition = GridLayout.LocalToCell(touchPosition);

        //if (_prevPosition != cellPosition)
        //{
        //    TempGridBuilding.transform.localPosition = GridLayout.CellToLocalInterpolated(cellPosition
        //        + new Vector3(0.5f, 0.5f, 0f));
        //    _prevPosition = cellPosition;
        //    FollowBuilding();
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (TempGridBuilding.CanBePlaced())
        //    {
        //        TempGridBuilding.Place();
        //        TempGridBuilding = null;
        //        ReloadUI();
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    ClearArea();
        //    Destroy(TempGridBuilding.gameObject);
        //    ReloadUI();
        //}
        //}
        //}
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

    #endregion

    #region Building Placement

    public void InitializeWithBuilding(GameObject building)
    {
        TempGridBuilding = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<GridBuilding>();
        TempGridBuilding.InitializeGridBuilding();
        ReloadUI();

        Vector3 screenMiddlePoint = _camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3Int cellPosition = GridLayout.WorldToCell(screenMiddlePoint);

        TempGridBuilding.transform.localPosition = GridLayout.CellToLocalInterpolated(new Vector3(cellPosition.x, cellPosition.y, 0f));
        _prevPosition = cellPosition;
        FollowBuilding();
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[_prevArea.size.x * _prevArea.size.y * _prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        _tempTilemap.SetTilesBlock(_prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        TempGridBuilding.Area.position = GridLayout.WorldToCell(TempGridBuilding.gameObject.transform.position);
        BoundsInt buildingArea = TempGridBuilding.Area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, _mainTilemap);

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

        _tempTilemap.SetTilesBlock(buildingArea, tileArray);
        _prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, _mainTilemap);
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
        SetTilesBlock(area, TileType.Empty, _tempTilemap);
        SetTilesBlock(area, TileType.Green, _mainTilemap);
    }

    #endregion

    #region Building Movement

    public void SaveOffset()
    {
        if (!TempGridBuilding)
            return;

        _startTouchPosition = Input.mousePosition;
        _startTouchPosition = Camera.main.ScreenToWorldPoint(_startTouchPosition);

        _deltaX = _startTouchPosition.x - TempGridBuilding.transform.position.x;
        _deltaY = _startTouchPosition.y - TempGridBuilding.transform.position.y;
    }

    public void MoveGridBuildingWithOffset()
    {
        if (!TempGridBuilding)
            return;

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = new Vector3(touchPosition.x - _deltaX, touchPosition.y - _deltaY);

        Vector3Int cellPosition = GridLayout.WorldToCell(position);

        if (_prevPosition != cellPosition)
        {
            TempGridBuilding.transform.localPosition = GridLayout.CellToLocalInterpolated(cellPosition);
            _prevPosition = cellPosition;
            FollowBuilding();
        }
    }

    #endregion

    #region Interface

    private void ReloadUI()
    {
        if (TempGridBuilding)
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
        if (!TempGridBuilding)
            return;

        if (!TempGridBuilding.Placed)
        {
            ClearArea();
            Destroy(TempGridBuilding.gameObject);
            TempGridBuilding = null;
            ReloadUI();
        }
    }

    public void Accept()
    {
        if (!TempGridBuilding)
            return;

        if (!TempGridBuilding.Placed)
        {
            if (TempGridBuilding.CanBePlaced())
            {
                _placedObjectsAmount++;
                TempGridBuilding.Place(_placedObjectsAmount);
                TempGridBuilding = null;
                ReloadUI();
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