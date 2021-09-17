using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Equip Data")]
    public bool Equip = false;
    public EquipType equipType = EquipType.consumable;
    public int EquipSlotID = -1;

    [HideInInspector]
    public int SlotID;
    [HideInInspector]
    public ItemData itemData;
    [HideInInspector]
    public InventoryData invData = new InventoryData(-1);

    Image Icon;
    Text Amount;

    private void Awake()
    {
        Icon = this.transform.Find("Icon").GetComponent<Image>();
        Amount = Icon.transform.Find("Text").GetComponent<Text>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (invData.ItemID == -1) return;
        Icon.transform.SetParent(this.transform.parent.parent);
        Icon.raycastTarget = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (invData.ItemID == -1) return;
        Icon.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();

        if (draggedSlot.invData.ItemID == -1) return;

        InventoryData draggedData = draggedSlot.invData;
        InventoryData currentData = invData;

        int DraggedSlotID = draggedSlot.Equip? draggedSlot.EquipSlotID : draggedSlot.SlotID;

        int maxBeltItems = GameController.control.MaxBeltSlotItems;

        if (draggedSlot.Equip)
        {
            if (Equip)
            {
                if (draggedData.ItemID != currentData.ItemID)
                {
                    invData = draggedData;
                    draggedSlot.invData = currentData;
                }
                else
                {
                    if (draggedData.Amount + currentData.Amount > maxBeltItems)
                    {
                        draggedData.Amount -= maxBeltItems - currentData.Amount;
                        currentData.Amount = maxBeltItems;
                    }
                    else
                    {
                        currentData.Amount += draggedData.Amount;
                        draggedData = new InventoryData(-1);
                    }

                    draggedSlot.invData = draggedData;
                    invData = currentData;
                }

                GameController.control.equipData[DraggedSlotID] = draggedSlot.invData;
                GameController.control.equipData[EquipSlotID] = invData;
            }
            else
            {
                if (draggedData.ItemID != currentData.ItemID)
                {
                    if (invData.Amount > maxBeltItems)
                    {
                        currentData.Amount -= maxBeltItems;
                        invData = currentData;

                        currentData.Amount = maxBeltItems;

                        StartCoroutine(AddItemDelayed(draggedData));
                        draggedSlot.invData = currentData;
                    }
                    else
                    {
                        invData = draggedData;
                        draggedSlot.invData = currentData;
                    }
                }

                else
                {
                    if (invData.Amount + draggedData.Amount <= itemData.MaxStack)
                    {
                        draggedSlot.invData = new InventoryData(-1);
                        invData.Amount = invData.Amount + draggedData.Amount;
                    }
                    else
                    {
                        int AmountToAdd = itemData.MaxStack - invData.Amount;
                        draggedSlot.invData.Amount = draggedData.Amount - AmountToAdd;
                        invData.Amount = invData.Amount + AmountToAdd;

                        Inventory.inv.AddItem(draggedSlot.invData);
                        draggedSlot.invData = new InventoryData(-1);
                    }
                }
               
                GameController.control.equipData[DraggedSlotID] = draggedSlot.invData;
                GameController.control.inventoryData[SlotID] = invData;
            }
        }
        else
        {
            if (Equip)
            {
                if (invData.ItemID != -1)
                {
                    if (draggedData.ItemID != invData.ItemID)
                    {
                        Inventory.inv.AddItem(invData);
                        invData = new InventoryData(-1, 0);
                    }
                }

                if (invData.ItemID == -1)
                {
                    if (draggedData.Amount > maxBeltItems)
                    {
                        invData = new InventoryData(draggedData.ItemID, maxBeltItems);
                        draggedData.Amount -= maxBeltItems;
                        draggedSlot.invData = draggedData;
                    }
                    else
                    {
                        invData = new InventoryData(draggedData.ItemID, draggedData.Amount);
                        draggedSlot.invData = new InventoryData(-1);
                    }
                }
                else
                {
                    int possibleAmountToAdd = maxBeltItems - invData.Amount;
                    if (draggedData.Amount > possibleAmountToAdd)
                    {
                        invData.Amount = maxBeltItems;
                        draggedData.Amount -= possibleAmountToAdd;
                        draggedSlot.invData = draggedData;
                    }
                    else
                    {
                        invData.Amount += draggedData.Amount;
                        draggedSlot.invData = new InventoryData(-1);
                    }
                }

                GameController.control.inventoryData[DraggedSlotID] = draggedSlot.invData;
                GameController.control.equipData[EquipSlotID] = invData;
            }
            else
            {
                if (draggedData.ItemID != currentData.ItemID)
                {
                    invData = draggedData;
                    draggedSlot.invData = currentData;
                }
                else
                {
                    if (invData.Amount + draggedData.Amount <= itemData.MaxStack)
                    {
                        draggedSlot.invData = new InventoryData(-1);
                        invData.Amount = invData.Amount + draggedData.Amount;
                    }
                    else
                    {
                        int AmountToAdd = itemData.MaxStack - invData.Amount;
                        draggedSlot.invData.Amount = draggedData.Amount - AmountToAdd;
                        invData.Amount = invData.Amount + AmountToAdd;
                    }

                }

                GameController.control.inventoryData[DraggedSlotID] = draggedSlot.invData;
                GameController.control.inventoryData[SlotID] = invData;
            }
        }

        draggedSlot.UpdateVisuals();
        UpdateVisuals();
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Icon.transform.SetParent(this.transform);
        Icon.transform.localPosition = new Vector3(0, 0, 0);
        Icon.raycastTarget = true;
    }

    private void Start()
    {
        if (Equip) {
        invData = GameController.control.equipData[EquipSlotID];
        }
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
       if (Inventory.inv == null)
        {
            StartCoroutine(RefreshVisuals());
            return;
        }

        itemData = Inventory.inv.GetItem(invData.ItemID);

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

    IEnumerator RefreshVisuals()
    {
        yield return new WaitForEndOfFrame();
        UpdateVisuals();
    }

    IEnumerator AddItemDelayed(InventoryData idata)
    {
        yield return new WaitForEndOfFrame();
        Inventory.inv.AddItem(idata);
    }
}
