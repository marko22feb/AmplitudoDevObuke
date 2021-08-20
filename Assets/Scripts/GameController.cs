using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int CoinsCount;
    private Text coinsText;
    public Color newTextColor = Color.white;

    public void Awake()
    {
        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
    }

    public void OnCoinsPickUp(int amount)
    {
        CoinsCount = CoinsCount + amount;
        coinsText.text = "Coins : " + CoinsCount;
        coinsText.color = newTextColor;
    }
}
