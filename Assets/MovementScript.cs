using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed;

    private void Start()
    {

    }

    private void Awake()
    {


    }

    private void Update()
    {
        float horizontal = 0;
        float vertical = 0;
        float sprintSpeed = speed;

        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;

        if (Input.GetKey(KeyCode.A)) left = true;
        if (Input.GetKey(KeyCode.D)) right = true;
        if (Input.GetKey(KeyCode.W)) up = true;
        if (Input.GetKey(KeyCode.S)) down = true;
        if (Input.GetKey(KeyCode.LeftShift)) sprintSpeed = speed * 2;

        if (!(left && right))
        {
            if (left) horizontal = -1f;
            if (right) horizontal = 1f;
        }

        if (!(up && down))
        {
            if (down) vertical = -1f;
            if (up) vertical = 1f;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal, vertical);
    }

    private void FixedUpdate()
    {
        
    }
}
