using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWorldBounds : MonoBehaviour
{
    private void Awake()
    {
        var bounds = GetComponent<Collider2D>().bounds;
        Globals.WorldBounds = bounds;
    }
}
