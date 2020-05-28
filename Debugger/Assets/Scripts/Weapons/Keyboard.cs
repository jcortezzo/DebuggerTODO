using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : Weapon
{
    public override void Attack()
    {
        if (!IsCD())
        {
            cdTimer = cd;
            return;

        }
        else
        {
            return;
        }
    }

    public override string ToString()
    {
        return "Keyboard";
    }
}