using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;
    int IsFacingLeft = 1;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        IsFacingLeft = transform.rotation.y == 0 ? 1 : -1;
        StartCoroutine(DestroySelf());
    }

    void Update()
    {
        rigid.velocity = transform.right * 900 * Time.deltaTime * IsFacingLeft;
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
