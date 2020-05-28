using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Enemy : LivingEntity
{
    int currentWayPoint = 0;
    bool reachEndOfPath;
    float nextWaypointDistance = 3f;

    public Player player;
    public LivingEntity target;
    public float sight;
    public float attackingDistance;
    public float attackSpeed = 0.2f;
    public float attackTimer = 0;
    public int TargetSize { get { return targets.Count; } }
    public ISet<LivingEntity> targets;
    public ISet<LivingEntity> friendlyTargets;

    private const string WEAPON_GIVER_LOC = "Prefabs/Weapons/WeaponGiver";

    // Start is called before the first frame update
    private bool projectileWeapon;
    private float initialDamage;

    protected override void Start()
    {
        base.Start();
        float primaryDamage = weaponHolder.primary.damage;
        projectileWeapon = weaponHolder.primary.projectile != null;
        initialDamage = projectileWeapon ? weaponHolder.primary.projectile.damage : primaryDamage;

        player = LevelManager.Instance.player;
        targets = new HashSet<LivingEntity>();
        friendlyTargets = new HashSet<LivingEntity>();

        GameObject child = new GameObject();
        child.transform.parent = this.transform;
        child.transform.localPosition = new Vector3(0, 0, 0);
        child.AddComponent<EnemySight>();
        CircleCollider2D collider = child.AddComponent<CircleCollider2D>();
        collider.radius = sight;
        collider.isTrigger = true;
    }


    public override void Update()
    {
        if (health <= 0)
        {
            EndLife();
        }

        base.Update();
    }

    public virtual void EndLife()
    {
        GlobalValues.Instance.money += speed * 50;
        Destroy(this.gameObject);
    }

    private void DropItem()
    {
        //Debug.Log("Spawn weapon");
        GameObject go = Instantiate(Resources.Load(WEAPON_GIVER_LOC), this.transform.position, Quaternion.identity) as GameObject;

    }

    public override void Move()
    {
        GetDirectionalInput();
        rb.velocity = direction.normalized * speed;
    }


    public override void GetDirectionalInput()
    {
        if (weaponHolder.primary != null && weaponHolder.primary.attacking || target == null) return;
        direction = (target.transform.position - this.transform.position).normalized;
        if (weaponHolder.primary != null && !weaponHolder.primary.attacking)
        {
            weaponHolder.primary.direction = direction;
        }
    }

    public override bool IsAttacking()
    {
        if (target == null) return false;
        float distance = Mathf.Sqrt(Mathf.Pow(target.transform.position.y - this.transform.position.y, 2) +
                                    Mathf.Pow(target.transform.position.x - this.transform.position.x, 2));
        return distance < attackingDistance && !IsFriendTargetingPlayer();
    }

    public bool IsFriendTargetingPlayer()
    {
        return alignment == Alignment.FRIEND && target == player;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
