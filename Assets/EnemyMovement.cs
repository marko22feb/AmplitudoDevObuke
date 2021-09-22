using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyType enemyType;
    public MovementType movementType;
    public PatrolType patrolType;
    public Transform raytraceStart;
    public LayerMask layer;

    public float speed;

    private Direction movementDir = Direction.left;
    Rigidbody2D rigid;
    

    void Awake()
    {
        rigid = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.patrol:

                break;
            case MovementType.freestyle:
                if (CanMoveThere(movementDir))
                {
                    rigid.velocity = new Vector2(movementDir == Direction.left ? -speed * Time.deltaTime : speed * Time.deltaTime, rigid.velocity.y);
                }
                else
                {
                    ChangeDirection();
                }
                break;
            default:
                break;
        }
        
    }

    private void ChangeDirection()
    {
        movementDir = movementDir == Direction.left ? Direction.right : Direction.left;
        transform.localRotation = new Quaternion(0, movementDir == Direction.left ? 0 : 180, 0, 0);
    }

    private bool CanMoveThere(Direction dir)
    {
        RaycastHit2D hit = Physics2D.Linecast(raytraceStart.position, raytraceStart.position + new Vector3(0, -2, 0), layer);

        RaycastHit2D forwardHit = Physics2D.Linecast(raytraceStart.position, raytraceStart.position + new Vector3(movementDir == Direction.left ? -0.05f : 0.05f, 0, 0), layer);

        if (hit.collider == null || forwardHit.collider != null) return false; else return true;
    }
}
