using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class InputController : MonoBehaviour
{
    public static InputController inputs;
    public InputActions keybindings;
    private string pathToCheck;

    private void Awake()
    {
        if (inputs == null) inputs = this;

        keybindings = new InputActions();
        keybindings.Actions.Enable();
        keybindings.Actions.Debug.started += OnDebug;
    }

    private void Start()
    {
        LoadKeybinds();
    }

    public void Rebind(string actionName, int bindingIndex, Text keyText, KeybindUI ui)
    {
        InputAction actionToRebind = keybindings.asset.FindAction(actionName);

        actionToRebind.Disable();
        InputActionRebindingExtensions.RebindingOperation rebind = actionToRebind.PerformInteractiveRebinding();

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();

            DoCheck(bindingIndex, pathToCheck, actionToRebind);
            ui.UpdateUI();

            SaveKeybinds();
            operation.Dispose();
        }
        );

        if (keybindings.Actions.Get().bindings[bindingIndex].overridePath != null)
        {
            pathToCheck = keybindings.Actions.Get().bindings[bindingIndex].overridePath;
        }
        else
        {
            pathToCheck = keybindings.Actions.Get().bindings[bindingIndex].path;
        }

        keyText.text = "Press Any Key";
        rebind.Start();
    }

    private void DoCheck(int Index, string oldPath, InputAction rebindedAction)
    {
        int exists = IsAlreadyBinded(Index, keybindings.Actions.Get().bindings[Index].overridePath);
        if (exists != -1)
        {
            Debug.Log("Already exists, multiple is " + keybindings.Actions.Get().bindings[exists].action.ToString());

            rebindedAction.ApplyBindingOverride(oldPath);
        }
    }

    public int IsAlreadyBinded(int Index, string pathToCheck)
    {
        if (pathToCheck == null) return -1;

        for (int i = 0; i < keybindings.Actions.Get().bindings.Count; i++)
        {
            if (i != Index)
            {
                string newPathToCheck;
                if (keybindings.Actions.Get().bindings[i].overridePath == null) newPathToCheck = keybindings.Actions.Get().bindings[i].path;
                else newPathToCheck = keybindings.Actions.Get().bindings[i].overridePath;

                if (pathToCheck == newPathToCheck)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private void OnDebug(InputAction.CallbackContext obj)
    {
        Debug.Log("Pressed Keybind");
    }

    public void SaveKeybinds()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "Keybinds.123");
        KeybindSave save = new KeybindSave();

        List<string> temp = new List<string>();

        for (int i = 0; i < keybindings.Actions.Get().bindings.Count; i++)
        {
            string PathToAdd;

            if (keybindings.Actions.Get().bindings[i].overridePath == null) PathToAdd = keybindings.Actions.Get().bindings[i].path;
            else PathToAdd = keybindings.Actions.Get().bindings[i].overridePath;
            temp.Add(PathToAdd);
        }

        save.savedPaths = temp;

        binaryFormatter.Serialize(file, save);
        file.Close();
    }

    private void LoadKeybinds()
    {
       
        if (File.Exists(Application.persistentDataPath + "Keybinds.123"))
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "Keybinds.123", FileMode.Open);
            KeybindSave save = (KeybindSave)binaryFormatter.Deserialize(file);

            for (int i = 0; i < keybindings.Actions.Get().bindings.Count; i++)
            {
                InputAction actionToRebind = keybindings.asset.FindAction(keybindings.Actions.Get().bindings[i].action.ToString());
                actionToRebind.ApplyBindingOverride(save.savedPaths[i]);
            }
            
            file.Close();
        }
    }
}

[System.Serializable]
public class KeybindSave
{
    public List<string> savedPaths;
}
