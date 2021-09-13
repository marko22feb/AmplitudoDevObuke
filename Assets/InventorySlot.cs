using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int SlotID;

    public ItemData itemData;
    public InventoryData invData;

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

        InventoryData draggedData = draggedSlot.invData;
        InventoryData currentData = invData;
        int DraggedSlotID = draggedSlot.SlotID;

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
            } else
            {
                int AmountToAdd = itemData.MaxStack - invData.Amount;
                draggedSlot.invData.Amount = draggedData.Amount - AmountToAdd;
                invData.Amount = invData.Amount + AmountToAdd;
            }
            
        }


        GameController.control.inventoryData[DraggedSlotID] = draggedSlot.invData;
        GameController.control.inventoryData[SlotID] = invData;

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
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
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
}
