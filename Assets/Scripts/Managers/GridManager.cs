using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
	White, // Free
	Green, // Valid Placement
	Red, // Taken
}

public class GridManager : MonoBehaviour
{
	public static GridManager Instance { get; private set; }

	private static readonly Dictionary<TileType, TileBase> tiles = new Dictionary<TileType, TileBase>();

	private GridLayout gridLayout;
	private Tilemap logicalTilemap = new Tilemap();
	private Tilemap viewTilemap = new Tilemap();

	[HideInInspector] public PlaceableStructure TempPlaceableObject;

	private Vector3 _startTouchPosition;
	private float _deltaX, _deltaY;

	#region Unity Methods

	private void Start()
	{
		Instance = this;

		string tilePath = @"Tiles\";
		foreach (TileType type in Enum.GetValues(typeof(TileType)))
			tiles.Add(type, Resources.Load<TileBase>(tilePath + $"{nameof(type)}Tile"));
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

	#endregion

	#region Building Placement

	public void FollowPlaceable(Placeable placeable)
	{
		//ClearArea();

		//TempPlaceableObject.area.position = GridLayout.WorldToCell(TempPlaceableObject.transform.position);
		//BoundsInt buildingArea = TempPlaceableObject.area;

		//TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

		//int size = baseArray.Length;
		//TileBase[] tileArray = new TileBase[size];

		//for (int i = 0; i < baseArray.Length; i++)
		//{
		//    if (baseArray[i] == tiles[TileType.White])
		//    {
		//        tileArray[i] = tiles[TileType.Green];
		//    }
		//    else
		//    {
		//        FillTiles(tileArray, TileType.Red);
		//        break;
		//    }
		//}

		//TempTilemap.SetTilesBlock(buildingArea, tileArray);
		//_prevArea = buildingArea;
	}

	public bool CanTakeArea(Placeable placeable)
	{
		TileBase[] baseArray = GetTilesBlock(placeable.Area, logicalTilemap);
		foreach (var tileBase in baseArray)
			if (tileBase != tiles[TileType.White])
				return false;
		
		return true;
	}

	public void MarkAreaWhite(Tilemap tilemap, BoundsInt area)
		=> MarkArea(tilemap, area, TileType.White);

	private void MarkArea(Tilemap tilemap, BoundsInt area, TileType marker)
	{
		int size = area.size.x * area.size.y * area.size.z;
		tilemap.SetTilesBlock(area, Enumerable.Repeat(tiles[marker], size).ToArray());
	}

	#endregion

	#region Building Movement

	public void SaveObjectOffset()
	{
		_startTouchPosition = Input.mousePosition;
		_startTouchPosition = Camera.main.ScreenToWorldPoint(_startTouchPosition);

		//_deltaX = _startTouchPosition.x - TempPlaceableObject.transform.position.x;
		//_deltaY = _startTouchPosition.y - TempPlaceableObject.transform.position.y;
	}

	public void MoveObjectWithOffset()
	{
		Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 position = new Vector3(touchPosition.x - _deltaX, touchPosition.y - _deltaY);
		Vector3Int cellPosition = gridLayout.WorldToCell(position);
	}

	#endregion

	#region Building Ending Buttons
	public void Accept()
	{
		//if (!TempPlaceableObject)
		//	return;

		//if (!TempPlaceableObject.placed)
		//{
		//	if (TempPlaceableObject.CanBePlaced())
		//	{
		//		IncreacePlacedObjectsAmount();
		//		TempPlaceableObject.Place(PlacedObjectsAmount);
		//		TempPlaceableObject = null;
		//		ReloadGrid();
		//		TempTilemap.gameObject.SetActive(false);
		//	}
		//}
		//else
		//{
		//	if (TempPlaceableObject.CanBePlaced())
		//	{
		//		TempPlaceableObject.Place(TempPlaceableObject.GridBuildingID);
		//		ReloadGrid();
		//		TempTilemap.gameObject.SetActive(false);
		//	}
		//}
	}
	public void Decline()
	{
		//if (!TempPlaceableObject)
		//	return;

		//if (!TempPlaceableObject.placed)
		//{
		//	ClearArea();
		//	Destroy(TempPlaceableObject.gameObject);
		//	TempPlaceableObject = null;
		//	ReloadGrid();
		//	TempTilemap.gameObject.SetActive(false);
		//}
		//else
		//{
		//	TempPlaceableObject.CancelEditing();
		//	ClearArea();
		//	TempPlaceableObject = null;
		//	ReloadGrid();
		//	TempTilemap.gameObject.SetActive(false);
		//}
	}
	#endregion
}