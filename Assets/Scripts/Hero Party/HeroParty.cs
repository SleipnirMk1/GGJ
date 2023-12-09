using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroParty : MonoBehaviour
{
    // Singleton
    public static HeroParty Instance {
        get; private set;
    }
    void Awake()
    {
        // Persistent Singleton
        SingletonAwake();
        PersistentScriptAwake();
        InitiateParty();
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
    void PersistentScriptAwake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int basePartyTotalLevels;
    public GameObject heroPrefab;
    public Transform spawnLocation;
    public List<HeroObject> heroClasses = new List<HeroObject>();

    public int dailyThreatGrowth = 2;

    public float spawnVariableX;
    public float spawnVariableY;

    [Header("Debug & Communication")]
    public List<HeroLevel> heroParty =  new List<HeroLevel>();
    public int consecutiveDeath = 0;
    public List<HeroLevel> fleeingHero;
    private int currentTotalLevels;


    public void InitiateParty()
    {
        ResetConsecutiveDeath();
        currentTotalLevels = basePartyTotalLevels;
        heroParty.Clear();

        int totalLvl = 0;
        int remainingLvl = basePartyTotalLevels;
        bool hasHealer = false;

        for(int i = 0; i < 4; ++i)
        {
            int selectedClassIdx = 0;
            if (hasHealer)
            {
                selectedClassIdx = Random.Range(0, heroClasses.Count-1);
            }
            else
            {
                selectedClassIdx = Random.Range(0, heroClasses.Count);
            }
            
            int selectedLvl = Random.Range(1, remainingLvl);

            HeroLevel newHero = ScriptableObject.CreateInstance<HeroLevel>();
            newHero.heroClass = heroClasses[selectedClassIdx];
            if (newHero.heroClass.heroType == HeroType.HEALER)
            {
                hasHealer = true;
            }
            
            newHero.SetLevel(selectedLvl);

            heroParty.Add(newHero);

            totalLvl += selectedLvl;
            remainingLvl -= selectedLvl;
            if (remainingLvl <= 0)
            {
                break;
            }
        }
        
    }

    public void FillParty()
    {
        int totalLvl = 0;
        int remainingLvl = currentTotalLevels * consecutiveDeath;

        bool hasHealer = false;
        foreach (HeroLevel hero in heroParty)
        {
            totalLvl += hero.level;
            if (hero.heroClass.heroType == HeroType.HEALER)
            {
                hasHealer = true;
            }
        }

        while (totalLvl < remainingLvl)
        {
            int selectedClassIdx = 0;
            if (hasHealer)
            {
                selectedClassIdx = Random.Range(0, heroClasses.Count-1);
            }
            else
            {
                selectedClassIdx = Random.Range(0, heroClasses.Count);
            }

            int selectedLvl = Random.Range(1, remainingLvl);
            HeroLevel newHero = ScriptableObject.CreateInstance<HeroLevel>();
            newHero.heroClass = heroClasses[selectedClassIdx];
            if (newHero.heroClass.heroType == HeroType.HEALER)
            {
                hasHealer = true;
            }

            newHero.SetLevel(selectedLvl);

            heroParty.Add(newHero);

            totalLvl += selectedLvl;
            remainingLvl -= selectedLvl;

            if (remainingLvl <= 0)
            {
                break;
            }
        }
    }

    public void ResetConsecutiveDeath()
    {
        consecutiveDeath = 0;
    }

    public void AddConsecutiveDeath()
    {
        consecutiveDeath += 1;
    }

    public void SpawnHeroParty()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        foreach (HeroLevel hero in heroParty)
        {
            float randX = Random.Range(-spawnVariableX, spawnVariableX);
            float randY = Random.Range(-spawnVariableY, spawnVariableY);
            Vector2 spawnPos = new Vector2(spawnLocation.position.x + randX, spawnLocation.position.y + randY);
            Hero heroScript = Instantiate(heroPrefab, spawnPos, spawnLocation.rotation).GetComponent<Hero>();
            heroScript.characterScriptableObject = hero;

            yield return new WaitForSeconds(0.15f);
        }
    }

    public void ProcessEndDay()
    {
        // threat growth
        currentTotalLevels += dailyThreatGrowth;
        if (consecutiveDeath > 1)
        {
            currentTotalLevels *= consecutiveDeath;
        }

        // rise lvl
        foreach (HeroLevel hero in heroParty)
        {
            if (hero.currentExp >= hero.expToNextLvl)
            {
                hero.SetLevel(hero.level + 1);
            }
        }

        // fill party to threat lvl
        FillParty();
    }
}
