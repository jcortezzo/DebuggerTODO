using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Defines a LivingEntity, that is, something that moves
 * around and can attack. LivingEntities can carry weapons
 * using their WeaponHolders. How LivingEntities move and
 * attack are determined by the subclass's implementation.
 * LivingEntities also have an alignment.
 */ 
public abstract class LivingEntity : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float health;
    public WeaponHolder weaponHolder;
    public LevelManager levelManager;
    public Vector3 direction;

    protected SpriteRenderer sr;

    public bool flip = false;
    public Animator anim;

    public Quaternion weaponEndRotation;

    public float hitstun;

    public Alignment alignment = Alignment.NEUTRAL;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        weaponHolder = this.GetComponentInChildren<WeaponHolder>();
        weaponHolder.owner = this;
        rb = this.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    public virtual void Update()
    {
        GetDirectionalInput();
        if (hitstun > 0)
        {
            hitstun -= Time.deltaTime;
            sr.material.color = sr.material.color == Color.red ? Color.blue : Color.red;
            if (weaponHolder.primary != null) weaponHolder.primary.attacking = false;
            return;
        }

        // new
        if (weaponHolder.primary != null && weaponHolder.weaponSR != null)
        {
            if (weaponHolder.primary.attacking)
            {
                SwingWeapon();
            }
            else
            {
                RotateWeapon();
            }
        }

        sr.material.color = Color.white;
        RotateSelf();
        Attack();
    }

    public virtual void FixedUpdate()
    {
        if (hitstun <= 0) Move();
    }

    public abstract void Move();

    public virtual void Attack()
    {
        if (IsAttacking())
        {
            if (weaponHolder.primary.attacking) return;
            if (!weaponHolder.primary.IsCD()) weaponHolder.primary.Attack();
            if (weaponHolder.primary.melee) // problematic, will swing even when you're on cooldown
            {
                weaponHolder.primary.attacking = true; // currently shoots at gun end rather than from middle
                weaponHolder.primary.hitbox.enabled = true;
                float angle = -Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                int f = !flip ? 1 : -1;
                weaponEndRotation = Quaternion.AngleAxis(angle + f * (weaponHolder.primary.swingRadius / 2), -this.transform.forward);
                weaponHolder.transform.rotation = Quaternion.AngleAxis(angle - f * (weaponHolder.primary.swingRadius / 2), -this.transform.forward);
            }
        }
    }

    public abstract bool IsAttacking();

    void RotateSelf()
    {
        float angle = -Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if ((angle > 0 && angle < 90) || (angle > -90 && angle < 0))
        {
            if (flip)
            {
                flip = !flip;
                sr.flipX = !sr.flipX;
            }
        }
        else
        {
            if (!flip)
            {
                flip = !flip;
                sr.flipX = !sr.flipX;
            }
        }

    }

    void SwingWeapon()
    {
        if (weaponHolder == null || weaponHolder.primary == null) return;
        weaponHolder.transform.rotation = Quaternion.Lerp(weaponHolder.transform.rotation, weaponEndRotation, Time.deltaTime * weaponHolder.primary.swingSpeed);
        float epsilon = 0.9999f;
        if (Mathf.Abs(Quaternion.Dot(weaponHolder.transform.rotation, weaponEndRotation)) >= epsilon)
        {
            weaponHolder.primary.attacking = false;
            weaponHolder.primary.hitbox.enabled = false;
            // reset weapon pos so it doesn't get stuck
            // in the same attacking orientation
            //GetMouseInput();
            GetDirectionalInput();
            RotateWeapon();
        }
    }

    public abstract void GetDirectionalInput();

    public virtual void RotateWeapon()
    {
        float weaponAngle = -Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponHolder.transform.rotation = Quaternion.AngleAxis(weaponAngle, -this.transform.forward);
        if (weaponAngle > 0)
        {

            weaponHolder.weaponSR.sortingOrder = sr.sortingOrder + 1;
        }
        else
        {

            weaponHolder.weaponSR.sortingOrder = sr.sortingOrder - 1;
        }
        if ((weaponAngle > 0 && weaponAngle < 90) || (weaponAngle > -90 && weaponAngle < 0))
        {
            weaponHolder.weaponSR.flipY = false;
            sr.flipX = false;
        }
        else
        {
            weaponHolder.weaponSR.flipY = true;
            sr.flipX = true;
        }
    }
}