using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugBuildingButton : MonoBehaviour
{
    private Button _button;

    public PlaceableObjectItem PlaceableObjectItem;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(CreateObject);
    }

    void Update()
    {
        _button.interactable = !GridBuildingSystem.Current.TempPlaceableObject;
    }

    private void CreateObject()
    {
        var obj = GridBuildingSystem.Current.InitializeWithBuilding(PlaceableObjectItem.Prefab);
        obj.GetComponent<PlaceableObject>().Initialize(PlaceableObjectItem);
    }
}
