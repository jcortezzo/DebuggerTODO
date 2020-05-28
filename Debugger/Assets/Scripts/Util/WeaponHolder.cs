using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    //public LevelManager levelManager;
    public LivingEntity owner;
    public Weapon primary;
    public Weapon secondary;
    public SpriteRenderer weaponSR;
    public GameObject itemSpawn;

    //public Rigidbody2D rb; // j code

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<LivingEntity>();
        if (owner is Player) // TODO: BADDDD FIX LATER -- separate the classes out make the oop gods happy
        {
            GameObject p = ItemIndexer.Instance.InstantiateWeapon(GlobalValues.Instance.primary);
            GameObject s = ItemIndexer.Instance.InstantiateWeapon(GlobalValues.Instance.secondary);
            Debug.Log(p + " " + s);
            if (p != null) AddPrimary(p.GetComponent<Weapon>());
            if (s != null) AddSecondary(s.GetComponent<Weapon>());
        }
        if (primary != null)
        {
            weaponSR = primary.GetComponent<SpriteRenderer>();
            primary.alignment = owner.alignment;
        }
    }

    public void SwitchWeapon()
    {
        if (primary != null && secondary != null)
        {
            secondary.GetComponent<SpriteRenderer>().flipY = primary.GetComponent<SpriteRenderer>().flipY;
        }
        Weapon temp = primary;
        primary = secondary;
        secondary = temp;

        if (primary != null) ToggleColor(primary.GetComponent<SpriteRenderer>(), 1);
        if (secondary != null) ToggleColor(secondary.GetComponent<SpriteRenderer>(), 0);
        weaponSR = (primary != null) ? primary.GetComponent<SpriteRenderer>() : null;
    }

    public void DropWeapon(bool prim)
    {
        Weapon wp = prim ? primary : secondary;
        if (wp == null) return;
        WeaponGiver wg = Instantiate(itemSpawn, this.transform.position, Quaternion.identity).GetComponent<WeaponGiver>();
        wg.rand = false;
        wg.weaponID = ItemIndexer.Instance.GetIndex(wp.ToString());
        Destroy(wp.gameObject);
    }

    public void AddPrimary(Weapon weapon)
    {
        this.primary = weapon;
        weapon.transform.parent = this.transform;

        primary.transform.localPosition = new Vector3(CalculateXOffset(weapon.transform.localScale.x), 0f, this.transform.position.z);
        primary.transform.localRotation = Quaternion.identity;
        weaponSR = primary.GetComponent<SpriteRenderer>();
        Debug.Log(owner == null);
        weaponSR.flipY = owner.flip;

        primary.alignment = owner.alignment;
    }

    public void AddSecondary(Weapon weapon)
    {
        this.secondary = weapon;
        weapon.transform.parent = this.transform;
        secondary.transform.localPosition = new Vector3(CalculateXOffset(weapon.transform.localScale.x), 0f, this.transform.position.z);
        secondary.transform.localRotation = Quaternion.identity;
        SpriteRenderer rd = secondary.GetComponent<SpriteRenderer>();
        secondary.GetComponent<SpriteRenderer>().flipY = owner.flip;
        ToggleColor(rd, 0);

        secondary.alignment = owner.alignment;
    }

    private float CalculateXOffset(float size)
    {
        return (1.0f / 5.0f) * size + (1.0f / 5.0f);//(2.0f / 5.0f) * size + (2.0f / 5.0f);
    }
    public void ToggleColor(SpriteRenderer rd, int val)
    {
        rd.GetComponent<SpriteRenderer>().color = new Color(rd.color.r, rd.color.g, rd.color.b, val);
    }

    private void OnDestroy()
    {
        if (owner is Player)
        {
            GlobalValues.Instance.primary = ItemIndexer.Instance.GetIndex(primary == null ? "Empty" : primary.ToString());
            GlobalValues.Instance.secondary = ItemIndexer.Instance.GetIndex(secondary == null ? "Empty" : secondary.ToString());
        }
    }


}