//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class Slot : MonoBehaviour
//{
//    private Item _assignedItem;
//    public Item assignedItem
//    {
//        get => _assignedItem;
//        private set
//        {
//            _assignedItem = value;
//            _assignedItem?.SetSlot(transform);
//        }
//    }

//    public int assignedItemAmount
//    {
//        get => _assignedItem.amount;
//        private set
//        {
//            _assignedItem.amount = value;
//            if (_assignedItem.amount == 0)
//            {
//                Destroy(_assignedItem.gameObject);
//                _assignedItem = null;
//                Save();
//            }
//        }
//    }


//    public int maxMergeAmount => assignedItem.itemSO.maxMergeAmount;
//    public bool isSelected;

//    private GameObject _selectVolumObj;

//    private void Update()
//    {
//        if (_assignedItem != null && _assignedItem.amount == 0)
//        {
//            Destroy(_assignedItem.gameObject);
//            _assignedItem = null;
//            Save();
//        }
//    }

//    public void Init(Inventory inventory)
//    {
//        _selectVolumObj = transform.Find("SelectedUI").gameObject;
//        this.inventory = inventory;
//    }

//    public bool TrySwapSlotData(InventorySlot slot)
//    {
//        if ((slot.assignedItem != null && inventory.CanEnterInven(slot.assignedItem.itemSO) == false) ||
//            slot.inventory.CanEnterInven(assignedItem.itemSO) == false)
//            return false;
//        Item temp = assignedItem;
//        assignedItem = slot.assignedItem;
//        slot.assignedItem = temp;
//        slot.Save();
//        Save();
//        return true;
//    }

//    public void SetItem(Item item)
//    {
//        assignedItem = item;
//    }

//    public bool TryAddAmount(int amount = 1)
//    {
//        if (assignedItemAmount + amount > maxMergeAmount)
//            return false;
//        assignedItemAmount += amount;
//        return true;
//    }
//    public bool TrySubAmount(int amount = 1)
//    {
//        if (assignedItemAmount - amount < 0)
//            return false;
//        assignedItemAmount -= amount;
//        Save();
//        return true;
//    }
//    public bool TrySetAmount(int amount = 0)
//    {
//        if (amount > maxMergeAmount || amount < 0)
//            return false;
//        assignedItemAmount = amount;
//        return true;
//    }
//    public int AddAmount(int amount = 1)
//    {
//        int remain = 0;
//        if (TryAddAmount(amount) == false)
//        {
//            remain = assignedItemAmount + amount - maxMergeAmount;
//            TrySetAmount(maxMergeAmount);
//        }

//        return remain;
//    }
//    public int SubAmount(int amount = 1)
//    {
//        int over = 0;
//        if (TrySubAmount(amount) == false)
//        {
//            over = amount - assignedItemAmount;
//            TrySetAmount(0);
//            Save();
//        }

//        return over;
//    }

//    public void OnMouse(bool isOnMouse)
//    {
//        if (_selectVolumObj == null) return;

//        _selectVolumObj.SetActive(isOnMouse);
//    }
//    public void OnSelect(bool isSelected)
//    {
//        if (_selectVolumObj == null) return;

//        _selectVolumObj.transform.localScale = Vector3.one * (isSelected ? 1.05f : 1f);
//    }

//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        InventoryManager.Instance.stayMouseSlot = this;

//        if (isSelected) return;
//        OnMouse(true);
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        InventoryManager.Instance.stayMouseSlot = null;

//        if (isSelected) return;
//        OnMouse(false);
//    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        inventory.SetSelected(this);
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        if (assignedItem != null)
//            assignedItem.transform.SetParent(inventory.itemStorage);

//        InventoryManager.Instance.dragItemSlot = this;
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        if (assignedItem != null)
//            assignedItem.transform.SetParent(transform);

//        InventorySlot slot = InventoryManager.Instance.stayMouseSlot;
//        if (slot == null || assignedItem == null)
//            assignedItem?.SetSlot();
//        else
//        {
//            if (TrySwapSlotData(slot))
//            {
//                inventory.SetSelected(null);
//                slot.inventory.SetSelected(slot);
//            }
//            else
//            {
//                assignedItem.SetSlot();
//            }
//        }
//        InventoryManager.Instance.dragItemSlot = null;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {

//    }
//}
