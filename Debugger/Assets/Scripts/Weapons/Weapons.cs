using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents the abstract form of a weapon. Abstract because
 * this doesn't represent what any particular weapon is, but
 * rather the default behavior and some needed implementation
 * for something to be considered a "weapon." Weapons should be
 * able to attack, though the implementation of this feature
 * depends on the subclass being defined. Weapons can also
 * come in melee form, in which case they can be swung. Being
 * melee / ranged is not mutually exclusive, that is, a weapon
 * can both be ranged and melee.
 */ 
public abstract class Weapon : MonoBehaviour
{
    public float damage;
    public bool melee;
    public float cd;
    public float cdTimer;
    public Vector2 direction;
    public Transform muzzle;
    public Projectile projectile;
    public float spread;
    public float swingSpeed;
    public float swingRadius;
    public bool attacking = false;
    public BoxCollider2D hitbox;
    public float knockback;
    public float hitstun;
    public float weight;
    protected CameraController cam;
    protected const float SHAKE_TIME = 0.05f;
    protected Animator anim;
    public Alignment alignment;
    protected WeaponHolder weaponHolder;

    // Start is called before the first frame update
    public virtual void Start()
    {
        muzzle = this.transform.GetChild(0);
        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
        cam = FindObjectOfType<CameraController>();
        anim = GetComponent<Animator>();
        weaponHolder = GetComponentInParent<WeaponHolder>(); // sketch
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (cdTimer > 0)
        {
            cdTimer -= Time.deltaTime;
        }
    }

    public abstract void Attack();

    /**
     * Determines whether self is in cooldown or not
     */ 
    public bool IsCD()
    {
        return cdTimer > 0;
    }

    /**
     * Inflicts damage on enemy or breakable if the weapon
     * is being swung. Has no effect if the weapon isn't
     * being swung
     */
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attacking) return;
        LivingEntity e = collision.gameObject.GetComponent<LivingEntity>();
        Breakable b = collision.gameObject.GetComponent<Breakable>();
        if (e != null &&
            (alignment == Alignment.NEUTRAL || e.alignment != alignment))
        {
            Damage(e);
        }
        else if (b != null)
        {
            b.TakeHit(damage, hitstun);
        }
    }

    /**
     * Inflicts damage amoung of damage to given
     * LivingEntity e
     */ 
    public virtual void Damage(LivingEntity e)
    {
        e.health -= this.damage;
        e.hitstun = hitstun;
        e.GetComponent<Rigidbody2D>().AddForce(direction * knockback);
    }

    /**
     * Shakes the camera for SHAKE_TIME
     * by weight amount
     */ 
    protected void ShakeCamera()
    {
        this.cam.Shake((transform.position - muzzle.position).normalized, weight, SHAKE_TIME);
    }
}