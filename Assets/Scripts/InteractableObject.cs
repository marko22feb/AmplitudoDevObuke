using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public InteractType interactType;
    float DoOnceDuration = 1f;
    [HideInInspector]
    public GameObject Player;

    [HideInInspector]
    public bool DoOnce = false;

    private void Start()
    {
        switch (interactType)
        {
            case InteractType.none:
                break;
            case InteractType.Door:
                DoOnceDuration = 1f;
                break;
            case InteractType.Lever:
                DoOnceDuration = 0.4f;
                break;
            case InteractType.Ladders:
                DoOnceDuration = 2f;
                break;
            case InteractType.Dialogue:
                DoOnceDuration = 2f;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!collision.isTrigger)
            {
                Player = collision.gameObject;
                if (Player.GetComponent<MovementScript>().movementMode != MovementMode.ladder)
                {
                    Player.GetComponent<PlayerInput>().ActivateInteract(this);
                    OnTriggerCheck(collision);
                }
            }
        }
    }

    public virtual void OnTriggerCheck (Collider2D collision)
    {

    }

    public virtual void OnTriggerCheckExit (Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!collision.isTrigger)
            {
                collision.gameObject.GetComponent<PlayerInput>().ActivateInteract(null);
                OnTriggerCheckExit(collision);
                Player = null;
            }
        }
    }

    public virtual void OnInteract()
    {
        if (DoOnce) return;
        StartCoroutine(IdiotProof());
    }

    IEnumerator IdiotProof()
    {
        DoOnce = true;
        yield return new WaitForSeconds(DoOnceDuration);
        DoOnce = false;
    }
}
