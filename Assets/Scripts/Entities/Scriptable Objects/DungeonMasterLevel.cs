using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dungeon Master", menuName = "Create Dungeon Master")]
public class DungeonMasterLevel : ScriptableObject
{
    public MinionObject minionBase;

    [Header("Communication")]
    public int level;
    public float expToNextLvl;
    public float currentMaxHealth;
    public float currentAtk;

    public float currentExp;

    public void AddExp(float value)
    {
        currentExp += value;
        if (currentExp >= expToNextLvl)
        {
            SetLevel(level++);

            // call gacha
        }
    }

    public void SetLevel(int value)
    {
        currentMaxHealth = Mathf.Sqrt(level + minionBase.health * minionBase.healthGrowth);
        currentAtk = Mathf.Sqrt(level + minionBase.atkDmg * minionBase.atkGrowth);

        expToNextLvl = minionBase.baseXpReq;
        for (int j = 0; j < level; ++j)
        {
            expToNextLvl = Mathf.Floor( minionBase.baseXpReq +  Mathf.Sqrt( expToNextLvl * level * minionBase.expGrowth ) );
        }

        currentExp = 0;
    }
}
