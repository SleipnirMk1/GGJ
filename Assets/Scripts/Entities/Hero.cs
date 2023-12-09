using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Hero : MonoBehaviour
{
    [Header("Character Reference")]
    public HeroLevel characterScriptableObject;

    [Header("Projectile Reference")]
    public GameObject projectilePrefab;

    [Header("UI Reference")]
    [SerializeField] GameObject infoHeroPrefab;
    Transform infoHero;
    Image HPBar;

    [Header("Hero Qualities")]

    [Header("Debug & Communication")]
    private string name;
    private float maxHealth;
    private float atkDelay;
    public float moveSpeed;
    private float atkRange;
    private float aggroRange;
    private Sprite projectileSprite;

    public HeroType heroType;
    private float physicalAtk;
    private float magicAtk;

    private bool isAllowedAttack;

    public float currentHealth;
    private SpriteRenderer spriteRenderer;

    private MovementHero movementHero;
    private Rigidbody2D rb2d;

    public EntityState currentState;

    public void StartRound()
    {
        currentState = EntityState.WALKING;
        isAllowedAttack = true;
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
            case EntityState.WALKING:
                Walking();
                break;
            case EntityState.ATTACKING:
                Attack();
                break;
            default :
                Debug.Log("Hero State Error");
                Debug.Log(currentState);
                break;
        }

        infoHero.GetComponent<RectTransform>().anchoredPosition = (Vector2)transform.position + Vector2.up * 0.75f;
    }

    void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        this.name = characterScriptableObject.heroClass.name;
        this.maxHealth = characterScriptableObject.currentMaxHealth;
        this.atkDelay = characterScriptableObject.heroClass.atkDelay;
        this.moveSpeed = characterScriptableObject.heroClass.moveSpeed;
        this.atkRange = characterScriptableObject.heroClass.atkRange;
        this.aggroRange = characterScriptableObject.heroClass.aggroRange;
        this.projectileSprite = characterScriptableObject.heroClass.projectile;

        spriteRenderer.sprite = characterScriptableObject.heroClass.sprite;

        this.heroType = characterScriptableObject.heroClass.heroType;
        this.physicalAtk = characterScriptableObject.currentPhysicalAtk;
        this.magicAtk = characterScriptableObject.currentMagicalAtk;

        movementHero = GetComponent<MovementHero>();

        currentHealth = maxHealth;

        infoHero = Instantiate(infoHeroPrefab, GameObject.Find("World Canvas").transform).transform;
        HPBar = infoHero.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    void UpdateDisplay()
    {
        HPBar.fillAmount = currentHealth/maxHealth;
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
            var temp = FilterMinion(colliders);
            if (temp.Length > 0)
            {
                colliders = temp;
            }
            else
            {
                colliders = FilterMaster(colliders);
            }        
        }


        if (colliders.Length > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, ( GetTarget().transform.position - transform.position) );

            if (hit.collider != null)
            {
                if (!hit.collider.CompareTag("Wall"))
                {
                    currentState = EntityState.ATTACKING;
                }
                else
                {
                    currentState = EntityState.WALKING;
                }
            }
            else
            {
                currentState = EntityState.WALKING;
            }
        }
        else
        {
            currentState = EntityState.WALKING;
        }
    }

    void Attack()
    {
        movementHero.StopWalking();

        Collider2D target = GetTarget();
        float distance = Vector2.Distance(target.transform.position, transform.position);

        if (distance > atkRange)
        {
            WalkToTarget(target.transform.position);
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
        //transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        var heading = (target - rb2d.position).normalized;
        rb2d.MovePosition(rb2d.position + heading * moveSpeed * Time.fixedDeltaTime);
    }

    IEnumerator DamageTarget(Collider2D obj)
    {
        Vector2 target = obj.transform.position;
        isAllowedAttack = false;

        GameObject instance = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectileScript = instance.GetComponent<Projectile>();
        projectileScript.heroAttacker = this;
        projectileScript.target = target;
        projectileScript.projectileSprite = projectileSprite;
        projectileScript.damage = physicalAtk;

        if (heroType != HeroType.HEALER)
            characterScriptableObject.AddExp(physicalAtk, obj.gameObject.GetComponent<Minion>());

        yield return new WaitForSeconds(atkDelay);

        isAllowedAttack = true;
    }

    Collider2D GetTarget()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, aggroRange);
        if (heroType == HeroType.HEALER)
        {
            colliders = FilterHero(colliders);
        }
        else
        {
            var temp = FilterMinion(colliders);
            if (temp.Length > 0)
            {
                colliders = temp;
            }
            else
            {
                colliders = FilterMaster(colliders);
            }    
        }
        
        Collider2D target = colliders[0]; 

        if (heroType == HeroType.HEALER)
        {
            float lowestHp = colliders[0].gameObject.GetComponent<Hero>().currentHealth;
            foreach(var collider in colliders) {
                float health = collider.gameObject.GetComponent<Hero>().currentHealth;
                if (health < lowestHp)
                {
                    lowestHp = health;
                    target = collider;
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

        return target;
    }

    void Walking()
    {
        movementHero.StartWalking();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateDisplay();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        DungeonMasterObject.Instance.AddExp(amount);
        SoulManager.Instance.AddSoul(Mathf.CeilToInt(amount));
    }

    void Die()
    {
        DungeonMasterObject.Instance.AddKillExp(this);
        Destroy(infoHero.gameObject);
        Destroy(gameObject);
    }

    void CheckCritical()
    {
        if (currentHealth <= (maxHealth * 0.25))
        {
            characterScriptableObject.isCritical = true;
        }
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

    Collider2D[] FilterMaster(Collider2D[] input)
    {
        List<Collider2D> retList = new List<Collider2D>();

        foreach(Collider2D c in input)
        {
            if(c.gameObject.GetComponent<DungeonMaster>() != null)
                retList.Add(c);
        }

        return retList.ToArray();
    }
}
