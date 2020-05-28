using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a Projectile, which is an entity
 * shot from something (often a weapon). Projectiles
 * will move towards a given direction at a given speed.
 */ 
public abstract class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public Vector2 direction;
    public float lifeSpan;
    public Rigidbody2D rb;
    public float knockback;
    public float hitstun;
    public Alignment alignment;

    /**
     * Initialized a new Projectile by giving a direction and
     * setting its rotation to correspond with its direction
     */
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction.Normalize();
        float angle = -Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, -this.transform.forward);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (IsExpired())
        {
            EndLife();
        }
        Move();
    }

    /**
     * Projectiles will expire when they hit a wall, enemy,
     * or a breakable object.
     */
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        LivingEntity e = collision.gameObject.GetComponent<LivingEntity>();
        Breakable b = collision.gameObject.GetComponent<Breakable>();
        if (collision.gameObject.CompareTag("Wall"))
        {
            EndLife();
        }
        else if (e != null &&
                 (alignment == Alignment.NEUTRAL || e.alignment != alignment))
        {
            Damage(e);
            EndLife();
        }
        else if (b != null)
        {
            b.TakeHit(damage, hitstun);
            EndLife();
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
            rb.freezeRotation = true;
        }
    }

    /**
     * Inflicts self's damage on a given LivingEntity e 
     */
    public virtual void Damage(LivingEntity e)
    {
        e.health -= this.damage;
        e.hitstun = hitstun;
        e.GetComponent<Rigidbody2D>().AddForce(direction * knockback);
    }

    public virtual void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    // legacy code, deprecated by OnDestroy()
    public virtual void EndLife()
    {
        Destroy(this.gameObject);
    }

    /**
     * Advances this in direction by speed
     */ 
    public virtual void Move()
    {
        rb.velocity = direction * speed;
    }

    /**
     * Determines whether a Projectile is
     * expired or not (its lifespan runs out)
     */
    public bool IsExpired()
    {
        return lifeSpan <= 0;
    }
}