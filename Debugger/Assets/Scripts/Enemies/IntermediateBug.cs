using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermediateBug : Bug
{
    [SerializeField] private const float TOWARDS_TIME = 4f;
    [SerializeField] private const float AWAY_TIME = 2f;
    [SerializeField] private float timer;

    private bool isTowards;

    protected override void Start()
    {
        base.Start();
        timer = TOWARDS_TIME;
        isTowards = true;
    }

    public override void Update()
    {
        base.Update();
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        } else
        {
            timer = isTowards ? AWAY_TIME : TOWARDS_TIME;
            isTowards = !isTowards;
        }
    }

    public bool IsTowards()
    {
        return isTowards;
    }

    public override void GetDirectionalInput()
    {
        base.GetDirectionalInput();
        if (!IsTowards())
        {
            direction = -direction;
        }
    }

    public override string ToString()
    {
        return "Intermediate Bug";
    }
}
