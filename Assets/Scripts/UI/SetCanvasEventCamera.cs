using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasEventCamera : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
