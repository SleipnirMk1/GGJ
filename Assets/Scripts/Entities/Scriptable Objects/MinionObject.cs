using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Create Minion")]
public class MinionObject : EntityObject
{
    [Header("Minion Specific Properties")]
    public MinionType minionType;
    public float atkDmg;
    public float soulCost;
}
