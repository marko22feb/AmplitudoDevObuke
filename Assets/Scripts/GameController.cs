using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController control;

    public GameObject Player;
    public int CoinsCount;
    public int MaxBeltSlotItems;

    private Text coinsText;
    public Color newTextColor = Color.white;

    public List<ItemData> items;
    public List<InventoryData> inventoryData;
    public List<InventoryData> equipData;

    bool IsNewGame = false;
    string Username = "User";

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

        Player = GameObject.Find("Player");
        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();

        for (int i = 0; i < 5; i++)
        {
            equipData.Add(new InventoryData(-1, 0));
        }

        bool CanLoad;
        LoadGame(out CanLoad);

        UpdateCoinText();
        SceneManager.activeSceneChanged += OnNewLevel;
    }

    private void OnNewLevel(Scene c, Scene b)
    {
        if (control == this)
        {
            Player = GameObject.Find("Player");
            coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
            UpdateCoinText();
        }
    }

    public void OnCoinsPickUp(int amount)
    {
        CoinsCount = CoinsCount + amount;
        SaveGame();
        UpdateCoinText();
    }

    void UpdateCoinText() { 
        coinsText.text = "Coins : " + CoinsCount;
        coinsText.color = newTextColor;
    }

    void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + Username + ".123");
        SaveGame save = new SaveGame();

        save.CoinsAmount = CoinsCount;

        binaryFormatter.Serialize(file, save);
        file.Close();
    }

    void LoadGame(out bool Success)
    {
        if (File.Exists(Application.persistentDataPath  + Username + ".123"))
        {
            Success = true;

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Username + ".123", FileMode.Open);
            SaveGame save = (SaveGame)binaryFormatter.Deserialize(file);

            CoinsCount = save.CoinsAmount;

            file.Close();

        } 
        else Success = false;
    }
}

[Serializable]
public class SaveGame
{
    public int CoinsAmount;
    public int LastPlayedScene;
    public float PlayerHealth;
}

public enum InteractType { none, Door, Lever, Ladders };
public enum Direction { forward, backward, left, right };

