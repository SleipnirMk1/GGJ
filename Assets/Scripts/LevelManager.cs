using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance {get; private set;}
    public int currentLevel;
    [Serializable]
    public class levelProp
    {
        public Transform spawnPoint, initialPath, finishPoint;
        public bool availability;
    }
    public levelProp[] levelProps;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AskTeleport(Transform hero, int fromLevel)
    {
        int nextLevel = fromLevel;
        for (int i = fromLevel + 1; i < levelProps.Length; i++)
        {
            if (levelProps[i].availability)
            {
                nextLevel = i;
                break;
            }
        }
        hero.position = levelProps[nextLevel].spawnPoint.position;
        hero.GetComponent<MovementHero>().currentLevel = nextLevel;
    }
}
