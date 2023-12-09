using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class DungeonMaster : MonoBehaviour
{
    [Header("Projectile Reference")]
    public GameObject projectilePrefab;

    [Header("Boss Stuff")]
    public Vector2 standingLocation;

    [Header("UI Reference")]

    private string name;
    private float maxHealth;
    private float atkDelay;
    private float moveSpeed;
    private float atkRange;
    private float aggroRange;
    private Sprite projectileSprite;

    [Header("Debug & Communication")]
    private MinionType minionType;
    private float atkDmg;

    private bool isAllowedAttack = true;
    public float currentHealth;
    private SpriteRenderer spriteRenderer;
    
    public EntityState currentState;

    public void StartRound()
    {
        currentState = EntityState.STANDBY;
        isAllowedAttack = true;
    }

    void Start()
    {
        DungeonMasterObject.Instance.SetLevel(1);
        Init();
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
                Debug.Log("Minion State Error");
                break;
        }
    }

    public void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.name = DungeonMasterObject.Instance.minionBase.name;
        this.maxHealth = DungeonMasterObject.Instance.minionBase.health;
        this.atkDelay = DungeonMasterObject.Instance.minionBase.atkDelay;
        this.moveSpeed = DungeonMasterObject.Instance.minionBase.moveSpeed;
        this.atkRange = DungeonMasterObject.Instance.minionBase.atkRange;
        this.aggroRange = DungeonMasterObject.Instance.minionBase.aggroRange;
        this.projectileSprite = DungeonMasterObject.Instance.minionBase.projectile;

        spriteRenderer.sprite = DungeonMasterObject.Instance.minionBase.sprite;

        this.minionType = DungeonMasterObject.Instance.minionBase.minionType;
        this.atkDmg = DungeonMasterObject.Instance.minionBase.atkDmg;

        currentHealth = maxHealth;
    }

    public void UpdateDisplay()
    {
        // refer ui
    }

    void CheckEnemy()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, aggroRange);
        colliders = FilterHero(colliders);

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
        projectileScript.masterAttacker = this;
        projectileScript.target = target;
        projectileScript.projectileSprite = projectileSprite;
        projectileScript.damage = atkDmg;

        DungeonMasterObject.Instance.AddExp(atkDmg);

        yield return new WaitForSeconds(atkDelay);

        isAllowedAttack = true;
    }

    Vector2 GetTarget()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, aggroRange);
        colliders = FilterHero(colliders);

        float shortestRange = Vector2.Distance(colliders[0].transform.position, transform.position);
        Collider2D target = colliders[0]; 

        foreach(var collider in colliders) {
            float distance = Vector2.Distance(collider.transform.position, transform.position);
            if (distance < shortestRange)
            {
                shortestRange = distance;
                target = collider;
            }
        }

        return target.transform.position;
    }

    void Walking()
    {
        transform.position = Vector2.MoveTowards(transform.position, standingLocation, moveSpeed * Time.deltaTime);
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
            if(c.gameObject.GetComponent<Hero>() != null)
                retList.Add(c);
        }

        return retList.ToArray();
    }
}
