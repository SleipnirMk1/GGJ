using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTime : MonoBehaviour
{
    public enum TimeState
    {
        ONGOING,
        STOPPED,
    }
    public float dayDurationInSeconds;
    public Slider timeSlider;

    private float currentTime;
    private TimeState timeState = TimeState.STOPPED;

    void Start()
    {
        timeSlider.maxValue = dayDurationInSeconds;
        currentTime = 0f;
        UpdateDisplay();
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
        UpdateDisplay();
        if (currentTime >= dayDurationInSeconds)
        {
            EndDay();
        }
    }

    void UpdateDisplay()
    {
        timeSlider.value = currentTime;
    }

    void EndDay()
    {
        StopDay();
    }

    public void ResumeDay()
    {
        timeState = TimeState.ONGOING;
    }

    public void StopDay()
    {
        timeState = TimeState.STOPPED;
    }

}
