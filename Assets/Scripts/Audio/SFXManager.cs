using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip strike;
    public AudioClip shield;
    public AudioClip selectCard;
    public AudioClip selectMenu;
    public AudioClip blowEnemy;
    public AudioClip heal;

    [Header("Audiosource")]
    public AudioSource source;

    public void PlaySFX(string name)
    {
        if (name == "strike")
        {
            source.clip = strike;
            source.Play();
        }
        else if (name == "shield")
        {
            source.clip = shield;
            source.Play();
        }
        else if (name == "select card")
        {
            source.clip = selectCard;
            source.Play();
        }
        else if (name == "select menu")
        {
            source.clip = selectMenu;
            source.Play();
        }
        else if (name == "blow")
        {
            source.clip = blowEnemy;
            source.Play();
        }
        else if (name == "heal")
        {
            source.clip = heal;
            source.Play();
        }
    }
}
