using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
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
