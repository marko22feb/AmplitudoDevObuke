using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    private bool CanDealDamage = true;
    private float iFrames = 2f;
    public bool isTrap = false;
    public float DamageByTrap = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CanDealDamage) return;
        StatComponent stats = GetComponent<StatComponent>();
        if (stats == null) return;
        stats.ModifyBy(Stats.health, isTrap? -DamageByTrap : -collision.GetComponent<StatComponent>().DamageAmount);
        StartCoroutine(DelayDamage());
        StartCoroutine(iFramesAnim(gameObject));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CanDealDamage) return;
        StatComponent stats = collision.gameObject.GetComponent<StatComponent>();
        if(stats != null)
        {
            stats.ModifyBy(Stats.health, isTrap ? -DamageByTrap : -GetComponent<StatComponent>().DamageAmount);
            StartCoroutine(DelayDamage());
            StartCoroutine(iFramesAnim(collision.gameObject));
        }
    }

    IEnumerator DelayDamage()
    {
        CanDealDamage = false;
        yield return new WaitForSeconds(iFrames);
        CanDealDamage = true;
    }

    IEnumerator iFramesAnim(GameObject target)
    {
        target.GetComponent<Animator>().SetTrigger("IsDamaged");
        yield return new WaitForSeconds(iFrames);
        target.GetComponent<Animator>().SetTrigger("DamageOver");
    }
}
