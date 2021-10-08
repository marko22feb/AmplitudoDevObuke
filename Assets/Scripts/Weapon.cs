using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int NumberOfPoints;

    public LayerMask layer;
    public List<Vector3> LastPositions = new List<Vector3>();
    public List<Transform> DetectionPoints = new List<Transform>();

    public List<StatComponent> EnemiesHit = new List<StatComponent>(); 

    void Update()
    {
   
        int index = 0;

        foreach (Transform item in DetectionPoints)
        {
            if (LastPositions.Count > index)
            {
                Debug.Log("Hit");
                RaycastHit2D hit = Physics2D.Linecast(item.position, LastPositions[index], layer);

                if (hit.collider != null)
                {
                    
                    StatComponent currentHit = hit.transform.gameObject.GetComponent<StatComponent>();
                    if (currentHit != null)
                    {
                        bool alreadyHit = false;
                        foreach (StatComponent stat in EnemiesHit)
                        {
                            if (stat == currentHit)
                            {
                                alreadyHit = true;
                                break;
                            }
                        }
                        if (!alreadyHit) {
                            EnemiesHit.Add(currentHit);
                            currentHit.ModifyBy(Stats.health, -transform.parent.parent.GetComponent<StatComponent>().DamageAmount);
                        }
                    }
                }
            }
            else
            {
                LastPositions.Add(item.position);
            }

            LastPositions[index] = item.position;
            index++;
        }
    }

    public void ResetAttack()
    {
        EnemiesHit.Clear();
        LastPositions.Clear();
    }
}
