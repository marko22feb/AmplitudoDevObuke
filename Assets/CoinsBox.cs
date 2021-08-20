using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsBox : MonoBehaviour
{
    public GameObject Coin;
    public int MaxCoins;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (MaxCoins <= 0) return;

        if (collision.gameObject.tag == "Player")
        {
            MaxCoins--;
            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(Coin, SpawnPosition, Quaternion.identity);
        }
    }
}
