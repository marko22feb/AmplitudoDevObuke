using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Android_MovementButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{ 
    MovementScript movement;
    public float direction = 1;
    float move = 0;

    private void Start()
    {
        movement = GameController.control.Player.GetComponent<MovementScript>();
    }

    private void Update()
    {
        movement.Movement(move, 0);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        move = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        move = 1;
    }
}
