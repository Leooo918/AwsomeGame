using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotSlot : MonoBehaviour
{
    public InventorySlot inventorySlot;

    public Item assignedItem => inventorySlot.assignedItem;

    private void Awake()
    {
        inventorySlot = GetComponent<InventorySlot>();
    }

    public void ReturnItem()
    {
        if (inventorySlot.assignedItem == null) return;

        InventoryManager.Instance.TryAddItem(assignedItem);
        inventorySlot.SetItem(null);
    }
    public void ClearItem()
    {
        if (assignedItem != null)
        {
            Destroy(assignedItem.gameObject);
            inventorySlot.SetItem(null);
        }
    }
}
