using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatComponent : MonoBehaviour
{
    [SerializeField]
    float MinimumHealth = 0;
    public float MaximumHealth = 100;
    public float CurrentHealth = 75;
    public float DamageAmount = 50;

    Slider healthSlider;
    Text healthText;
    [SerializeField]
    bool isPlayer;

    private void Awake()
    {
        if (isPlayer)
        {
            healthSlider = GameObject.Find("PlayerHealthSlider").GetComponent<Slider>();
        } else
        {
            CurrentHealth = MaximumHealth;
            healthSlider = transform.parent.Find("EnemyCanvas").transform.Find("HealthSlider").GetComponent<Slider>();
        }

        healthText = healthSlider.transform.Find("HealthText").GetComponent<Text>();
        healthSlider.minValue = MinimumHealth;
        healthSlider.maxValue = MaximumHealth;
        UpdateSlider();
    }
    
    private void UpdateSlider()
    {
        healthSlider.value = CurrentHealth;
        healthText.text = CurrentHealth + " / " + MaximumHealth;
    }

    public void ModifyBy(Stats stat, float amount)
    {
        CurrentHealth = CurrentHealth + amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinimumHealth, MaximumHealth);
        UpdateSlider();
    }
}
