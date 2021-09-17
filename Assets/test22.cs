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
        yield return new WaitForSeconds(1);
        InventoryData item = new InventoryData(0);
        item.ItemID = 1;
        item.Amount = 10;

        Inventory.inv.AddItem(item);
    }
}
