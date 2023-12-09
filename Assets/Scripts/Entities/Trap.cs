using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Trap : MonoBehaviour
{
    [Header("Character Reference")]
    public TrapObject characterScriptableObject;

    [Header("VFX Reference")]
    public GameObject vFXPrefab;

    // [Header("UI Reference")]
    // [SerializeField] GameObject infoHeroPrefab;
    // Transform infoMinion;
    // Image HPBar;

    private string name;
    private float atkDelay;
    private float atkRange;
    private Sprite explodeSprite;

    [Header("Debug & Communication")]
    private float atkDmg;
    private float soulCost;

    private SpriteRenderer spriteRenderer;
    
    public EntityState currentState;

    public void StartRound()
    {
        currentState = EntityState.STANDBY;
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
                StartCoroutine(Attack());
                break;
            default :
                Debug.Log("Trap State Error");
                break;
        }

        //infoMinion.GetComponent<RectTransform>().anchoredPosition = (Vector2)transform.position + Vector2.up * 0.75f;
    }

    void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.name = characterScriptableObject.name;
        this.atkDelay = characterScriptableObject.atkDelay;
        this.atkRange = characterScriptableObject.atkRange;
        this.explodeSprite = characterScriptableObject.explodeSprite;

        spriteRenderer.sprite = characterScriptableObject.sprite;

        this.atkDmg = characterScriptableObject.atkDmg;
        this.soulCost = characterScriptableObject.soulCost;


        // infoMinion = Instantiate(infoHeroPrefab, GameObject.Find("World Canvas").transform).transform;
        // HPBar = infoMinion.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    void UpdateDisplay()
    {
        // HPBar.fillAmount = currentHealth/maxHealth;
    }

    void CheckEnemy()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, atkRange);

        if (colliders.Length > 0)
        {
            currentState = EntityState.ATTACKING;
        }
        else
        {
            currentState = EntityState.STANDBY;
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(atkDelay);

        var colliders = Physics2D.OverlapCircleAll(transform.position, atkRange);
        foreach(var collider in colliders)
        {
            collider.GetComponent<Hero>().TakeDamage(atkDmg);
            Die();
        }
    }

    void Standby()
    {
        // idk, idle animation?
    }

    void Die()
    {
        //Destroy(infoMinion.gameObject);
        Destroy(gameObject);
    }
}
