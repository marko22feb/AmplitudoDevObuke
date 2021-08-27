using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    enum Direction {forward, backward, left, right };

    private Direction prevDashDir = default;
    public float speed;
    private Animator playerAnimator;
    private Rigidbody2D rigid;
    public GameController control;

    private void Start()
    {
        Application.targetFrameRate = 100;
    }

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        control = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        float horizontal = rigid.velocity.x;
        float sprintSpeed = speed;

        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;

        if (Input.GetKey(KeyCode.S)) down = true;
        if (!down)
        {
            bool Detected = false;
            if (Input.GetKeyUp(KeyCode.A)) DetectDoubleClick(Direction.left, out Detected);
            if (Detected) return;
            if (Input.GetKeyUp(KeyCode.D)) DetectDoubleClick(Direction.right, out Detected);
            if (Detected) return;
            if (Input.GetKey(KeyCode.A)) left = true;
            if (Input.GetKey(KeyCode.D)) right = true;
            if (Input.GetKey(KeyCode.W)) up = true;
            if (Input.GetKey(KeyCode.LeftShift)) sprintSpeed = speed * 2;
        }

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

    private void DetectDoubleClick (Direction dir, out bool Detected)
    {
        float distance = 0;
        if (dir == prevDashDir)
        {
            if (dir == Direction.left) 
            { 
                distance = -3f; 
            } else 
            { 
                distance = 3f; 
            }

            transform.position = new Vector3(transform.position.x + distance, transform.position.y, 0);
            Detected = true;
        } else
        {
            prevDashDir = dir;
            Detected = false;
            StartCoroutine(ResetDoubleClick());
        }
    }

    IEnumerator ResetDoubleClick()
    {
        yield return new WaitForSeconds(0.4f);
        prevDashDir = Direction.forward;
    }
}
