using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trap", menuName = "Create Trap")]
public class TrapObject : ScriptableObject
{
    public string name;
    public float atkDelay;
    public float atkRange;
    public Sprite sprite;
    public Sprite explodeSprite;
    public float atkDmg;
    public float soulCost;
}
