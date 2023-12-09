using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTracker : MonoBehaviour
{
    // Singleton
    public static DayTracker Instance {
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

    public int dayCount;
}
