using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour, Breakable
{
    [SerializeField] private float health = 6f;
    private float hitstun = 0f;
    private SpriteRenderer sr;

    public void Break()
    {
        Destroy(this.gameObject);
    }

    public bool IsAlive()
    {
        return health >= 0;
    }

    public void TakeHit(float damage, float hitstun)
    {
        health -= damage;
        this.hitstun += hitstun;
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive()) Break();

        if (hitstun > 0)
        {
            hitstun -= Time.deltaTime;
            sr.material.color = sr.material.color == Color.red ? Color.blue : Color.red;
            return;
        } else
        {
            sr.material.color = Color.white;
        }
    }
}
