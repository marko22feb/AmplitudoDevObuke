using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : InteractableObject
{
    public bool isTop = false;

    private void Awake()
    {
        interactType = InteractType.Ladders;
    }
    public override void OnInteract()
    {
        if (Player.GetComponent<MovementScript>().movementMode == MovementMode.ground)
        {
            base.OnInteract();
            Player.transform.position = GetComponent<BoxCollider2D>().bounds.center;
            Player.GetComponent<Animator>().SetBool("IsOnLadder", true);
            Player.GetComponent<Animator>().SetTrigger("OnLadderEnter");
            Player.GetComponent<Rigidbody2D>().gravityScale = 0;
            Player.GetComponent<CapsuleCollider2D>().size = new Vector2(0.5f, 1.5f);
            Player.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.9f);
            Player.GetComponent<MovementScript>().movementMode = MovementMode.ladder;
        }
    }

    public override void OnTriggerCheckExit(Collider2D collision)
    {
        if (!DoOnce)
        {
            if (isTop && collision.gameObject.GetComponent<MovementScript>().movementMode == MovementMode.ladder)
            {
                Vector3 offset = new Vector3(0, Player.GetComponent<CapsuleCollider2D>().size.y / 2 + 0.6f, 0);

                collision.gameObject.GetComponent<MovementScript>().ExitLadder(offset);
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }
}
