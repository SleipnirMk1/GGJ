using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Hero : MonoBehaviour
{
    [Header("Character Reference")]
    public HeroObject characterScriptableObject;

    [Header("Projectile Reference")]
    public GameObject projectilePrefab;

    [Header("UI Reference")]

    [Header("Hero Qualities")]
    public float exp;

    private string name;
    private float maxHealth;
    private float atkDelay;
    private float moveSpeed;
    private float atkRange;
    private float aggroRange;
    private Sprite projectileSprite;

    public HeroType heroType;
    private float physicalAtk;
    private float magicAtk;

    private bool isAllowedAttack;

    public float currentHealth;
    private SpriteRenderer spriteRenderer;

    [Header("Debug & Communication")]
    public EntityState currentState;

    public void StartRound()
    {
        currentState = EntityState.WALKING;
        isAllowedAttack = true;
    }

    void Awake()
    {
        Init();
    }

    void Start()
    {
        UpdateDisplay();
        StartRound();
    }

    void Update()
    {
        CheckEnemy();

        switch(currentState)
        {
            case EntityState.WALKING:
                Walking();
                break;
            case EntityState.ATTACKING:
                Attack();
                break;
            default :
                Debug.Log("Hero State Error");
                break;
        }
    }

    void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.name = characterScriptableObject.name;
        this.maxHealth = characterScriptableObject.health;
        this.atkDelay = characterScriptableObject.atkDelay;
        this.moveSpeed = characterScriptableObject.moveSpeed;
        this.atkRange = characterScriptableObject.atkRange;
        this.aggroRange = characterScriptableObject.aggroRange;
        this.projectileSprite = characterScriptableObject.projectile;

        spriteRenderer.sprite = characterScriptableObject.sprite;

        this.heroType = characterScriptableObject.heroType;
        this.physicalAtk = characterScriptableObject.physicalAtk;
        this.magicAtk = characterScriptableObject.magicAtk;

        currentHealth = maxHealth;
    }

    void UpdateDisplay()
    {
        // refer ui
    }

    void CheckEnemy()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, aggroRange);
        if (heroType == HeroType.HEALER)
        {
            colliders = FilterHero(colliders);
        }
        else
        {
            colliders = FilterMinion(colliders);
        }

        if (colliders.Length > 0)
        {
            currentState = EntityState.ATTACKING;
        }
        else
        {
            currentState = EntityState.WALKING;
        }
    }

    void Attack()
    {
        // desummon the walker :v

        Vector2 target = GetTarget();
        float distance = Vector2.Distance(target, transform.position);

        if (distance > atkRange)
        {
            WalkToTarget(target);
        }
        else
        {
            if (isAllowedAttack)
            {
                StartCoroutine(DamageTarget(target));
            }
        }
    }

    void WalkToTarget(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    IEnumerator DamageTarget(Vector2 target)
    {
        isAllowedAttack = false;

        GameObject instance = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectileScript = instance.GetComponent<Projectile>();
        projectileScript.heroAttacker = this;
        projectileScript.target = target;
        projectileScript.projectileSprite = projectileSprite;
        projectileScript.damage = physicalAtk;

        yield return new WaitForSeconds(atkDelay);

        isAllowedAttack = true;
    }

    Vector2 GetTarget()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, aggroRange);
        if (heroType == HeroType.HEALER)
        {
            colliders = FilterHero(colliders);
        }
        else
        {
            colliders = FilterMinion(colliders);
        }
        
        Collider2D target = colliders[0]; 

        if (heroType == HeroType.HEALER)
        {
            float lowestHp = colliders[0].gameObject.GetComponent<Hero>().currentHealth;


            {
                foreach(var collider in colliders) {
                    float health = collider.gameObject.GetComponent<Hero>().currentHealth;
                    if (health < lowestHp)
                    {
                        lowestHp = health;
                        target = collider;
                    }
                }
            }
        }
        else
        {
            float shortestRange = Vector2.Distance(colliders[0].transform.position, transform.position);

            foreach(var collider in colliders) {
                float distance = Vector2.Distance(collider.transform.position, transform.position);
                if (distance < shortestRange)
                {
                    shortestRange = distance;
                    target = collider;
                }
            }
        }

        return target.transform.position;
    }

    void Walking()
    {
        // summon the walker :v
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateDisplay();

        if (currentHealth <= 0)
        {
            Die();
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    Collider2D[] FilterMinion(Collider2D[] input)
    {
        List<Collider2D> retList = new List<Collider2D>();

        foreach(Collider2D c in input)
        {
            if(c.gameObject.GetComponent<Minion>() != null)
                retList.Add(c);
        }

        return retList.ToArray();
    }

    Collider2D[] FilterHero(Collider2D[] input)
    {
        List<Collider2D> retList = new List<Collider2D>();

        foreach(Collider2D c in input)
        {
            Hero heroScript = c.gameObject.GetComponent<Hero>();
            if(heroScript != null)
            {
                if (heroScript.currentHealth < heroScript.maxHealth)
                {
                    retList.Add(c);
                }
            }
                
        }

        return retList.ToArray();
    }
}
