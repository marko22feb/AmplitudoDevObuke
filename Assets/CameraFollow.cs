using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField]
    Vector3 offset;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        float dampTime = 0.15f;
        Vector3 velocity = Vector3.zero;
        float followSpeed = Vector2.Distance(transform.position, playerTransform.position) > 5 ? 0.15f : 0.015f;
        Vector3 newPosition = Vector3.Lerp(transform.position, playerTransform.position + offset, followSpeed);
        transform.position = 

        Vector3.SmoothDamp(transform.position, playerTransform.position + offset, ref velocity, dampTime);
    }
}
