using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimize : MonoBehaviour
{
    public float OptimizeTime = 0.2f;

    public List<Animator> animators = new List<Animator>();
    public List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();
    public List<MonoBehaviour> scripts = new List<MonoBehaviour>();

    public void Awake()
    {
        if (animators.Count == 0)
        {
            Animator temp = GetComponent<Animator>();
            if (temp != null) animators.Add(temp);

            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    temp = transform.GetChild(i).GetComponent<Animator>();
                    if (temp != null) animators.Add(temp);
                }
            }
        }

        if (rigidbodies.Count == 0)
        {
            Rigidbody2D temp = GetComponent<Rigidbody2D>();
            if (temp != null) rigidbodies.Add(temp);

            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    temp = transform.GetChild(i).GetComponent<Rigidbody2D>();
                    if (temp != null) rigidbodies.Add(temp);
                }
            }
        }
        StartCoroutine(OptimizationBasedOnDistance());
    }

    IEnumerator OptimizationBasedOnDistance()
    {
        yield return new WaitForSeconds(OptimizeTime);
        float distance = Mathf.Abs((transform.position - GameController.control.Player.transform.position).magnitude);
        if (distance > GameController.control.OptimizationDistance)
        {
            foreach (Animator item in animators)
            {
                item.enabled = false;
            }
            foreach (Rigidbody2D item in rigidbodies)
            {
                item.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            foreach (MonoBehaviour item in scripts)
            {
                item.enabled = false;
            }

        } else
        {
            foreach (Animator item in animators)
            {
                item.enabled = true;
            }
            foreach (MonoBehaviour item in scripts)
            {
                item.enabled = true;
            }
            foreach (Rigidbody2D item in rigidbodies)
            {
                item.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

     //   StartCoroutine(OptimizationBasedOnDistance());
    }
}
