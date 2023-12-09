using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip arrowHit;
    public AudioClip click;
    public AudioClip hit;
    public AudioClip slash;
    public AudioClip soulGet;
    public AudioClip teeth;

    [Header("Audiosource")]
    public AudioSource source;

    public void PlaySFX(string name)
    {
        if (name == "Arrow")
        {
            source.clip = arrowHit;
            source.Play();
        }
        else if (name == "Click")
        {
            source.clip = click;
            source.Play();
        }
        else if (name == "Hit")
        {
            source.clip = hit;
            source.Play();
        }
        else if (name == "Slash")
        {
            source.clip = slash;
            source.Play();
        }
        else if (name == "Soul")
        {
            source.clip = soulGet;
            source.Play();
        }
        else if (name == "Teeth")
        {
            source.clip = teeth;
            source.Play();
        }
    }
}
