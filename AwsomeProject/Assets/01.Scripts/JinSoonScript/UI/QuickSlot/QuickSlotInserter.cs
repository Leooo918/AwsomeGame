using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class QuickSlotInserter : InventorySlot
{
    private QuickSlotInserterSet inserterSet;
    private int slotIdx = 0;

    protected override void Awake()
    {
        base.Awake();
        inserterSet = transform.parent.GetComponent<QuickSlotInserterSet>();
    }

    public override void InsertItem(Item item)
    {
        if (item.itemSO.itemType != ItemType.Portion) return;
        base.InsertItem(item);

        QuickSlotManager.Instance.QuickSlotSetsParent.SetItem(item.itemSO, inserterSet.slotNum, slotIdx);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Item item = InventoryManager.Instance.curMovingItem;
        if (item != null && item.itemSO.itemType != ItemType.Portion) return;
        base.OnPointerClick(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Item item = InventoryManager.Instance.curMovingItem;
        if (item != null && item.itemSO.itemType != ItemType.Portion) return;
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Item item = InventoryManager.Instance.curMovingItem;
        if (item != null && item.itemSO.itemType != ItemType.Portion) return;
        base.OnPointerExit(eventData);
    }

    public void Init(int idx)
    {
        slotIdx = idx;
    }
}
