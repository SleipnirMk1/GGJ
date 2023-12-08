using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityObject : ScriptableObject
{
    [Header("Common Entity Properties")]
    public string name;
    public float health;
    public float atkDelay;
    public float moveSpeed;
    public float atkRange;
    public Sprite sprite;
    public Sprite projectile;

    public float aggroRange;
}
