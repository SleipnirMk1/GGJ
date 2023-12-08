using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    public static SoulManager Instance {get; set;}
    [SerializeField] TMP_Text text;
    public int soulCount;

    void Awake()
    {
        Instance = this;
    }

    void Start(){
        UpdateSoulText();
    }

    public void AddSoul(int value)
    {
        soulCount += value;
        UpdateSoulText();
    }

    public void ReduceSoul(int value){
        soulCount -= value;
        UpdateSoulText();
    }

    public void UpdateSoulText(){
        text.text = soulCount.ToString();
    }
}
