using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsListener : MonoBehaviour
{
    private bool _isAnimationEnded = true;
    [SerializeField] private UnityEvent _eventOnAnimationEnded;

    public bool IsAnimationEnded => _isAnimationEnded;

    public void OnAnimationEnded()
    {
        _isAnimationEnded = true;
        _eventOnAnimationEnded?.Invoke();
    }

    public void OnAnimationStarted()
    {
        _isAnimationEnded = false;
    }

    private void OnDisable()
    {
        OnAnimationEnded();
    }
}
