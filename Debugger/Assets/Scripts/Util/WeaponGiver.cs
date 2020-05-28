using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGiver : Giver
{
    public int weaponID;
    public bool rand;
    protected SpriteRenderer sr;
    protected const string PATH = "Images/Weapons/";

    public override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (rand)
        {
            weaponID = ItemIndexer.Instance.GetRandomBaseWeaponID();
        }
        sr.sprite = Resources.Load<Sprite>(PATH + ItemIndexer.Instance.GetName(weaponID));
    }

    public Weapon GetWeapon(Weapon prev, Player player, bool primary)
    {
        GameObject go;

        if (prev == null)
        {
            go = ItemIndexer.Instance.InstantiateWeapon(weaponID);
            return go.GetComponent<Weapon>();
        }
        int prevID = ItemIndexer.Instance.GetIndex(prev.ToString());
        if (!ItemIndexer.Instance.Combinable(prevID, this.weaponID))
        {
            player.weaponHolder.DropWeapon(primary);
            go = ItemIndexer.Instance.InstantiateWeapon(weaponID);
            Destroy(prev.gameObject);
        }
        else
        {
            go = ItemIndexer.Instance.InstantiateCombination(ItemIndexer.Instance.GetIndex(prev.ToString()), weaponID);
            Destroy(prev.gameObject);
        }
        return go.GetComponent<Weapon>();
    }

    public override GameObject Get()
    {
        return ItemIndexer.Instance.InstantiateWeapon(weaponID);
    }

    private void Update()
    {
        if (playerCollision)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Weapon wp = GetWeapon(player.weaponHolder.primary, player, true);
                wp.transform.parent = player.weaponHolder.transform;
                player.weaponHolder.AddPrimary(wp);
                Destroy(this.gameObject);
                playerCollision = false;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Weapon wp = GetWeapon(player.weaponHolder.secondary, player, false);
                wp.transform.parent = player.weaponHolder.transform;
                player.weaponHolder.AddSecondary(wp);
                Destroy(this.gameObject);
                playerCollision = false;
            }
        }
    }

}