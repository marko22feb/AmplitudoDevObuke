using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEvents : MonoBehaviour
{
    Weapon weapon;

    public void OnAttackStart()
    {
        if (weapon == null) weapon = transform.GetChild(0).GetComponent<Weapon>();
        weapon.enabled = true;
        weapon.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void OnAttackEnd()
    {
        weapon.enabled = false;
        weapon.ResetAttack();
        weapon.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().SetTrigger("AttackFinished");
    }
}
