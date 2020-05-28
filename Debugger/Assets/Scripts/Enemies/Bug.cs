using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : Enemy
{
    public override void Update()
    {
        base.Update();
        float angle = -Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, -this.transform.forward);
    }

    public override void Move()
    {
        base.Move();
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y));
    }

    public override string ToString()
    {
        return "Bug";
    }
}
