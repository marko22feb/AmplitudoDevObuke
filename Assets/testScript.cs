using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testScript : MonoBehaviour
{
    public Slider hpSlider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ColliderEnter");
    }

    public void OnSliderValueChanged()
    {
        Debug.Log(hpSlider.value);
    }
}
