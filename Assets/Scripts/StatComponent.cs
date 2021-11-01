using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatComponent : MonoBehaviour
{
    public float DamageAmount = 50;

    public StatData HealthStat;
    public StatData StaminaStat;
    public StatData ManaStat;

    [SerializeField]
    bool isPlayer;

    AIBehavior behavior;

    private void Awake()
    {
        if (isPlayer)
        {
            HealthStat.SetStatBar(GameObject.Find("PlayerHealthBar").GetComponent<StatBar>());
            StaminaStat.SetStatBar(GameObject.Find("PlayerStaminaBar").GetComponent<StatBar>());
            ManaStat.SetStatBar(GameObject.Find("PlayerManaBar").GetComponent<StatBar>());
        } else
        {
            behavior = GetComponent<AIBehavior>();
            // CurrentHealth = MaximumHealth;
            //  healthBar = transform.parent.Find("EnemyCanvas").transform.Find("HealthSlider").GetComponent<Slider>();
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
                HealthStat.statBar.UpdateBarVisuals(HealthStat.currentValue, HealthStat.maximumValue);
                break;
            case Stats.stamina:
                StaminaStat.statBar.UpdateBarVisuals(StaminaStat.currentValue, StaminaStat.maximumValue);
                break;
            case Stats.mana:
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
