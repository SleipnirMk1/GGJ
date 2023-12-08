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

        if (heroScript != null)
        {
            heroScript.TakeDamage(damage);
            // refer master to gain xp
        }
        else if (minionScript != null)
        {
            if (heroAttacker != null)
            {
                heroAttacker.exp += damage;
            }
            minionScript.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
