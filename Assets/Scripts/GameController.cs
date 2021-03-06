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
    public int LivesCount;
    public int LastCheckpoint = -1;
    public int MaxBeltSlotItems;

    public List<AudioClip> backgroundmusic = new List<AudioClip>();
    private AudioSource audioSource;
    private GameObject console;
    public GameObject consoleText;

    public float OptimizationDistance = 25f;

    private Text coinsText;
    public Color newTextColor = Color.white;

    public List<ItemData> items;
    public List<InventoryData> inventoryData;
    public List<InventoryData> equipData;
    public List<int> BossesDead;

    public bool IsNewGame = false;
    public bool IsMainMenu = false;
    [HideInInspector]
    public bool GameOver = false;
    public string Username = "User";
    [HideInInspector]
    public bool IsInputEnabled = true;

    [SerializeField]
    private GameObject settingsPrefab;
    [HideInInspector]
    public GameObject settingsCanvas;

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

        for (int i = 0; i < 5; i++)
        {
            equipData.Add(new InventoryData(-1, 0));
        }

        SceneManager.activeSceneChanged += OnNewLevel;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();
        StartCoroutine(delayedAd());

        for (int i = 0; i < 30; i++)
        {
            Console("" + i);
        }
    }

    public void Console(string Debug)
    {
        console = GameObject.Find("Console_Canvas");
        if (console == null)
        {
            console = Instantiate(console);
        }

        GameObject consolePanel = console.transform.Find("Panel").transform.Find("Panel").gameObject;

        GameObject temp = Instantiate(consoleText, consolePanel.transform);
        temp.GetComponent<Text>().text = System.DateTime.Now + " : " + Debug;
    }

    private IEnumerator delayedAd()
    {
        yield return new WaitForSeconds(1);
        GetComponent<AdController>().PlayRewardedAd(OnRewardAdComplete);
    }

    private IEnumerator PlayNextBackgroundMusic(float Duration)
    {
        yield return new WaitForSeconds(Duration);
        PlayBackgroundMusic();
    }

    void PlayBackgroundMusic()
    {
        int random = UnityEngine.Random.Range(0, backgroundmusic.Count);
        audioSource.clip = backgroundmusic[random];
        audioSource.Play();
        float lenght = backgroundmusic[random].length;
        StartCoroutine(PlayNextBackgroundMusic(lenght));
    }

    private void OnNewLevel(Scene c, Scene b)
    {
        if (!IsMainMenu)
        {
            if (control == this)
            {
                settingsCanvas = GameObject.Find("SettingsCanvas");
                if (settingsCanvas == null) settingsCanvas = Instantiate(settingsPrefab);

                bool CanLoad = false;

                if (!IsNewGame)
                {
                    LoadGame(out CanLoad);
                }

                Player = GameObject.Find("Player");

                if (CanLoad)
                {
                    Checkpoint[] checkpoints = GameObject.FindObjectsOfType<Checkpoint>();

                    foreach (var item in checkpoints)
                    {
                        Debug.Log("Checkpoints Check");
                        if (item.CheckpointID == LastCheckpoint)
                        {
                            Player.transform.position = item.transform.position;
                           
                            break;
                        }
                    }
                }
                
                coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
                UpdateCoinText();
            }
        }
    }

    public static float MapClampRanged(float value, float inMin, float inMax, float outMin, float outMax)
    {
        float percent = Mathf.InverseLerp(inMin, inMax, value);
        float outValue = Mathf.Lerp(outMin, outMax, percent);
        return outValue;
    }

    public void OnCoinsPickUp(int amount)
    {
        CoinsCount = CoinsCount + amount;
        if (CoinsCount >= 100)
        {
            CoinsCount -= 100;
            LivesCount += 1;
            SaveLives();
        }

        UpdateCoinText();
    }

    public void OnRewardAdComplete()
    {
        OnCoinsPickUp(40);
    }    

    public void ReduceLife()
    {
        if (LivesCount > 0)
        {
            LivesCount -= 1;
            SaveLives();
        }
    }

    void UpdateCoinText() { 
        coinsText.text = "Coins : " + CoinsCount;
        coinsText.color = newTextColor;
    }

    public void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        SaveGame save = new SaveGame();

        if (File.Exists(Application.persistentDataPath + Username + ".123"))
        {
            FileStream Loadedfile = File.Open(Application.persistentDataPath + Username + ".123", FileMode.Open);
            save = (SaveGame)binaryFormatter.Deserialize(Loadedfile);
            Loadedfile.Close();
        }

        FileStream file = File.Create(Application.persistentDataPath + Username + ".123");

        save.CoinsAmount = CoinsCount;
        save.checkpointIndex = LastCheckpoint;
        save.BossesDead = BossesDead;
        save.LastPlayedScene = SceneManager.GetActiveScene().buildIndex;

        binaryFormatter.Serialize(file, save);
        file.Close();
    }

    public void SaveLives()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        SaveGame save = new SaveGame();

        if (File.Exists(Application.persistentDataPath + Username + ".123")) { 
            FileStream Loadedfile = File.Open(Application.persistentDataPath + Username + ".123", FileMode.Open);
            save = (SaveGame)binaryFormatter.Deserialize(Loadedfile);
            Loadedfile.Close();
          }

        FileStream file = File.Create(Application.persistentDataPath + Username + ".123");

        save.LivesAmount = LivesCount;

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
            LivesCount = save.LivesAmount;
            LastCheckpoint = save.checkpointIndex;
            BossesDead = save.BossesDead;

            file.Close();

        } 
        else Success = false;
    }

    public int GetLastPlayedScene()
    {
        int scene = 0;

        if (File.Exists(Application.persistentDataPath + Username + ".123"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Username + ".123", FileMode.Open);
            SaveGame save = (SaveGame)binaryFormatter.Deserialize(file);

            scene = save.LastPlayedScene;

            file.Close();

        }

        return scene;
    }

    public void SaveItemsData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "items.123");
        SaveItems save = new SaveItems();

        save.itemdata = items;

        binaryFormatter.Serialize(file, save);
        file.Close();
    }
}

