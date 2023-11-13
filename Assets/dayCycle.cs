using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public const float SecondsInDay = 86400f;

    [SerializeField] [Range(0, SecondsInDay)] private float _realtimeDayLength = 60;
    [SerializeField] private float _startHour;

    private DateTime _currentTime;
    private DateTime _lastDayTime;

    private void Start()
    {
        _currentTime = DateTime.Now + TimeSpan.FromHours(_startHour);
        _lastDayTime = _currentTime;
    }

    private void Update()
    {
        // Calculate a seconds step that we need to add to the current time
        float secondsStep = SecondsInDay / _realtimeDayLength * Time.deltaTime;
        _currentTime = _currentTime.AddSeconds(secondsStep);

        // Check and increment the days passed
        TimeSpan difference = _currentTime - _lastDayTime;
        if(difference.TotalSeconds >= SecondsInDay)
        {
            _lastDayTime = _currentTime;
            GameManager.instance.curDay++;
        }
    }
}
