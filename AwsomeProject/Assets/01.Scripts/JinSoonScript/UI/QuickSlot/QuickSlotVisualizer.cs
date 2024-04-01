using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotVisualizer : MonoBehaviour
{
    private IngameQuickSlot[] slots = new IngameQuickSlot[5];

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            slots[i] = transform.GetChild(i).GetComponent<IngameQuickSlot>();
        }
    }

    public void SetQuickSlot()
    {
        for (int i = 0; i < 5; i++)
        {
            slots[i].DeleteItem();
            Item item = InventoryManager.Instance.PlayerInventory.quickSlot[i].assignedItem;
            if (item != null)
                slots[i].SetItem(item.itemSO, item.itemAmount);
        }
    }
}
