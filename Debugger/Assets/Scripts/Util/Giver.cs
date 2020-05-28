using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Giver : MonoBehaviour
{
    protected bool playerCollision = false;
    protected Player player;

    public virtual void Start()
    {

    }
    public abstract GameObject Get();

    private void OnTriggerStay2D(Collider2D collision)
    {
        detectPlayer(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectPlayer(collision);
    }

    private void detectPlayer(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCollision = true;
            //Debug.Log("collided with player");
            player = collision.gameObject.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCollision = false;
        }
    }
}