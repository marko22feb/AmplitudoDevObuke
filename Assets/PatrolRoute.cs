using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PatrolRoute : MonoBehaviour
{
    public LayerMask layer;
    public List<bool> hits;


#if UNITY_EDITOR
    private void Update()
    {
        hits.Clear();
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position, layer);
            hits.Add(hit.collider == null);
        }
    }
#endif

    private void OnDrawGizmos()
    {

        if (hits.Count == 0) return;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = hits[i] ? Color.blue : Color.red;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (i > hits.Count - 1) 
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = hits[i] ? Color.blue : Color.red;
            }
            Gizmos.DrawSphere(transform.GetChild(i).transform.position, 0.1f);
        }
    }
}