[Serializable]
public class SaveGame
{
    public int CoinsAmount;
    public int LivesAmount;
    public int LastPlayedScene;
    public int checkpointIndex;
    public List<int> BossesDead;
}

[Serializable]
public class SaveItems
{
    public List<ItemData> itemdata;

    public SaveItems()
    {

    }
}

public enum Stats { health, stamina, mana};
public enum InteractType { none, Door, Lever, Ladders, Dialogue };
public enum Direction { forward, backward, left, right };
public enum EnemyType { ground, jumping, flying };
public enum PatrolType { looping, backAndForth, single };
[Serializable]
public enum LockedDoor { enemyDead, hasItem, needCoins, custom };
public enum MovementType { patrol, freestyle };

public enum MovementMode { ground, ladder, falling, swimming}
[Serializable]
public enum Abilities { bullet, multibullet, bulletRain, bulletRainWave, bulletRainWaveRandomized, NULL};

[Serializable]
public enum MainMenuButtonType { Continue, newgame, multiplayer, settings, exit };
[Serializable]
public enum BehaviorType { idle, attackMelee, attackRanged, ability};
[Serializable]
public enum NPCState { regular, attacking, stunned, scriptedAbility};
[Serializable]
public enum ArithmeticOperation { lessThen, lessOrEqual, equal, greaterThen, greaterOrEqual};
[Serializable]
public enum AIActionToTake { usePotion, useAbility, changeBehavior}

[Serializable]
public struct OnStatTrigger
{
    public ArithmeticOperation operation;
    public float Percent;
    public AIActionToTake action;

    public Abilities abilityToUse;
    public BehaviorType behaviorToChangeTo;
}

[Serializable]
public struct OnSightChange
{
    public List<string> Tags;
    public BehaviorType ChangeBehaviorTo;
}

[Serializable]
public struct BehaviorConfig
{
    public OnSightChange OnSight;
    public OnSightChange OnLoseSight;

    public List<OnStatTrigger> healthTriggers;
    public List<OnStatTrigger> manaTriggers;
    public List<OnStatTrigger> staminaTriggers;

    public float FollowDistance;
}
