using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    enum Direction {forward, backward, left, right };
    [SerializeField]
    LayerMask floorLayer = default;
    private Direction prevDashDir = default;
    public float speed;
    private Animator playerAnimator;
    private Rigidbody2D rigid;
    private BoxCollider2D boxCollider;
    public GameController control;
    private float DashDistance = 3f;
    private bool isOnGround = false;

    private void Start()
    {
        Application.targetFrameRate = 100;
    }

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        control = GameObject.Find("GameController").GetComponent<GameController>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float horizontal = rigid.velocity.x;
        float vertical = rigid.velocity.y;
        float sprintSpeed = speed;

        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;

        CheckForGroundCollision();

        if (isOnGround)
        {
            if (Input.GetKey(KeyCode.S)) down = true;
        }

        if (!down)
        {
            bool Detected = false;
            if (Input.GetKeyUp(KeyCode.A)) DetectDoubleClick(Direction.left, out Detected);
            if (Detected) return;
            if (Input.GetKeyUp(KeyCode.D)) DetectDoubleClick(Direction.right, out Detected);
            if (Detected) return;
            if (Input.GetAxis("Horizontal") < 0) left = true;
            if (Input.GetAxis("Horizontal") > 0) right = true;

            if (isOnGround)
            {
                if (Input.GetKey(KeyCode.W)) up = true;
                if (Input.GetKey(KeyCode.LeftShift)) sprintSpeed = speed * 2;
            }
        }

        if (!(left && right))
        {
            if (!isOnGround) sprintSpeed = speed * 0.5f;
            if (left) horizontal = -sprintSpeed * Time.deltaTime;
            if (right) horizontal = sprintSpeed * Time.deltaTime;
        }

        if (!(up && down))
        {
            if (down) { playerAnimator.SetBool("IsCrouched", true); }
            if (up)
            {
                vertical = rigid.velocity.y + 4f;
            }
        }

        if (!down) { playerAnimator.SetBool("IsCrouched", false); }

        if (left || right)
        {
            if (horizontal < 0) transform.rotation = new Quaternion(0, 180, 0, 0); else transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal, vertical);
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
            if (!CanDashThere(dir)) {
                Detected = false;
                return; 
            }

            if (dir == Direction.left) 
            { 
                distance = -DashDistance; 
            } else 
            { 
                distance = DashDistance; 
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

    private bool CanDashThere(Direction dir)
    {
        bool left = false;
        if (dir == Direction.left) left = true;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, left ? Vector2.left : Vector2.right, 3f, floorLayer);

        if (hit.collider != null)
        {
            
            DashDistance = hit.distance - 0.575f;
            Debug.Log(DashDistance);
            return false;
        }
        else
        {
            DashDistance = 3f;
            return true;
        }
    }

    private void CheckForGroundCollision()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.2f, floorLayer);
        if (hit.collider != null) isOnGround = true; else isOnGround = false;
        Debug.Log(isOnGround);
    }

    IEnumerator ResetDoubleClick()
    {
        yield return new WaitForSeconds(0.4f);
        prevDashDir = Direction.forward;
    }
}
