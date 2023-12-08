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

    [Header("Debug & Communication")]
    public List<HeroLevel> heroParty =  new List<HeroLevel>();
    public int consecutiveDeath = 0;
    private int currentTotalLevels;

    void Start()
    {
        InitiateParty();
        SpawnHeroParty();
    }

    public void InitiateParty()
    {
        heroParty.Clear();

        int totalLvl = 0;
        int remainingLvl = basePartyTotalLevels;
        for(int i = 0; i < 4; ++i)
        {
            int selectedClassIdx = Random.Range(0, heroClasses.Count);
            int selectedLvl = Random.Range(1, remainingLvl);

            HeroLevel newHero = ScriptableObject.CreateInstance<HeroLevel>();
            newHero.heroClass = heroClasses[selectedClassIdx];
            newHero.level = selectedLvl;

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
        foreach (HeroLevel hero in heroParty)
        {
            totalLvl += hero.level;
        }

        for (int i = 0; i < (4 - heroParty.Count); ++i)
        {
            int selectedClassIdx = Random.Range(0, heroClasses.Count);
            int selectedLvl;

            HeroLevel newHero = ScriptableObject.CreateInstance<HeroLevel>();
            newHero.heroClass = heroClasses[selectedClassIdx];

            switch(heroParty.Count)
            {
                case 1:
                case 2:
                    selectedLvl = Random.Range(1, remainingLvl);
                    break;
                case 3:
                    selectedLvl = remainingLvl;
                    break;
                default:
                    return;
            }

            newHero.level = selectedLvl;

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
        foreach (HeroLevel hero in heroParty)
        {
            Hero heroScript = Instantiate(heroPrefab, spawnLocation.position, spawnLocation.rotation).GetComponent<Hero>();
            heroScript.characterScriptableObject = hero;
        }
    }
}
