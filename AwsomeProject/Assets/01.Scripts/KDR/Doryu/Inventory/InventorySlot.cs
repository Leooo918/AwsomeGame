using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Doryu.Inventory
{
    [Serializable]
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
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
        public int maxMergeAmount => assignedItem.itemSO.maxMergeAmount;

        private GameObject _selectVolumObj;

        private void Awake()
        {
            _selectVolumObj = transform.Find("SelectedUI").gameObject;
        }

        public void SwapSlotData(InventorySlot slot)
        {
            Item temp = assignedItem;
            assignedItem = slot.assignedItem;
            slot.assignedItem = temp;
        }

        public void SetItem(Item item)
        {
            assignedItem = item;
        }

        public bool TryAddAmount(int amount = 1)
        {
            if (assignedItem.amount + amount > maxMergeAmount)
                return false;
            assignedItem.amount += amount;
            return true;
        }
        public bool TrySubAmount(int amount = 1)
        {
            if (assignedItem.amount - amount < maxMergeAmount)
                return false;
            assignedItem.amount += amount;
            return true;
        }
        public bool TrySetAmount(int amount = 0)
        {
            if (amount > maxMergeAmount || amount < 0)
                return false;
            assignedItem.amount = amount;
            return true;
        }
        public int AddAmount(int amount = 1)
        {
            int remain = 0;
            if (TryAddAmount(amount) == false)
            {
                remain = assignedItem.amount + amount - maxMergeAmount;
                TrySetAmount(maxMergeAmount);
            }

            return remain;
        }
        public int SubAmount(int amount = 1)
        {
            int over = 0;
            if (TrySubAmount(amount) == false)
            {
                over = amount - assignedItem.amount;
                TrySetAmount(0);
            }

            return over;
        }

        public void OnMouse(bool isOnMouse)
        {
            _selectVolumObj.SetActive(isOnMouse);
        }
        public void OnSelect(bool isSelected)
        {
            _selectVolumObj.transform.localScale = Vector3.one * (isSelected ? 1.05f : 1f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (InventoryManager.Instance.SelectedSlot == this) return;

            OnMouse(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (InventoryManager.Instance.SelectedSlot == this) return;

            OnMouse(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            InventoryManager.Instance.SelectedSlot = this;
        }
    }
}
