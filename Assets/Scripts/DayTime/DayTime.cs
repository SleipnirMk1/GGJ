using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    public enum TimeState
    {
        ONGOING,
        STOPPED,
    }
    public float dayDurationInSeconds;
    private float currentTime;
    private TimeState timeState = TimeState.STOPPED;

    void Start()
    {
        currentTime = 0f;
        StopDay();
    }

    void Update()
    {
        switch (timeState)
        {
            case TimeState.ONGOING:
                ProgressTime();
                break;

            case TimeState.STOPPED:
                break;

            default:
                Debug.Log("Time state error");
                break;
        }
    }

    void ProgressTime()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= dayDurationInSeconds)
        {
            StopDay();
        }
    }

    public void StartDay()
    {
        timeState = TimeState.ONGOING;
    }

    public void StopDay()
    {
        timeState = TimeState.STOPPED;
    }

}
