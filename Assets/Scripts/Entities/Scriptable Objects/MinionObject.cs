using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Create Minion")]
public class MinionObject : EntityObject
{
    [Header("Minion Specific Properties")]
    public Sprite card;
    public MinionType minionType;
    public float atkDmg;
    public float soulCost;
    public int weight;

    public float cooldown;

    [Header("Boss Specific Properties")]
    public float baseXpReq = 100f;
    public float expGrowth;
    public float atkGrowth;
    public float healthGrowth;
}
