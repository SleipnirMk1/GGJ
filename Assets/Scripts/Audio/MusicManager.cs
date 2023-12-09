using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip titleTheme;
    public AudioClip traversalTheme;
    public AudioClip bossTheme;

    private AudioSource audioSource;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(string name)
    {
        audioSource.Stop();

        if (name == "title")
            audioSource.clip = titleTheme;
        else if (name == "traversal")
            audioSource.clip = traversalTheme;
        else if (name == "boss")
            audioSource.clip = bossTheme;
        
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
