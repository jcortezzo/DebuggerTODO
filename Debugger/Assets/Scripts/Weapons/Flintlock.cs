using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flint : Weapon
{
    public override void Attack()
    {
        if (!IsCD())
        {
            cdTimer = cd;
            Projectile bullet = Instantiate(projectile, muzzle.position, Quaternion.identity);
            bullet.direction = new Vector2(direction.x + Random.Range(-spread, spread), direction.y + Random.Range(-spread, spread));
            bullet.alignment = alignment;
            ShakeCamera();
        }
        else
        {
            return;
        }
        //throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return "Flintlock";
    }

}