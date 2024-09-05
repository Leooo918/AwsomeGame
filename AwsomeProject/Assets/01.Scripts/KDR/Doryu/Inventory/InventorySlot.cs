using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Inventory _inventory;
    private Item _assignedItem;
    public Item assignedItem
    {
        get => _assignedItem;
        private set
        {
            _assignedItem = value;
            _assignedItem?.SetSlot(transform);
        }
    }
    public int assignedItemAmount
    {
        get => _assignedItem.amount;
        private set
        {
            _assignedItem.amount = value;
            _assignedItem.TextUpdate();
        }
    }
    public int maxMergeAmount => assignedItem.itemSO.maxMergeAmount;

    private GameObject _selectVolumObj;

    public void Init(Inventory inventory)
    {
        _selectVolumObj = transform.Find("SelectedUI").gameObject;
        _inventory = inventory;
    }

    public void Save()
    {
        _inventory.Save();
    }

    public void SwapSlotData(InventorySlot slot)
    {
        Item temp = assignedItem;
        assignedItem = slot.assignedItem;
        slot.assignedItem = temp;
        slot.Save();
        Save();
    }

    public void SetItem(Item item)
    {
        assignedItem = item;
    }

    public bool TryAddAmount(int amount = 1)
    {
        if (assignedItemAmount + amount > maxMergeAmount)
            return false;
        assignedItemAmount += amount;
        return true;
    }
    public bool TrySubAmount(int amount = 1)
    {
        if (assignedItemAmount - amount < maxMergeAmount)
            return false;
        assignedItemAmount += amount;
        return true;
    }
    public bool TrySetAmount(int amount = 0)
    {
        if (amount > maxMergeAmount || amount < 0)
            return false;
        assignedItemAmount = amount;
        return true;
    }
    public int AddAmount(int amount = 1)
    {
        int remain = 0;
        if (TryAddAmount(amount) == false)
        {
            remain = assignedItemAmount + amount - maxMergeAmount;
            TrySetAmount(maxMergeAmount);
        }

        return remain;
    }
    public int SubAmount(int amount = 1)
    {
        int over = 0;
        if (TrySubAmount(amount) == false)
        {
            over = amount - assignedItemAmount;
            TrySetAmount(0);
        }

        return over;
    }

    public void OnMouse(bool isOnMouse)
    {
        if (_selectVolumObj == null) return;

        _selectVolumObj.SetActive(isOnMouse);
    } 
    public void OnSelect(bool isSelected)
    {
        if (_selectVolumObj == null) return;

        _selectVolumObj.transform.localScale = Vector3.one * (isSelected ? 1.05f : 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InventoryManager.Instance.SelectedSlot == this) return;
        InventoryManager.Instance.stayMouseSlot = this;

        OnMouse(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventoryManager.Instance.SelectedSlot == this) return;
        InventoryManager.Instance.stayMouseSlot = null;

        OnMouse(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.SelectedSlot = this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (assignedItem != null)
            assignedItem.transform.SetParent(transform.parent.parent);

        InventoryManager.Instance.dragItemSlot = this;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (assignedItem != null)
            assignedItem.transform.SetParent(transform);


        if (InventoryManager.Instance.stayMouseSlot == null)
            assignedItem.SetSlot();
        else
            SwapSlotData(InventoryManager.Instance.stayMouseSlot);
        InventoryManager.Instance.dragItemSlot = null;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}

