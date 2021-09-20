using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void OnInventoryButton()
    {
        GameController.control.Player.GetComponent<PlayerInput>().CanReEnterInventory();
    }
}
