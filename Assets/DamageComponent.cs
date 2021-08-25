using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [SerializeField]
    float DamageAmount = 50;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StatComponent stats = collision.gameObject.GetComponent<StatComponent>();
        if(stats != null)
        {
            stats.ModifyBy(-DamageAmount);
        }
    }
}
