using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URL : MonoBehaviour
{
    public void Load(string url)
    {
        Application.OpenURL(url);
    }
}
