using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PortionTableSlot : InventorySlot
{
    private PortionTable portionTable;
    [SerializeField] private int number = 0;

    protected override void Awake()
    {
        base.Awake();
        portionTable = GetComponentInParent<PortionTable>();
    }

    public override void InsertItem(Item item)
    {
        if (item.itemSO.itemType != ItemType.Ingredient) return;
        base.InsertItem(item);

        portionTable.AddItem(number, assignedItem.itemSO as IngredientItemSO);
    }

    public void RemoveItem()
    {
        //EffectEnum
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Item item = InventoryManager.Instance.curMovingItem;
        if (item != null && item.itemSO.itemType != ItemType.Ingredient) return;
        base.OnPointerClick(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Item item = InventoryManager.Instance.curMovingItem;
        if (item != null && item.itemSO.itemType != ItemType.Ingredient) return;
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Item item = InventoryManager.Instance.curMovingItem;
        if (item != null && item.itemSO.itemType != ItemType.Ingredient) return;
        base.OnPointerExit(eventData);
    }
}
