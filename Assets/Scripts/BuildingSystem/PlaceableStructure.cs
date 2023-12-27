using UnityEngine;

public class PlaceableStructure : Placeable
{
	public void Place()
	{
		//if (!placed)
		//	_paddock.GetComponent<Selectable>().PlayPlacementSound();

		//Vector3Int position = GridBuildingSystem.Instance.gridLayout.LocalToCell(Position);
		BoundsInt areaTemp = Area;
		//areaTemp.position = ;

		//transform.position = GridBuildingSystem.Instance.gridLayout.CellToLocal(position);
		

		//GridBuildingSystem.Instance.TakeArea(areaTemp);
		//SelectablesManager.Current.CheckForSelectables();
		//CameraObjectFollowing.Current.SetTarget(null);
	}

	public void StartEditing()
	{
		//if (_selectable.IsSelected)
		//{
			CameraObjectFollowing.Current.SetTarget(transform);

			//Vector3Int positionInt = GridBuildingSystem.Instance.GridLayout.WorldToCell(transform.position);
			//BoundsInt areaTemp = area;
			//areaTemp.position = positionInt;

			//GridBuildingSystem.Instance.SetAreaWhite(areaTemp, GridBuildingSystem.Instance.MainTilemap);

			//GridBuildingSystem.Instance.FollowPlaceable(this);
			//GridBuildingSystem.Instance.ReloadGrid();
		//}
	}

	public void CancelEditing()
	{
		//transform.position = position;
		//Place(GridBuildingID);
	}
}
