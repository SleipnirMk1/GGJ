using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ScriptableObject
{
    [Header("Common Entity Properties")]
    public string name;
    public float health;
    public float atkDelay;
    public float moveSpeed;
    public float atkRange;
    public Sprite sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
