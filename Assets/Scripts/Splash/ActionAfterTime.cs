using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionAfterTime : MonoBehaviour
{
    [SerializeField] private UnityEvent _onTimeEnded;
    [SerializeField] private float _timeToWait;

    private float startTime;

    private void OnEnable()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - startTime > _timeToWait)
        {
            _onTimeEnded?.Invoke();
        }
    }
}
