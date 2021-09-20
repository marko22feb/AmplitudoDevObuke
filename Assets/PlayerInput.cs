using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InteractableObject objectToInteractWith;
    public GameObject InteractCanvasPrefab;
    private GameObject spawnedCanvas;
    private Animator InventoryAC;
    public Animator WeaponSlotAC;
    private bool InventoryAnimation = false;
    private StatComponent statComp;

    public void ActivateInteract(InteractableObject ob)
    {
        if (objectToInteractWith == ob) return;
        objectToInteractWith = ob;
        if (ob != null)
            spawnedCanvas = Instantiate(InteractCanvasPrefab, objectToInteractWith.transform);
        else Destroy(spawnedCanvas);
    }

    private void Awake()
    {
        InventoryAC = GameObject.Find("InventoryPanel").GetComponent<Animator>();
        statComp = GetComponent<StatComponent>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (objectToInteractWith != null)
            {
                objectToInteractWith.OnInteract();
            }
        }

        if (Input.GetButtonDown("Inventory"))
        {
            if (!InventoryAnimation)
            {
                InventoryAC.SetTrigger("ClickedInventory");
                InventoryAnimation = true;
            }
        }

        if (Input.GetButtonDown("Consumable Left"))
        {
            UseConsumable(0);
        }

        if (Input.GetButtonDown("Consumable Right"))
        {
            UseConsumable(1);
        }

        if (Input.GetButtonDown("Melee"))
        {
            if (WeaponSlotAC.transform.childCount > 0)
            {
                WeaponSlotAC.SetTrigger("Attack");
            }
        }

        if (Input.GetButtonDown("Shoot"))
        {
            
        }
    }

    public void UseConsumable(int slot)
    {
        if (GameController.control.equipData[slot].Amount > 0)
        {
            ItemData itemData = Inventory.inv.GetItem(GameController.control.equipData[slot].ItemID);
            if (statComp.CurrentHealth == statComp.MaximumHealth) return;
            statComp.ModifyBy(itemData.consumeData.stat, itemData.consumeData.changeBy);
            InventoryData slotData = GameController.control.equipData[slot];
            slotData.Amount -= 1;

            if (slotData.Amount < 1) slotData = new InventoryData(-1);
            GameController.control.equipData[slot] = slotData;
            Inventory.inv.RefreshEquipableSlots();
        }
    }

    public void CanReEnterInventory ()
    {
        InventoryAnimation = false;
    }
}
