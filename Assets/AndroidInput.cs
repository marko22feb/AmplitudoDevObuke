using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AndroidInput : MonoBehaviour, IPointerClickHandler
{
    [System.Serializable]
    public enum InputType {shoot, consumableLeft, consumableRight, melee, inventory, interact};
    public InputType inputType;
    private PlayerInput playerInput;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playerInput == null) playerInput = GameController.control.Player.GetComponent<PlayerInput>();

        switch (inputType)
        {
            case InputType.shoot:
                break;
            case InputType.consumableLeft:
                playerInput.UseConsumable(0);
                break;
            case InputType.consumableRight:
                playerInput.UseConsumable(1);
                break;
            case InputType.melee:
                playerInput.Melee();
                break;
            case InputType.inventory:
                playerInput.ClickedInventory();
                break;
            case InputType.interact:
                playerInput.Interact();
                break;
            default:
                break;
        }
    }
}
