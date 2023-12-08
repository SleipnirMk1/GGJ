using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLevel : ScriptableObject
{
    public HeroObject heroClass;

    [Header("Communication")]
    public int level;
    public float expToNextLvl;
    public float currentMaxHealth;
    public float currentPhysicalAtk;
    public float currentMagicalAtk;

    public float currentExp;

    public void AddExp(float value)
    {
        currentExp += value;
        if (currentExp >= expToNextLvl)
        {
            SetLevel(level++);
        }
    }

    public void SetLevel(int value)
    {
        currentMaxHealth = Mathf.Sqrt(level + heroClass.health * heroClass.healthGrowth);
        currentPhysicalAtk = Mathf.Sqrt(level + heroClass.physicalAtk * heroClass.atkGrowth);

        expToNextLvl = heroClass.baseXpReq;
        for (int j = 0; j < level; ++j)
        {
            expToNextLvl = Mathf.Floor( heroClass.baseXpReq +  Mathf.Sqrt( expToNextLvl * level * heroClass.expGrowth ) );
        }

        currentExp = 0;
    }
}
