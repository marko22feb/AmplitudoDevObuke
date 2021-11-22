using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    [Header("Misc")]
    [SerializeField]
    LayerMask floorLayer = default;
    [Header ("Movement Control")]
    [SerializeField]
    float speed;
    [SerializeField]
    float jumpHeight = 30f;
    [SerializeField]
    float airControl = 1f;
    [SerializeField]
    Vector2 velocityClamp = new Vector2(-8f, 8f);

    [HideInInspector]
    public MovementMode movementMode = MovementMode.ground;

    Dictionary<Direction, float> AndroidMovement = new Dictionary<Direction, float>();

    private Direction prevDashDir = default;
    private Animator playerAnimator;
    [HideInInspector]
    public Rigidbody2D rigid;
    private BoxCollider2D boxCollider;
    [HideInInspector]
    public GameController control;
    public bool TestingAndroid = false;
    private float DashDistance = 3f;
    private bool isOnGround = false;
    private InputActions keybinds;
    private InputAction movementAction;

    private void Start()
    {

        Application.targetFrameRate = 100;
        keybinds = InputController.inputs.keybindings;
        movementAction = keybinds.Actions.Movement;
      //  movementAction.started += DetectDoubleClick;
    }

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        control = GameObject.Find("GameController").GetComponent<GameController>();
        boxCollider = GetComponent<BoxCollider2D>();

        AndroidMovement.Add(Direction.forward, 0);
        AndroidMovement.Add(Direction.backward, 0);
        AndroidMovement.Add(Direction.left, 0);
        AndroidMovement.Add(Direction.right, 0);
    }

    private void Update()
    {
        CheckForGroundCollision();
        CheckForBoard(1);

        if (GameController.control.IsInputEnabled)
        {
            if (Application.platform != RuntimePlatform.Android && !TestingAndroid)
            {
                bool down = false;
                if (isOnGround && movementMode == MovementMode.ground)
                {
                    if (movementAction.ReadValue<Vector2>().y < 0) down = true;
                }

                if (!down && movementMode == MovementMode.ground)
                {
                    bool Detected = false;
                    if (Input.GetKeyUp(KeyCode.A)) DetectDoubleClick(Direction.left, out Detected);
                    if (Detected) return;
                    if (Input.GetKeyUp(KeyCode.D)) DetectDoubleClick(Direction.right, out Detected);
                    if (Detected) return;

                    float ver = 0;
                    if (movementAction.ReadValue<Vector2>().y > 0)
                    {
                        ver = movementAction.ReadValue<Vector2>().y;
                    }
                    Movement(movementAction.ReadValue<Vector2>().x, ver);
                } else
                {
                    CheckForBoard(-2);
                }

                if (movementMode == MovementMode.ladder)
                {
                    if (movementAction.ReadValue<Vector2>().y < 0)
                    {
                        CheckForGroundCollision();
                        if (isOnGround)
                        {
                            ExitLadder(new Vector3 (0, 0, 0));
                        }
                    }
                    LadderMovement(movementAction.ReadValue<Vector2>().y);
                }

                SetCrouched(down);
            }
            else
            {
                if (movementMode == MovementMode.ground)
                Movement(AndroidMovement[Direction.left] + AndroidMovement[Direction.right], AndroidMovement[Direction.forward] + AndroidMovement[Direction.backward]);
            }
        }

        playerAnimator.SetFloat("HorizontalVelocity", rigid.velocity.x);
        playerAnimator.SetFloat("VerticalVelocity", rigid.velocity.y);
    }

    public void Movement(float horizontal, float vertical)
    {
        if (rigid.velocity.x != 0) if (rigid.velocity.x < 0) transform.rotation = new Quaternion(0, 180, 0, 0); else transform.rotation = new Quaternion(0, 0, 0, 0);

        if (isOnGround)
        {
            float sprintSpeed = speed;
            if (Input.GetKey(KeyCode.LeftShift)) sprintSpeed = speed * 2;

            if (vertical > 0) vertical = jumpHeight; else vertical = rigid.velocity.y;

            rigid.velocity = new Vector2(horizontal * sprintSpeed * Time.deltaTime, vertical);
        } else
        {
            rigid.velocity += new Vector2(horizontal * airControl, 0);
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, velocityClamp.x, velocityClamp.y), rigid.velocity.y);
        }
    }

    public void LadderMovement(float vertical)
    {
        rigid.velocity = new Vector2(0, vertical * speed * Time.deltaTime);
    }

    public void ExitLadder(Vector3 offset)
    {
        transform.position = GetComponent<BoxCollider2D>().bounds.center + offset;
        movementMode = MovementMode.ground;
        playerAnimator.SetBool("IsOnLadder", false);
        rigid.gravityScale = 3;
        GetComponent<CapsuleCollider2D>().size = new Vector2(1.15f, 1.5f);
        GetComponent<BoxCollider2D>().size = new Vector2(0.85f, 0.9f);
        rigid.velocity = new Vector2(0, 0);
    }

    public void SetCrouched (bool crouch)
    {
        playerAnimator.SetBool("IsCrouched", crouch);
    }

    public void SetAndroidMovement(Direction direction, float value)
    {
        AndroidMovement[direction] = value;
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
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, .3f, floorLayer);
        if (hit.collider != null) isOnGround = true; else isOnGround = false;
        playerAnimator.SetBool("IsOnGround", isOnGround);
    }

    private void CheckForBoard(float direction)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y + direction), floorLayer);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Board")
            {
                hit.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }

    }

    IEnumerator ResetDoubleClick()
    {
        yield return new WaitForSeconds(0.2f);
        prevDashDir = Direction.forward;
    }
}
