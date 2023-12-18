using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerOverUIChecker : MonoBehaviour
{
    public static PointerOverUIChecker Current;

    public static PanZoomMobile _panZoomMobile;

    private void Awake()
    {
        _panZoomMobile = FindObjectOfType<PanZoomMobile>();
    }

    private void Start()
    {
        Current = this;
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
