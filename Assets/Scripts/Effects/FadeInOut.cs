using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private Animator[] _animatorControllers;

    public void SetFade(bool isEnabled)
    {
        for (int i = 0; i < _animatorControllers.Length; i++)
        {
            _animatorControllers[i].SetBool("FadeInOut", isEnabled);
        }
    }
} 
