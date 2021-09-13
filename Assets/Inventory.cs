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
                GameController.control.inventoryData.Add(new InventoryData(-1));
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

    public void AddItem(InventoryData data)
    {
        int slotID = -1;

        for (int i = 0; i < GameController.control.inventoryData.Count; i++)
        {
            if (GameController.control.inventoryData[i].ItemID == data.ItemID)
            {
                ItemData itemToCheck = GetItem(GameController.control.inventoryData[i].ItemID);
                int FreeSlots = itemToCheck.MaxStack - GameController.control.inventoryData[i].Amount;
                if (FreeSlots > 0) slotID = i;
                break;
            }
        }

        if (slotID != -1)
        {
            ItemData itemToCheck = GetItem(GameController.control.inventoryData[slotID].ItemID);
            int FreeSlots = itemToCheck.MaxStack - GameController.control.inventoryData[slotID].Amount;

            if (data.Amount > FreeSlots)
            {
                InventoryData newInvData = data;
                newInvData.Amount = newInvData.Amount - FreeSlots;

                InventoryData editInvData = GameController.control.inventoryData[slotID];
                editInvData.Amount = itemToCheck.MaxStack;

                GameController.control.inventoryData[slotID] = editInvData;
                AddItem(newInvData);
            }
            else
            {
                InventoryData editInvData = GameController.control.inventoryData[slotID];
                editInvData.Amount = GameController.control.inventoryData[slotID].Amount + data.Amount;
                GameController.control.inventoryData[slotID] = editInvData;
            }
        }
        else
        {
            for (int i = 0; i < GameController.control.inventoryData.Count; i++)
            {
                if (GameController.control.inventoryData[i].ItemID == -1)
                {
                    slotID = i;
                    break;
                }
            }

            if (slotID != -1)
            {
                ItemData itemToCheck = GetItem(GameController.control.inventoryData[slotID].ItemID);
                if (data.Amount > itemToCheck.MaxStack)
                {
                    GameController.control.inventoryData[slotID] = data;
                } else
                {
                    ItemData itemToAdd = GetItem(data.ItemID);
                    InventoryData dataToAdd = data;

                    dataToAdd.Amount = itemToAdd.MaxStack;

                    GameController.control.inventoryData[slotID] = dataToAdd;

                    InventoryData newInvData = data;
                    newInvData.Amount = newInvData.Amount - itemToAdd.MaxStack;

                    AddItem(newInvData);
                }

            }
            else
            {
                Debug.Log("Inventory Full");
            }
        }

        int childs = GameObject.Find("InventoryItems").transform.GetChildCount();
        for (int i = 0; i < childs; i++)
        {
            GameObject.Find("InventoryItems").transform.GetChild(i).GetComponent<InventorySlot>().invData = GameController.control.inventoryData[i];
            GameObject.Find("InventoryItems").transform.GetChild(i).GetComponent<InventorySlot>().UpdateVisuals();
        } 
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
    public int MaxStack;
    public Sprite Icon;

    public ItemData(int itemID)
    {
        ItemID = -1;
        ItemName = "default";
        type = ItemType.consumable;
        MaxStack = 1;
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