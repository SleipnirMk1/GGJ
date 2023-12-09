using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 20f;

    [Header("Debug & Communication")]
    public Hero heroAttacker;
    public Minion minionAttacker;
    public DungeonMaster masterAttacker;
    public Vector2 target;
    public Sprite projectileSprite;
    public float damage;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = projectileSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Hero heroScript = other.gameObject?.GetComponent<Hero>();
        Minion minionScript = other.gameObject?.GetComponent<Minion>();
        DungeonMaster dungeonMasterScript = other.gameObject?.GetComponent<DungeonMaster>();

        if (heroScript != null)
        {
            if (heroAttacker != null)
            {
                if (heroAttacker.heroType == HeroType.HEALER && heroAttacker != heroScript)
                {
                    heroScript.TakeDamage(-damage);
                    heroAttacker.characterScriptableObject.AddHealExp(damage);
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    return;
                }
            }

            heroScript.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (minionScript != null)
        {
            if (minionAttacker != null)
            {
                return;
            }
            minionScript.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (dungeonMasterScript != null)
        {
            if (masterAttacker != null)
            {
                return;
            }
            dungeonMasterScript.TakeDamage(damage);
        }

        if (other.CompareTag("Wall")) Destroy(gameObject);
    }
}
