using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test22 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(tempAdd());
    }

    IEnumerator tempAdd()
    {
        yield return new WaitForSeconds(3);
        InventoryData item = new InventoryData(0);
        item.ItemID = 1;
        item.Amount = 111;

        Inventory.inv.AddItem(item);
    }
}
