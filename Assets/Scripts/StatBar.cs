using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    bool RecentlyChange = false;
    float lerpElapsed = 0f;
    float lerpAlpha = 0f;
    float maxStatValue = 100f;
    float currentStatValue = 100f;

    [SerializeField]
    float lerpSpeed = 1f;

    [SerializeField]
    Color FrontBarColor;
    [SerializeField]
    Color PositiveColor;
    [SerializeField]
    Color NegativeColor;

    Image backBar;
    Image frontBar;
    Text StatValue;

    private void Awake()
    {
        backBar = transform.Find("BackBar").GetComponent<Image>();
        frontBar = transform.Find("FrontBar").GetComponent<Image>();
        StatValue = transform.Find("StatValue").GetComponent<Text>();

        frontBar.color = FrontBarColor;
        frontBar.fillAmount = currentStatValue / maxStatValue;
        backBar.color = NegativeColor;
        backBar.fillAmount = frontBar.fillAmount;
    }

    private void Update()
    {
        if (RecentlyChange)
        {
            float frontFill = frontBar.fillAmount;
            float backFill = backBar.fillAmount;

            float currentPercent = currentStatValue / maxStatValue;
            
            if (backFill > currentPercent)
            {
                backBar.color = NegativeColor;
                frontBar.fillAmount = currentPercent;

                lerpAlpha = lerpElapsed * lerpSpeed;
                lerpElapsed += Time.deltaTime;

                lerpAlpha = lerpAlpha * lerpAlpha;

                backBar.fillAmount = Mathf.Lerp(backFill, currentPercent, lerpAlpha);
            } 
            else
            {
                backBar.color = PositiveColor;

                backBar.fillAmount = currentPercent;

                lerpAlpha = lerpElapsed * lerpSpeed;
                lerpElapsed += Time.deltaTime;

                lerpAlpha = lerpAlpha * lerpAlpha;

                frontBar.fillAmount = Mathf.Lerp(frontFill, currentPercent, lerpAlpha);
            }
           
            if (lerpAlpha >= 1)
            {
                RecentlyChange = false;
            }
        }
    }


    public void UpdateBarVisuals(float currentValue, float maxValue)
    {
        RecentlyChange = true;
        maxStatValue = maxValue;
        currentStatValue = currentValue;
        StatValue.text = currentValue + " / " + maxValue;
        lerpElapsed = 0;
    }
}
