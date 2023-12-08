using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Create Hero")]
public class Hero : Entity
{
    [Header("Hero Specific Properties")]
    public HeroType heroType;
    public float physicalAtk;
    public float magicAtk;
}
