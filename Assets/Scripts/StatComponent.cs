using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatComponent : MonoBehaviour
{
    public float DamageAmount = 50;

    public StatData HealthStat;
    public StatData StaminaStat;
    public StatData ManaStat;

    [SerializeField]
    bool isPlayer;
    public string EnemyName = "defaultName";
    public bool isBoss = false;
    public bool isDead = false;

    AIBehavior behavior;
    Animator AC;

    private void Awake()
    {
        AC = GetComponent<Animator>();

        if (isPlayer)
        {
            HealthStat.SetStatBar(GameObject.Find("PlayerHealthBar").GetComponent<StatBar>());
            StaminaStat.SetStatBar(GameObject.Find("PlayerStaminaBar").GetComponent<StatBar>());
            ManaStat.SetStatBar(GameObject.Find("PlayerManaBar").GetComponent<StatBar>());
        } else
        {
            behavior = GetComponent<AIBehavior>();

            if (isBoss)
            {
                HealthStat.SetStatBar(GameObject.Find("BossStatBar").GetComponent<StatBar>());
            }
            else
            {
                HealthStat.SetStatBar(transform.parent.Find("EnemyCanvas").Find("HealthStatBar").GetComponent<StatBar>());
            }
        }
    }
    private void Start()
    {
        HealthStat.SetMaxStat();
        StaminaStat.SetMaxStat();
        ManaStat.SetMaxStat();

        UpdateStats(Stats.health);
        UpdateStats(Stats.mana);
        UpdateStats(Stats.stamina);
    }
    
    public void UpdateStats(Stats stat)
    {
        switch (stat)
        {
            case Stats.health:
                if (HealthStat.statBar != null)
                    HealthStat.statBar.UpdateBarVisuals(HealthStat.currentValue, HealthStat.maximumValue);
                else Debug.Log("Health Bar Missing : " + gameObject.name);
                break;
            case Stats.stamina:
                if (StaminaStat.statBar != null)
                StaminaStat.statBar.UpdateBarVisuals(StaminaStat.currentValue, StaminaStat.maximumValue);
                break;
            case Stats.mana:
                if (ManaStat.statBar != null)
                    ManaStat.statBar.UpdateBarVisuals(ManaStat.currentValue, ManaStat.maximumValue);
                break;
            default:
                break;
        }
    }

    public float GetStatValue(Stats stat)
    {
        float value = 0f;
        switch (stat)
        {
            case Stats.health:
                value = HealthStat.currentValue;
                break;
            case Stats.stamina:
                value = StaminaStat.currentValue;
                break;
            case Stats.mana:
                value = ManaStat.currentValue;
                break;
            default:
                break;
        }
        return value;
    }

    public void ModifyBy(Stats stat, float amount)
    {
        StatData statToEdit = new StatData();
        switch (stat)
        {
            case Stats.health:
                statToEdit = HealthStat;
                break;
            case Stats.stamina:
                 statToEdit = StaminaStat;
                break;
            case Stats.mana:
                 statToEdit = ManaStat;
                break;
            default:
                break;
        }
        
        statToEdit.currentValue = statToEdit.currentValue + amount;
        statToEdit.currentValue = Mathf.Clamp(statToEdit.currentValue, statToEdit.minimumValue, statToEdit.maximumValue);

        if (!isPlayer)
        {
            switch (stat)
            {
                case Stats.health:
                    behavior.OnHealthTrigger(statToEdit.currentValue / statToEdit.maximumValue);
                    break;
                case Stats.stamina:
                    behavior.OnStaminaTrigger(1f);
                    break;
                case Stats.mana:
                    behavior.OnManaTrigger(1f);
                    break;
                default:
                    break;
            }
        }

        switch (stat)
        {
            case Stats.health:
                HealthStat = statToEdit;
                if (HealthStat.currentValue <= 0) OnDeath();
                break;
            case Stats.stamina:
                StaminaStat = statToEdit;
                break;
            case Stats.mana:
                ManaStat = statToEdit;
                break;
            default:
                break;
        }

        UpdateStats(stat);
    }

    public void OnDeath()
    {
        if (isPlayer)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        else
        {
            if (isBoss) 
            {
                HealthStat.statBar.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
            } else
            {

            }
            transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        AC.SetBool("IsDead", true);
        isDead = true;
    }

    public void DestroySelf()
    {
        if (isPlayer)
        {
            if (GameController.control.LivesCount <= 0)
            {
                GameObject.Find("Canvas_MainHUD").GetComponent<Canvas>().enabled = false;
                GameObject.Find("DeathCanvas").GetComponent<Canvas>().enabled = true;
                GameController.control.IsInputEnabled = false;
                GameController.control.GameOver = true;
                Destroy(gameObject);
            }
            else
            {
                GameController.control.GameOver = false;
                GameController.control.ReduceLife();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else Destroy(transform.parent.gameObject);
    }
}

[System.Serializable]
public struct StatData
{
    public float minimumValue;
    public float maximumValue;
    public float currentValue;

    public StatBar statBar;

    public void SetMaxStat()
    {
        currentValue = maximumValue;
    }

    public void ModifyValue(float by)
    {
        currentValue = currentValue + by;
    }

    public void SetStatBar(StatBar bar)
    {
        statBar = bar;
    }
}
