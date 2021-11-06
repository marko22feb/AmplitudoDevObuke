using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBehavior : MonoBehaviour
{
    [Header("Behavior Config")]
    [SerializeField]
    private BehaviorType DefaultBehavior;
    [SerializeField]
    private BehaviorConfig IdleBehavior;
    [SerializeField]
    private BehaviorConfig MeleeBehavior;
    [SerializeField]
    private BehaviorConfig RangeBehavior;
    [SerializeField]
    private BehaviorConfig AbilitiesBehavior;

    [HideInInspector]
    public BehaviorConfig currentBehaviorConfig;
    public BehaviorType currentBehaviorType = default;

    [Header("Combat")]
    public NPCState currentState = default;
    public bool canBeStunned = false;


    [Header("Misc")]
    public GameObject Target;
    public float DistanceOfSight = 10;
    private Rigidbody2D rigid;
    public int AmountOfSightRays = 20;
    private bool SightLoss = false;
    [HideInInspector]
    public Lever lever;
    private AbilityComponent ability;
    private bool IsBoss = false;
    private StatComponent stats;

    private void Awake()
    {
        stats = GetComponent<StatComponent>();
        IsBoss = stats.isBoss;
    }

    void Start()
    {
        ability = GetComponent<AbilityComponent>();
        OnBehaviorChange(DefaultBehavior);

        rigid = transform.parent.GetComponent<Rigidbody2D>();
   
        StartCoroutine(OnSight());
       // StartCoroutine(dealDamage());
    }

    void OnBehaviorChange(BehaviorType type)
    {
        if (!stats.isDead)
        {
            currentBehaviorType = type;
            switch (type)
            {
                case BehaviorType.idle:
                    currentBehaviorConfig = IdleBehavior;
                    break;
                case BehaviorType.attackMelee:
                    currentBehaviorConfig = MeleeBehavior;
                    if (IsBoss)
                    {
                        stats.HealthStat.statBar.transform.parent.GetComponent<CanvasGroup>().alpha = 1;
                        stats.HealthStat.statBar.transform.parent.Find("NameText").GetComponent<Text>().text = stats.EnemyName;
                    }
                    break;
                case BehaviorType.attackRanged:
                    currentBehaviorConfig = RangeBehavior;
                    break;
                case BehaviorType.ability:
                    currentBehaviorConfig = AbilitiesBehavior;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnNPCStateChange(NPCState state)
    {
        if (stats.isDead) return;
        
        currentState = state;

        switch (state)
        {
            case NPCState.regular:
                break;
            case NPCState.attacking:
                break;
            case NPCState.stunned:
                StopCoroutine(OnStunned(1f));
                transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                StartCoroutine(OnStunned(5f));
                break;
            case NPCState.scriptedAbility:
                break;
            default:
                break;
        }
    }

    IEnumerator OnSight()
    {
        if (!stats.isDead)
        {
            GameObject tempTarget = null;

            for (int i = 0; i < AmountOfSightRays; i++)
            {
                float vertical = transform.position.y - (AmountOfSightRays / 2) + i;

                RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.rotation.y == 0 ? transform.position.x - DistanceOfSight : transform.position.x + DistanceOfSight, vertical));

                if (hit.collider != null)
                {
                    foreach (string item in currentBehaviorConfig.OnSight.Tags)
                    {
                        if (hit.collider.tag == item)
                        {
                            tempTarget = hit.collider.gameObject;
                            Target = tempTarget;
                            StopCoroutine(OnSightLoss());
                            SightLoss = false;
                            StartCoroutine(FollowTarget());
                            if (currentBehaviorConfig.OnSight.ChangeBehaviorTo != currentBehaviorType)
                            {
                                OnBehaviorChange(currentBehaviorConfig.OnSight.ChangeBehaviorTo);
                            }
                            break;
                        }
                    }
                    if (tempTarget != null)
                        break;
                }

                if (tempTarget == null)
                {
                    if (Target != null && !SightLoss)
                    {
                        StartCoroutine(OnSightLoss());
                    }
                }
            }

            if (Target != null)
                Debug.Log(Target.name);

            yield return new WaitForSeconds(0.2f);
            StopCoroutine(OnSight());
            StartCoroutine(OnSight());
        }
    }

    private IEnumerator OnSightLoss()
    {
        if (!stats.isDead)
        {
            SightLoss = true;
            yield return new WaitForSeconds(2f);

            if (currentBehaviorConfig.OnLoseSight.ChangeBehaviorTo != currentBehaviorType)
            {
                OnBehaviorChange(currentBehaviorConfig.OnLoseSight.ChangeBehaviorTo);
            }

            Target = null;
            SightLoss = false;
        }
    }

    private IEnumerator FollowTarget()
    {
        yield return new WaitForEndOfFrame();

        if (!stats.isDead)
        {
            if (Target == null)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                StopCoroutine(FollowTarget());
            }

            else
            {
                if (Target.transform.position.x < transform.position.x) transform.rotation = new Quaternion(0, 0, 0, 0); else transform.rotation = new Quaternion(0, 180, 0, 0);
                rigid.velocity = new Vector2(Target.transform.position.x < transform.position.x ? -3 : 3, rigid.velocity.y);
                float distance = (Target.transform.position - transform.position).magnitude;

                if (Mathf.Abs(distance) < currentBehaviorConfig.FollowDistance)
                {
                    AttackTarget();
                }
                else
                {
                    StopCoroutine(FollowTarget());
                    StartCoroutine(FollowTarget());
                }
            }
        }
    }

    IEnumerator OnStunned(float Duration)
    {
        if (!stats.isDead)
        {
            yield return new WaitForSeconds(Duration);

            if (currentState == NPCState.stunned)
                currentState = NPCState.regular;
        }
    }

    private void AttackTarget()
    {
        if (stats.isDead) return;
        Debug.Log("We are in attack range");
        rigid.velocity = new Vector2(0, 0);
        StartCoroutine(FollowTarget());
    }

    public void OnHealthTrigger(float percent)
    {
        if (!stats.isDead) { 
        bool satisfiesOperation = false;

            foreach (OnStatTrigger i in currentBehaviorConfig.healthTriggers)
            {
                switch (i.operation)
                {
                    case ArithmeticOperation.lessThen:
                        if (percent < i.Percent)
                            satisfiesOperation = true;
                        break;
                    case ArithmeticOperation.lessOrEqual:
                        if (percent <= i.Percent)
                            satisfiesOperation = true;
                        break;
                    case ArithmeticOperation.equal:
                        if (percent == i.Percent)
                            satisfiesOperation = true;
                        break;
                    case ArithmeticOperation.greaterThen:
                        if (percent > i.Percent)
                            satisfiesOperation = true;
                        break;
                    case ArithmeticOperation.greaterOrEqual:
                        if (percent >= i.Percent)
                            satisfiesOperation = true;
                        break;
                    default:
                        break;
                }

                if (satisfiesOperation)
                {
                    switch (i.action)
                    {
                        case AIActionToTake.usePotion:
                            break;
                        case AIActionToTake.useAbility:
                            ability.OnAbilityUse(i.abilityToUse);
                            canBeStunned = true;
                            lever.IsActive = true;
                            lever.UpdateVisuals();
                            break;
                        case AIActionToTake.changeBehavior:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    public void OnStaminaTrigger(float percent)
    {
        if (stats.isDead) return;
    }

    public void OnManaTrigger(float percent)
    {
        if (stats.isDead) return;
    }

    private IEnumerator dealDamage()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<StatComponent>().ModifyBy(Stats.health, -30);
    }
}
