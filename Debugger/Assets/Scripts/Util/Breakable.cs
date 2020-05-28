using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Breakable
{
    void Break();

    bool IsAlive();

    void TakeHit(float damage, float hitstun);
}