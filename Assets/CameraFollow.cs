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
       Vector3 newPosition = Vector3.Lerp(transform.position, playerTransform.position + offset, 0.05f);
        transform.position = newPosition;
    }
}
