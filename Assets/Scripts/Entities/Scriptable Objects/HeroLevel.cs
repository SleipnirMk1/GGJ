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

    public bool isCritical = false;

    public void AddExp(float value, Minion target)
    {
        float minionSoul = target.characterScriptableObject.soulCost;
        float tempVal = value / (Mathf.Sqrt(level) + (Mathf.Sqrt(minionSoul)));

        currentExp += tempVal;
    }

    public void AddHealExp(float value)
    {
        currentExp += (value * 0.6f);
    }

    public void SetLevel(int value)
    {
        level = value;
        if (value == 1)
        {
            currentMaxHealth = heroClass.health;
            currentPhysicalAtk = heroClass.physicalAtk;
            expToNextLvl = heroClass.baseXpReq;
        }
        else
        {
            currentMaxHealth = heroClass.health;
            currentPhysicalAtk = heroClass.physicalAtk;
            expToNextLvl = heroClass.baseXpReq;

            for (int j = 0; j < level; ++j)
            {
                currentMaxHealth = Mathf.Sqrt(level) + currentMaxHealth * heroClass.healthGrowth;
                currentPhysicalAtk = 0.9f * Mathf.Sqrt(level) / level + currentPhysicalAtk * heroClass.atkGrowth;
                expToNextLvl = Mathf.Floor(heroClass.baseXpReq + Mathf.Sqrt(expToNextLvl * level * heroClass.expGrowth));
            }
        }

        currentExp = 0;
    }
}
