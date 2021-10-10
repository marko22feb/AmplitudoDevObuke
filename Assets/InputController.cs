using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController inputs;
    public InputActions keybindings;

    private void Awake()
    {
        if (inputs == null) inputs = this;

        keybindings = new InputActions();
        keybindings.Actions.Enable();
    }

}
