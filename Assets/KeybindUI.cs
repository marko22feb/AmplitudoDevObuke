using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeybindUI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference reference;

    [SerializeField]
    [Range (0,20)]
    private int Index;
    [SerializeField]
    private InputActions actions;
    [SerializeField]
    private InputBinding binding;

    [SerializeField]
    Text nameText;
    [SerializeField]
    Text actionText;

    private void OnValidate()
    {
        UpdateAction();
        UpdateUI();
    }

    private void Start()
    {
        UpdateAction();
    }

    private void UpdateAction()
    {
        if (actions == null)
        {
            if (InputController.inputs != null)
            {
                if (InputController.inputs.keybindings == null)
                    actions = new InputActions();
                else actions = InputController.inputs.keybindings;
            }
            else actions = new InputActions();
        }
        binding = actions.Actions.Get().bindings[Index];
    }

    public void UpdateUI()
    {
        nameText.text = reference.name;
        actionText.text = binding.ToDisplayString();
    }

}
