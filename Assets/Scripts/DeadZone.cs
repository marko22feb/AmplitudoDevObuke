using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<StatComponent>() != null)
        {
            collision.GetComponent<StatComponent>().ModifyBy(Stats.health, -2147483647);
        }
    }
}
