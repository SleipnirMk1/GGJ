using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // Singleton
    public static SFXManager Instance {
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

    [Header("SFX")]
    public AudioClip arrowHit;
    public AudioClip click;
    public AudioClip hit;
    public AudioClip slash;
    public AudioClip soulGet;
    public AudioClip teeth;

    [Header("Audiosource")]
    public AudioSource source;

    [Header("Misc")]
    public float soundDelay = 0.5f;

    private bool[] allowedToFire = {true, true, true, true, true, true};

    public void PlaySFX(string name)
    {
        if (name == "Arrow")
        {
            StartCoroutine(PlaySFX(0));
        }
        else if (name == "Click")
        {
            StartCoroutine(PlaySFX(1));
        }
        else if (name == "Hit")
        {
            StartCoroutine(PlaySFX(2));
        }
        else if (name == "Slash")
        {
            StartCoroutine(PlaySFX(3));
        }
        else if (name == "Soul")
        {
            StartCoroutine(PlaySFX(4));
        }
        else if (name == "Teeth")
        {
            StartCoroutine(PlaySFX(5));
        }
    }

    IEnumerator PlaySFX(int idx)
    {
        AudioClip toBePlayed = soulGet;
        switch(idx)
        {
            case 0:
                toBePlayed = arrowHit;
                break;
            case 1:
                toBePlayed = click;
                break;
            case 2:
                toBePlayed = hit;
                break;
            case 3:
                toBePlayed = slash;
                break;
            case 4:
                toBePlayed = soulGet;
                break;
            case 5:
                toBePlayed = teeth;
                break;
            default:
                Debug.Log("SFX error");
                break;
        }

        if (allowedToFire[idx])
        {
            allowedToFire[idx] = false;

            source.clip = toBePlayed;
            source.Play();

            yield return new WaitForSeconds(soundDelay);
            allowedToFire[idx] = true;
        }
        
    }
}
