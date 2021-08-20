using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    GameController control;

    private void Awake()
    {
       control = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            control.OnCoinsPickUp(1);
            Destroy(gameObject);
        }
    }
}
