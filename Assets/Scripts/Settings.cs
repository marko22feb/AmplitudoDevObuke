using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private Canvas SettingsCanvas;
    private Canvas MainMenuCanvas;

    private void Start()
    {
        SettingsCanvas = GetComponent<Canvas>();
        if (GameController.control.IsMainMenu)
        MainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<Canvas>();
    }

    public void OnMainMenuButtonClick(int type)
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                if (GameController.control.IsMainMenu) {
                    SettingsCanvas.enabled = false;
                    MainMenuCanvas.enabled = true;
                } else 
                {
                    Time.timeScale = 1f;
                    GameController.control.IsInputEnabled = true;
                    SettingsCanvas.enabled = false;
                };
                break;
            default:
                break;
        }
    }
}
