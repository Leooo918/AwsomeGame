using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotSlot : MonoBehaviour
{
    private InventorySlot _inventorySlot;

    public Item assignedItem => _inventorySlot.assignedItem;

    private void Awake()
    {
        _inventorySlot = GetComponent<InventorySlot>();
    }

    public void ReturnItem()
    {
        if (_inventorySlot.assignedItem == null) return;

        InventoryManager.Instance.TryAddItem(assignedItem);
        _inventorySlot.SetItem(null);
    }
    public void ClearItem()
    {
        if (assignedItem != null)
        {
            Destroy(assignedItem.gameObject);
            _inventorySlot.SetItem(null);
        }
    }
}
