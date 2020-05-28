using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedBug : IntermediateBug
{
    [SerializeField] private const float ATTACK_TIME = 2f;
    [SerializeField] private float cooldownTimer;
    private bool ready;

    protected override void Start()
    {
        base.Start();
        cooldownTimer = 0;
        ready = true;
    }

    public override void Update()
    {
        base.Update();
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        } else
        {
            //cd = !cd;
        }
    }

    public override bool IsAttacking()
    {
        //if (cd) return false;
        if (base.IsAttacking() && ready) cooldownTimer = ATTACK_TIME;
        ready = false;
        return base.IsAttacking();
    }

    public override string ToString()
    {
        return "Advanced Bug";
    }
}
