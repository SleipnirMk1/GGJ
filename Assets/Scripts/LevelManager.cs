using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int indexLevel;
    [Serializable]
    public class levelProp
    {
        public Transform cameraSpawnPoint, unitSpawnPoint, initialPath, finishPoint;
        public bool availability;
    }
    public levelProp[] levelProps;
    [SerializeField] Button upLvButton, downLvButton, buyFloor2Button;
    [SerializeField] DragCamera cam;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        upLvButton.onClick.AddListener(ToUpperLevel);
        downLvButton.onClick.AddListener(ToLowerLevel);
        buyFloor2Button.onClick.AddListener(delegate{BuyFloor(2,500);});
    }

    void OnDisable()
    {
        upLvButton.onClick.RemoveListener(ToUpperLevel);
        downLvButton.onClick.RemoveListener(ToLowerLevel);
        buyFloor2Button.onClick.RemoveListener(delegate{BuyFloor(2,500);});
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
        hero.position = levelProps[nextLevel].unitSpawnPoint.position;
        hero.GetComponent<MovementHero>().currentLevel = nextLevel;
    }

    void ToUpperLevel()
    {
        if (indexLevel > 0)
        {
            for (int i = indexLevel - 1; i >= 0; i--)
            {
                print(i);
                if (levelProps[i].availability)
                {
                    cam.enabled = false;
                    cam.transform.position = levelProps[i].cameraSpawnPoint.position;
                    indexLevel = i;
                    cam.enabled = true;
                    break;
                }
            }
        }
    }

    void ToLowerLevel()
    {
        if (indexLevel < levelProps.Length)
        {
            for (int i = indexLevel + 1; i < levelProps.Length; i++)
            {
                if (levelProps[i].availability)
                {
                    cam.enabled = false;
                    cam.transform.position = levelProps[i].cameraSpawnPoint.position;
                    indexLevel = i;
                    cam.enabled = true;
                    break;
                }
            }
        }
    }

    public void GoToCertainLevel(int levelGoal)
    {
        cam.enabled = false;
        cam.transform.position = levelProps[levelGoal].cameraSpawnPoint.position;
        indexLevel = levelGoal;
        cam.enabled = true;
    }

    public void BuyFloor(int floorBought, int price){
        if (SoulManager.Instance.soulCount >= price){
            buyFloor2Button.gameObject.SetActive(false);
            buyFloor2Button.transform.parent.GetComponent<Button>().interactable = true;
            SoulManager.Instance.ReduceSoul(price);
            levelProps[floorBought-1].availability = true;
        }
    }
}
