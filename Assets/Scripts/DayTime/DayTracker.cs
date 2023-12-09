using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayTracker : MonoBehaviour
{
    // Singleton
    public static DayTracker Instance {
        get; private set;
    }
    [SerializeField] MinionGacha gacha;

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

    private int dayCount = 1;
    public TMP_Text dayText;

    void Start()
    {
        ResetDay();
    }

    public void AddDay()
    {
        dayCount++;
        dayText.text = dayCount.ToString();
    }

    public void ResetDay()
    {
        dayCount = 1;
        dayText.text = dayCount.ToString();
        gacha.InitializeGacha();
    }

    public int GetDay()
    {
        return dayCount;
    }
}
