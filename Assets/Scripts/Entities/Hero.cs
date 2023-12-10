using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Hero : MonoBehaviour
{
    [Header("Character Reference")]
    public HeroLevel characterScriptableObject;
    public float criticalHealthThreshold = 0.25f;
    public float criticalTimeThreshold = 0.2f;

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
    bool isFleeing;

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
        if (currentState != EntityState.CRITICAL)
        {
            CheckEnemy();
            CheckTime();
            CheckCritical();
        }
        else
        {
            if (!isFleeing)
            {
                movementHero.FleeBattle();
                isFleeing = true;
            }
        }

        switch (currentState)
        {
            case EntityState.WALKING:
                Walking();
                break;
            case EntityState.ATTACKING:
                Attack();
                break;
            case EntityState.CRITICAL:
                break;
            default:
                Debug.Log("Hero State Error");
                Debug.Log(currentState);
                break;
        }
        
        infoHero.GetComponent<RectTransform>().anchoredPosition = (Vector2)transform.position + Vector2.up * 0.75f;
        infoHero.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = characterScriptableObject.level.ToString();
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
        HPBar.fillAmount = currentHealth / maxHealth;
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
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, GetTarget().transform.position - transform.position);
            List<RaycastHit2D> rayList = new List<RaycastHit2D>(hit);

            if (rayList[0].collider != null)
            {
                int del = -1;
                for (int i = 0; i < rayList.Count; ++i)
                {
                    if (rayList[i].collider.CompareTag("Deadzone"))
                    {
                        del = i;
                        break;
                    }
                }
                if (del != -1)
                    rayList.RemoveAt(del);

                if (rayList[0].collider.CompareTag("Wall"))
                {
                    currentState = EntityState.WALKING;
                }
                else
                {
                    currentState = EntityState.ATTACKING;
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
        if (target == null)
        {
            currentState = EntityState.STANDBY;
            return;
        }

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
        if (transform.position.x < target.x) spriteRenderer.flipX = true;
        if (transform.position.x > target.x) spriteRenderer.flipX = false;
    }

    IEnumerator DamageTarget(Collider2D obj)
    {
        isAllowedAttack = false;

        switch(heroType)
        {
            case HeroType.HUNTER:
                SFXManager.Instance.PlaySFX("Arrow");
                break;
            case HeroType.WIZARD:
            case HeroType.HEALER:
                SFXManager.Instance.PlaySFX("Click");
                break;
            default:
                SFXManager.Instance.PlaySFX("Slash");
                break;
        }

        Vector2 target = obj.transform.position;
        GameObject instance = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectileScript = instance.GetComponent<Projectile>();
        projectileScript.heroAttacker = this;
        projectileScript.target = target;
        projectileScript.projectileSprite = projectileSprite;
        projectileScript.damage = physicalAtk;

        characterScriptableObject.AddExp(physicalAtk, obj.gameObject.GetComponent<Minion>());
        if (heroType != HeroType.HEALER)
        {
            Minion isMinion = obj.gameObject.GetComponent<Minion>();
            if (isMinion != null)
            {
                characterScriptableObject.AddExp(physicalAtk, obj.gameObject.GetComponent<Minion>());
            }
        }   

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
        
        if (colliders.Length == 0)
        {
            return null;
        }
        
        Collider2D target = colliders[0]; 

        if (heroType == HeroType.HEALER)
        {
            float lowestHp = colliders[0].gameObject.GetComponent<Hero>().currentHealth;
            foreach (var collider in colliders)
            {
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
            foreach (var collider in colliders)
            {
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

        if (currentHealth <= 0.25f * maxHealth)
        {
            currentState = EntityState.CRITICAL;
        }
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
        HeroParty.Instance.AddConsecutiveDeath();

        Destroy(infoHero.gameObject);
        Destroy(gameObject);
    }

    public void FleeBattle()
    {
        Destroy(infoHero.gameObject);
        Destroy(gameObject);
    }

    void CheckCritical()
    {
        if (currentHealth <= (maxHealth * criticalHealthThreshold))
        {
            characterScriptableObject.isCritical = true;
        }
    }

    Collider2D[] FilterMinion(Collider2D[] input)
    {
        List<Collider2D> retList = new List<Collider2D>();

        foreach (Collider2D c in input)
        {
            if (c.gameObject.GetComponent<Minion>() != null)
                retList.Add(c);
        }

        return retList.ToArray();
    }

    Collider2D[] FilterHero(Collider2D[] input)
    {
        List<Collider2D> retList = new List<Collider2D>();

        foreach (Collider2D c in input)
        {
            Hero heroScript = c.gameObject.GetComponent<Hero>();
            if (heroScript != null)
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

        foreach (Collider2D c in input)
        {
            if (c.gameObject.GetComponent<DungeonMaster>() != null)
                retList.Add(c);
        }

        return retList.ToArray();
    }

    void CheckTime()
    {
        float currentTime = DayTime.Instance.GetCurrentTime();
        float maxTime = DayTime.Instance.dayDurationInSeconds;

        if (currentTime >= ((1 - criticalTimeThreshold) * maxTime))
        {
            currentState = EntityState.CRITICAL;
        }
    }
}
