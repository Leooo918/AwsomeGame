using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class PortionCraftingIngredientSlot : InventorySlot
{
    public override void InsertItem(Item item)
    {
        Vector3 position = Vector3.zero;
        foreach (var r in parents)
        {
            if (r.GetComponent<Canvas>() != null || r.name == "InventoryBackground") break;
            position += r.localPosition;
        }

        if (item.itemAmount == 1)
        {
            item.GetComponent<RectTransform>().localPosition = position;
            item.Init(1, this);
            assignedItem = item;
        }
        else if (item.itemAmount > 1)
        {
            Item nItem = Instantiate(item.itemSO.prefab, item.transform.parent).GetComponent<Item>();
            nItem.GetComponent<RectTransform>().localPosition = position;
            nItem.Init(1, this);
            assignedItem = nItem;

            item.RemoveItem(1);
            item.ReturnToLastSlot();
        }


    }
}
