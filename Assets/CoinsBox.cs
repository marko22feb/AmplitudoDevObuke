using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsBox : MonoBehaviour
{
    public GameObject Coin;
    public int MaxCoins;
    public Vector3 VelocityRange;

    public Sprite FullSprite;
    public Sprite EmptySprite;

    private SpriteRenderer sr;

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (MaxCoins == 0) sr.sprite = EmptySprite; else sr.sprite = FullSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (MaxCoins <= 0) return;

        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.y >= transform.position.y) return;
            MaxCoins--;
            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject temp = Instantiate(Coin, SpawnPosition, Quaternion.identity);

            float possiblex = Random.Range(VelocityRange.x, -1f);
            float possibley = Random.Range(1f, VelocityRange.y);

            int i = Random.Range(0, 2);
            bool random = false;
            if (i == 1) random = true;

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(random ? possiblex:possibley, VelocityRange.z);
            if (MaxCoins <= 0) sr.sprite = EmptySprite;
        }
    }
}
