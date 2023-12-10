using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DayTracker : MonoBehaviour
{
    // Singleton
    public static DayTracker Instance {
        get; private set;
    }
    [SerializeField] MinionGacha gacha;
    [SerializeField] Button skipDayButton;
    bool allowSkip;

    void Awake()
    {
        // Persistent Singleton
        SingletonAwake();
    }
    void SingletonAwake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private int dayCount = 1;
    public TMP_Text dayText;

    void OnEnable(){
        skipDayButton.onClick.AddListener(SkipToNextDay);
    }

    void Start()
    {
        ResetDay();
    }

    public void CheckHeroes(){
        LevelManager levelManager = LevelManager.Instance;
        int sumHeroes = 0;
        for (int i = 0; i < levelManager.heroesInEachLevel.Count; i++)
        {
            sumHeroes += levelManager.heroesInEachLevel[i].heroList.Count;
            if (sumHeroes > 0) return;
        }
        if (sumHeroes == 0) allowSkip = true;
    }

    public void SkipToNextDay(){
        DayTime.Instance.EndDay();
        allowSkip = false;
    }

    void Update(){
        if (allowSkip) skipDayButton.gameObject.SetActive(true);
        else skipDayButton.gameObject.SetActive(false);
    }

    public void AddDay()
    {
        dayCount++;
        dayText.text = dayCount.ToString();
    }

    public void ResetDay()
    {
        dayCount = 1;
        dayText.text = dayCount.ToString();
        gacha.InitializeGacha();
    }

    public int GetDay()
    {
        return dayCount;
    }
}
