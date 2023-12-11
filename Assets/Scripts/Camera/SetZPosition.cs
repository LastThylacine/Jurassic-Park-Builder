using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetZPosition : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x,
            transform.position.y,
            -10);
    }
}
