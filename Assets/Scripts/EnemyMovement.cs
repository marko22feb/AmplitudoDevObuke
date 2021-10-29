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

    private bool MovementInterrupted;
    public float speed;

    private Direction movementDir = Direction.left;
    Rigidbody2D rigid;

    public float AcceptablePatrolRadius;
    private Vector3 LocationToMoveTo;
    private int CurrentPatrolPoint;
    private int patrolPointCount;
    private bool reachedEndOfPatrol = false;
    private AIBehavior behavior;
    public Transform patrolRoute;
    

    void Awake()
    {
        rigid = transform.parent.GetComponent<Rigidbody2D>();
        behavior = GetComponent<AIBehavior>();
    }

    private void Start()
    {
      if (movementType == MovementType.patrol)
        {
            if (patrolRoute == null) {
                throw new System.NotImplementedException();
            }
            patrolPointCount = patrolRoute.childCount;
            GetClosestPatrolPoint();
        }

        StartCoroutine(DelayedOptimization(2f));
    }

    private void Movement()
    {
        if (behavior.currentBehaviorType != BehaviorType.idle || behavior.currentState == NPCState.stunned)
        {
            MovementInterrupted = true;
            return;
        }

        if (MovementInterrupted)
        {
            if (movementDir == Direction.left && transform.rotation.y != 0) ChangeDirection(Direction.right);
            if (movementDir == Direction.right && transform.rotation.y != 180) ChangeDirection(Direction.left);
            MovementInterrupted = false;
        }

        switch (movementType)
        {
            case MovementType.patrol:
                if (reachedEndOfPatrol)
                {
                    rigid.velocity = new Vector2(0, 0);
                    return;
                }

                Vector2 DistanceToPatrolPoint = transform.position - patrolRoute.GetChild(CurrentPatrolPoint).position;
                float distanceToCheck = Mathf.Abs(DistanceToPatrolPoint.magnitude);

                if (distanceToCheck < AcceptablePatrolRadius)
                {
                    GetNextPatrolPoint();
                    return;
                }

                rigid.velocity = new Vector2(movementDir == Direction.left ? -speed * Time.deltaTime : speed * Time.deltaTime, rigid.velocity.y);
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

    private void ChangeDirection(Direction newDir)
    {
        movementDir = newDir;
        transform.localRotation = new Quaternion(0, movementDir == Direction.left ? 0 : 180, 0, 0);
    }

    private bool CanMoveThere(Direction dir)
    {
        RaycastHit2D hit = Physics2D.Linecast(raytraceStart.position, raytraceStart.position + new Vector3(0, -2, 0), layer);

        RaycastHit2D forwardHit = Physics2D.Linecast(raytraceStart.position, raytraceStart.position + new Vector3(movementDir == Direction.left ? -0.05f : 0.05f, 0, 0), layer);

        if (hit.collider == null || forwardHit.collider != null) return false; else return true;
    }

    private void GetClosestPatrolPoint()
    {
        float distance = Mathf.Infinity;

        for (int i = 0; i < patrolPointCount; i++)
        {
            Vector2 length = transform.position - patrolRoute.GetChild(i).position;
            float distanceToCheck = Mathf.Abs(length.magnitude);

            if (distanceToCheck < distance)
            {
                distance = distanceToCheck;
                CurrentPatrolPoint = i;
                LocationToMoveTo = patrolRoute.GetChild(i).position;
                ChangeDirection(transform.position.x < LocationToMoveTo.x ? Direction.right : Direction.left);
            }
        }
    }

    private void GetNextPatrolPoint()
    {
        int nextPatrolRoute = CurrentPatrolPoint + (movementDir == Direction.left ? -1 : 1);

        if (nextPatrolRoute >= patrolPointCount)
        {
            switch (patrolType)
            {
                case PatrolType.looping:
                    nextPatrolRoute = 0;
                    break;
                case PatrolType.backAndForth:
                    nextPatrolRoute = patrolPointCount - 2;
                    break;
                case PatrolType.single:
                    nextPatrolRoute = patrolPointCount - 1;
                    reachedEndOfPatrol = true;
                    break;
                default:
                    break;
            }
        } else if (nextPatrolRoute < 0)
        {
            switch (patrolType)
            {
                case PatrolType.looping:
                    nextPatrolRoute = patrolPointCount -1;
                    break;
                case PatrolType.backAndForth:
                    nextPatrolRoute = 1;
                    break;
                case PatrolType.single:
                    nextPatrolRoute = 0;
                    reachedEndOfPatrol = true;
                    break;
                default:
                    break;
            }
        }
        ChangeDirection(CurrentPatrolPoint > nextPatrolRoute ? Direction.left : Direction.right);
        CurrentPatrolPoint = nextPatrolRoute;
        LocationToMoveTo = patrolRoute.GetChild(nextPatrolRoute).position; 
    }

    private IEnumerator DelayedOptimization(float duration)
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(Optimization());
    }

    private IEnumerator Optimization()
    {
        
        float optimizeTime = Time.deltaTime;
        
        float distanceToPlayer = Mathf.Abs((transform.position - GameController.control.Player.transform.position).magnitude);
        float alpha = 1;
        if (distanceToPlayer > 5) alpha = 0.2f;

        optimizeTime = Mathf.Lerp(alpha, Time.deltaTime, 0.2f);

        yield return new WaitForSeconds(optimizeTime);
        Movement();
        StartCoroutine(Optimization());
    }
}
