using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed;
    private Animator playerAnimator;
    private Rigidbody2D rigid;

    private void Start()
    {
        Application.targetFrameRate = 100;
    }

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = rigid.velocity.x;
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
            if (left) horizontal = -1f * sprintSpeed * Time.deltaTime;
            if (right) horizontal = 1f * sprintSpeed * Time.deltaTime;
        }

        if (!(up && down))
        {
            if (down) { playerAnimator.SetBool("IsCrouched", true); }
        }

        if (!down) { playerAnimator.SetBool("IsCrouched", false); }

        if (left || right)
        {
            if (horizontal < 0) transform.rotation = new Quaternion(0, 180, 0, 0); else transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal, rigid.velocity.y);
        playerAnimator.SetFloat("HorizontalVelocity", horizontal);
    }

    private void FixedUpdate()
    {
        
    }
}
