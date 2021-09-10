using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inv;
    public GameObject Slot;
    public int MaxInvSlots;

    private void Start()
    {
        inv = this;

        for (int i = 0; i < MaxInvSlots; i++)
        {
            GameObject temp = Instantiate(Slot, this.transform);
            temp.GetComponent<InventorySlot>().SlotID = i;

            if (i >= GameController.control.inventoryData.Count)
            {
                temp.GetComponent<InventorySlot>().invData = new InventoryData(i);
            }
            else temp.GetComponent<InventorySlot>().invData = GameController.control.inventoryData[i];
        }
    }

    public ItemData GetItem(int ItemID)
    {
        ItemData data = new ItemData(1);

        foreach (ItemData d in GameController.control.items)
        {
            if (d.ItemID == ItemID)
            {
                data = d;
                break;
            }
        }
        return data;
    }
}

[System.Serializable]
public enum ItemType
{
    consumable,
    equipable
}

[System.Serializable]
public struct ItemData 
{ 
    public int ItemID;
    public string ItemName;

    public ItemType type;

    public Sprite Icon;

    public ItemData(int itemID)
    {
        ItemID = -1;
        ItemName = "default";
        type = ItemType.consumable;
        Icon = null;
    }
}

[System.Serializable]
public struct InventoryData
{
    public int ItemID;
    public int Amount;

    public InventoryData (int i)
    {
        ItemID = -1;
        Amount = 0;
    }
}