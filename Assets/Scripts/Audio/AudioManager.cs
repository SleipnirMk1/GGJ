using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum MusicState
    {
        BOSS,
        BOSSBYE,
        NORMAL,
    }

    public DungeonMaster dungeonMaster;
    public LevelManager levelManager;

    private MusicState whatIsPlaying = MusicState.NORMAL;
    private MusicState whatState = MusicState.NORMAL;

    void Start()
    {
        whatState = MusicState.NORMAL;
        whatIsPlaying = MusicState.NORMAL;
    }

    void Update()
    {
        CheckState();
        CheckPlaying();
    }

    void CheckState()
    {
        // get boss hp
        float currentHealth = dungeonMaster.currentHealth;
        float maxHealth = dungeonMaster.maxHealth;
        int totalLvlInRoom = LevelManager.Instance.totalLvlInRoom;

        if (currentHealth <= (maxHealth * 0.5))
        {
            whatState = MusicState.BOSSBYE;
        }
        // get encounter in boss room
        else if (totalLvlInRoom > 4)
        {
            whatState = MusicState.BOSS;
        }
        else
        {
            whatState = MusicState.NORMAL;
        }
    }

    void CheckPlaying()
    {
        if (whatState != whatIsPlaying)
        {
            switch(whatState)
            {
                case MusicState.NORMAL:
                    PlayNormal();
                    break;
                case MusicState.BOSS:
                    PlayBoss();
                    break;
                case MusicState.BOSSBYE:
                    PlayBossBye();
                    break;
                default:
                    Debug.Log("Music error");
                    break;
            }
        } 
    }

    void PlayNormal()
    {
        MusicManager.Instance.PlayMusic("Normal");
        whatIsPlaying = MusicState.NORMAL;
    }

    void PlayBoss()
    {
        MusicManager.Instance.PlayMusic("Boss");
        whatIsPlaying = MusicState.BOSS;
    }

    void PlayBossBye()
    {
        MusicManager.Instance.PlayMusic("Boss Bye");
        whatIsPlaying = MusicState.BOSSBYE;
    }
}
