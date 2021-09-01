using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController control;
    public int CoinsCount;
    private Text coinsText;
    public Color newTextColor = Color.white;

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (control == null) 
        { 
            control = this; 
        } else 
        { 
            Destroy(gameObject);
            return;
        }

        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
        UpdateCoinText();
        SceneManager.activeSceneChanged += OnNewLevel;
    }

    private void OnNewLevel(Scene c, Scene b)
    {
        if (control == this)
        {
            coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
            UpdateCoinText();
        }
    }

    public void OnCoinsPickUp(int amount)
    {
        CoinsCount = CoinsCount + amount;
        UpdateCoinText();
    }

    void UpdateCoinText() { 
        coinsText.text = "Coins : " + CoinsCount;
        coinsText.color = newTextColor;
    }
}
