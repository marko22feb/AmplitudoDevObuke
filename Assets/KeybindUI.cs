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
    [Range (0, 20)]
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
        StartCoroutine(delayedCheck());
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
        if (InputController.inputs != null)
        {
            if (InputController.inputs.keybindings == null)
                actions = new InputActions();
            else actions = InputController.inputs.keybindings;
        }

        binding = actions.Actions.Get().bindings[Index];
        nameText.text = binding.action.ToString();
        actionText.text = binding.ToDisplayString();
    }

    public void OnClickReset()
    {
        int bindToCheck = InputController.inputs.IsAlreadyBinded(Index, InputController.inputs.keybindings.Actions.Get().bindings[Index].path);
        if (bindToCheck == -1)
        {
            InputAction actionToReset = InputController.inputs.keybindings.asset.FindAction(binding.action.ToString());
            actionToReset.ApplyBindingOverride(InputController.inputs.keybindings.Actions.Get().bindings[Index].path);
            InputController.inputs.SaveKeybinds();
            UpdateUI();
        }
        else
        {
            Debug.Log("Already exists, multiple is " + InputController.inputs.keybindings.Actions.Get().bindings[bindToCheck].action.ToString());
        }
    }

    public void OnClick()
    {
        InputController.inputs.Rebind(binding.action.ToString(), Index, actionText, this);
        Debug.Log("Text");
    }

    IEnumerator delayedCheck()
    {
        yield return new WaitForSeconds(1f);
        UpdateUI();
    }
}
