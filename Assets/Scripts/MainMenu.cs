using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ContinueButton;
    private Canvas SettingsCanvas;
    private Canvas MainMenuCanvas;

    public void Start()
    {
        if (!File.Exists(Application.persistentDataPath + GameController.control.Username + ".123"))
        {
            ContinueButton.SetActive(false);
        }
        SettingsCanvas = GameObject.Find("SettingsCanvas").GetComponent<Canvas>();
       MainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<Canvas>();
    }

    public void OnMainMenuButtonClick(int type)
    {
        switch (type)
        {
            case 0:
                GameController.control.IsMainMenu = false;
                GameController.control.IsNewGame = false;
                SceneManager.LoadScene(GameController.control.GetLastPlayedScene());
                break;
            case 1:
                GameController.control.IsMainMenu = false;
                GameController.control.IsNewGame = true;
                SceneManager.LoadScene(0);
                break;
            case 2:
                break;
            case 3:
                MainMenuCanvas.enabled = false;
                SettingsCanvas.enabled = true;
                break;
            case 4:
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
