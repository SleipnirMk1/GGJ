using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTime : MonoBehaviour
{
    // Singleton
    public static DayTime Instance {
        get; private set;
    }
    void Awake()
    {
        // Persistent Singleton
        SingletonAwake();
    }
    void SingletonAwake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
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
        StartDay();
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

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void StartDay()
    {
        currentTime = 0;

        HeroParty.Instance.SpawnHeroParty();
        ResumeDay();
    }

    public void EndDay()
    {
        StopDay();
        DayTracker.Instance.AddDay();

        DungeonMasterObject.Instance.ProcessEndDay();
        HeroParty.Instance.ProcessEndDay();
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
