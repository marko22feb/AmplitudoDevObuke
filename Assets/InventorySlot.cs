using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int SlotID;

    public ItemData itemData;
    public InventoryData invData;

    Image Icon;
    Text Amount;

    private void Awake()
    {
        Icon = this.transform.Find("Icon").GetComponent<Image>();
        Amount = this.transform.Find("Text").GetComponent<Text>();
    }

    private void Start()
    {
        if (invData.ItemID != -1)
        {
            Icon.color = Color.white;
            Icon.sprite = itemData.Icon;
            Amount.text = "x" + invData.Amount;
        }
        else
        {
            Color transparentColor = new Color(0, 0, 0, 0);
            Icon.color = transparentColor;
            Amount.text = null;
        }
    }
}
