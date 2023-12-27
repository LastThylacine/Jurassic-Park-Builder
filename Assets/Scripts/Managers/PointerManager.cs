using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerManager : MonoBehaviour
{
	public static PointerManager Instance { get; private set; }

	private void Start()
	{
		Instance = this;
	}

	private void OnMouseDown()
	{
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			// check for ui first, then colliders
			// IsMouseOverSelectableUI()
			if (IsMouseOverSelectableCollider(out ISelectable selected))
				selected.Select();
		}
	}

	public static bool IsMouseOverSelectableCollider(out ISelectable selectable)
	{
		RaycastHit2D[] results = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		List<ISelectable> selectableResults = results.SelectMany(FilterRaycastsToISelectable).ToList();

		selectable = selectableResults.Count > 0 ? selectableResults.First() : null;
		return selectableResults.Count > 0;
	}

	private static ISelectable[] FilterRaycastsToISelectable(RaycastHit2D raycast)
	{
		Debug.Log(raycast.collider.gameObject.name);
		if (raycast.collider.gameObject.GetComponent<ISelectable>() is ISelectable selectable)
			return new ISelectable[] { selectable };
		return new ISelectable[0];
	}
}