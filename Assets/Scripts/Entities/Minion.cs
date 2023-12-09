using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Minion : MonoBehaviour
{
    [Header("Character Reference")]
    public MinionObject characterScriptableObject;

    [Header("Projectile Reference")]
    public GameObject projectilePrefab;

    [Header("UI Reference")]
    [SerializeField] GameObject infoHeroPrefab;
    Transform infoMinion;
    Image HPBar;

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
    private float soulCost;

    private bool isAllowedAttack = true;

    public float currentHealth;
    private SpriteRenderer spriteRenderer;
    
    public EntityState currentState;

    public void StartRound()
    {
        currentState = EntityState.STANDBY;
        isAllowedAttack = true;
    }

    void Awake()
    {
    }

    void Start()
    {
        Init();
        UpdateDisplay();
        StartRound();
    }

    void Update()
    {
        CheckEnemy();

        switch(currentState)
        {
            case EntityState.STANDBY:
                Standby();
                break;
            case EntityState.ATTACKING:
                Attack();
                break;
            default :
                Debug.Log("Minion State Error");
                break;
        }

        infoMinion.GetComponent<RectTransform>().anchoredPosition = (Vector2)transform.position + Vector2.up * 0.75f;
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

        this.minionType = characterScriptableObject.minionType;
        this.atkDmg = characterScriptableObject.atkDmg;
        this.soulCost = characterScriptableObject.soulCost;

        currentHealth = maxHealth;

        infoMinion = Instantiate(infoHeroPrefab, GameObject.Find("World Canvas").transform).transform;
        HPBar = infoMinion.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    void UpdateDisplay()
    {
        HPBar.fillAmount = currentHealth/maxHealth;
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
            currentState = EntityState.STANDBY;
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
        projectileScript.minionAttacker = this;
        projectileScript.target = target;
        projectileScript.projectileSprite = projectileSprite;
        projectileScript.damage = atkDmg;

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

    void Standby()
    {
        // idk, idle animation?
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
        Destroy(infoMinion.gameObject);
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
