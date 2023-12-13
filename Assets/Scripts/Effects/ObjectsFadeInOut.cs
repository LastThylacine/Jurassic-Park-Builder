using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ObjectsFadeInOut : MonoBehaviour
{
    [SerializeField] private Animator[] _animatorControllers;

    public void FadeInOut(bool isEnabled)
    {
        for (int i = 0; i < _animatorControllers.Length; i++)
        {
            _animatorControllers[i].SetBool("FadeInOut", isEnabled);
        }
    }
} 
