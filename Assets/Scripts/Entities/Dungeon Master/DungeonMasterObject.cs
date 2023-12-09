using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMasterObject : MonoBehaviour
{
    // Singleton
    public static DungeonMasterObject Instance {
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

    public MinionObject minionBase;
    public MinionGacha gachaScript;
    public DungeonMaster dungeonMasterEntity;

    [Header("Communication")]
    public int level;
    public float expToNextLvl;
    public float currentMaxHealth;
    public float currentAtk;

    public List<MinionObject> ownedMinions = new List<MinionObject>();
    public float currentExp;

    public void AddExp(float value)
    {
        XPManager.Instance.UpdateXP();
        currentExp += value;
    }

    public void AddKillExp(Hero hero)
    {
        AddExp(hero.characterScriptableObject.level * 60);
    }

    public void SetLevel(int value)
    {
        if (value == 1)
        {
            currentMaxHealth = minionBase.health;
            currentAtk = minionBase.atkDmg;
            expToNextLvl = minionBase.baseXpReq;
        }
        else
        {
            currentMaxHealth = Mathf.Sqrt(level) + minionBase.health * minionBase.healthGrowth;
            currentAtk = Mathf.Sqrt(level) / level + minionBase.atkDmg * minionBase.atkGrowth;

            expToNextLvl = minionBase.baseXpReq;
            for (int j = 0; j < level; ++j)
            {
                expToNextLvl = Mathf.Floor( minionBase.baseXpReq +  Mathf.Sqrt( expToNextLvl * level * minionBase.expGrowth ) );
            }
        }
        
        currentExp = 0;
    }

    public void ProcessEndDay()
    {
        dungeonMasterEntity.Init();
        dungeonMasterEntity.UpdateDisplay();
        dungeonMasterEntity.StartRound();

        if (currentExp >= expToNextLvl)
        {
            SetLevel(level++);
            gachaScript.InitializeGacha();
            
            dungeonMasterEntity.Init();
            dungeonMasterEntity.UpdateDisplay();
            dungeonMasterEntity.StartRound();
        }
        else
        {
            DayTime.Instance.StartDay();
        }
    }
}
