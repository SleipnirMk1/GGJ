using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Singleton
    public static MusicManager Instance {
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

    public AudioClip boss;
    public AudioClip bossBye;
    public AudioClip normal;

    private AudioSource audioSource;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(string name)
    {
        audioSource.Stop();

        if (name == "Boss")
            audioSource.clip = boss;
        else if (name == "Boss Bye")
            audioSource.clip = bossBye;
        else if (name == "Normal")
            audioSource.clip = normal;
        
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
