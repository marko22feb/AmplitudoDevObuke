using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public InteractType interactType;
    float DoOnceDuration = 1f;

    [HideInInspector]
    public bool DoOnce = false;

    private void Awake()
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
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        GameController.control.Player.GetComponent<PlayerInput>().ActivateInteract(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
            GameController.control.Player.GetComponent<PlayerInput>().ActivateInteract(null);
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
