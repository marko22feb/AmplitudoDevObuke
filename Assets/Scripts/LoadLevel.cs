using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : InteractableObject
{
    [SerializeField]
    string LevelName;
    public Scene SceneToLoad;

    public override void OnInteract()
    {
       if (DoOnce) return;
        SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
        base.OnInteract();
    }
}
