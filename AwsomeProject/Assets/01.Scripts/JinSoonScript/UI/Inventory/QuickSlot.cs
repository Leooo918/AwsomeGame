using UnityEngine.EventSystems;

public class QuickSlot : InventorySlot
{
    public override void InsertItem(Item item)
    {
        if (item.itemSO.itemType != ItemType.Portion) return;
        base.InsertItem(item);

        InventoryManager.Instance.QuickSlot.SetQuickSlot();
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
}
