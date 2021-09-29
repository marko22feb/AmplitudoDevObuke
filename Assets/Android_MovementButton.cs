using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Android_MovementButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{ 
    MovementScript movement;
    public Direction dir;
    float move = 0;

    private void Start()
    {
        movement = GameController.control.Player.GetComponent<MovementScript>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        move = 0;
        movement.SetAndroidMovement(dir, move);
        if (dir == Direction.backward)
        movement.SetCrouched(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (dir)
        {
            case Direction.forward:
                move = 1;
                break;
            case Direction.backward:
                movement.SetCrouched(true);
                move = -1;
                break;
            case Direction.left:
                move = -1;
                break;
            case Direction.right:
                move = 1;
                break;
            default:
                break;
        }
        movement.SetAndroidMovement(dir, move);
    }
}
