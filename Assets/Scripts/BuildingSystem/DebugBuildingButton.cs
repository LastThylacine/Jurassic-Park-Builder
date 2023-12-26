using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugBuildingButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    void Update()
    {
        _button.interactable = !GridBuildingSystem.Current.TempPlaceableObject;
    }
}
