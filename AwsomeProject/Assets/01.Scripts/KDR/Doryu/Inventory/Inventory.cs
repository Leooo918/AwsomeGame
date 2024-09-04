using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Doryu.JBSave;
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;

namespace Doryu.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private InventorySaveData _inventoryData = new InventorySaveData();
        private InventorySlot[,] slots = new InventorySlot[0, 0];

        [SerializeField] private Vector2Int _inventorySize;
        [SerializeField] private Transform _slotParent;

        [ContextMenu("ClearSaveData")]
        public void ClearSaveData()
        {
            slots = new InventorySlot[_inventorySize.x, _inventorySize.y];
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    slots[x, y] = null;
                }
            }
            _inventoryData.Init(_inventorySize.x * 100 + _inventorySize.y);
            Save();
        }

        public void Init()
        {
            slots = new InventorySlot[_inventorySize.x, _inventorySize.y];
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    InventorySlot slot = Instantiate(InventoryManager.Instance.slotPrefab, _slotParent);
                    slots[x, y] = slot;
                }
            }
            _inventoryData.Init(_inventorySize.x * 100 + _inventorySize.y);
            Load();
        }

        public bool AddItem(Item item)
        {
            //이미 있는 아이템에 더하기
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    if (slots[x, y].assignedItem != null &&
                        slots[x, y].assignedItem.itemSO == item.itemSO &&
                        slots[x, y].assignedItem.isFull == false)
                    {
                        int remain = slots[x, y].AddAmount(item.amount);
                        if (remain != 0)
                        {
                            item.amount = remain;
                            AddItem(item);
                        }
                        else
                        {
                            if (item.gameObject.activeSelf)
                                Destroy(item.gameObject);
                        }
                        Save();
                        return true;
                    }
                }
            }

            //이미 있는 아이템이 없거나 가득 찼을때 하나 만들기
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    if (slots[x, y].assignedItem == null)
                    {
                        Item newItem = Instantiate(InventoryManager.Instance.itemPrefab);
                        newItem.Init();
                        newItem.itemSO = item.itemSO;
                        if (item.amount > item.itemSO.maxMergeAmount)
                        {
                            newItem.amount = item.itemSO.maxMergeAmount;
                            slots[x, y].SetItem(newItem);

                            item.amount -= item.itemSO.maxMergeAmount;
                            AddItem(item);
                        }
                        else
                        {
                            newItem.amount = item.amount;
                            slots[x, y].SetItem(newItem);
                        }
                        Save();
                        return true;
                    }
                }
            }
            Debug.Log("아이템 인벤이 가득 찼습니다.");
            return false;
        }
        public bool AddItem(ItemSO itemSO, int amount)
        {
            //이미 있는 아이템에 더하기
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    if (slots[x, y].assignedItem != null &&
                        slots[x, y].assignedItem.itemSO == itemSO &&
                        slots[x, y].assignedItem.isFull == false)
                    {
                        int remain = slots[x, y].AddAmount(amount);
                        if (remain != 0)
                        {
                            AddItem(itemSO, remain);
                        }
                        Save();
                        return true;
                    }
                }
            }

            //이미 있는 아이템이 없거나 가득 찼을때 하나 만들기
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    if (slots[x, y].assignedItem == null)
                    {
                        Item newItem = Instantiate(InventoryManager.Instance.itemPrefab);
                        newItem.Init();
                        newItem.itemSO = itemSO;
                        if (amount > itemSO.maxMergeAmount)
                        {
                            newItem.amount = itemSO.maxMergeAmount;
                            slots[x, y].SetItem(newItem);

                            amount -= itemSO.maxMergeAmount;
                            AddItem(itemSO, amount);
                        }
                        else
                        {
                            newItem.amount = amount;
                            slots[x, y].SetItem(newItem);
                        }
                        Save();
                        return true;
                    }
                }
            }
            return false;
        }

        public void Save()
        {
            for (int x = 0; x < _inventorySize.x; x++)
            {
                for (int y = 0; y < _inventorySize.y; y++)
                {
                    SlotSave newSlotSaveStruct = new SlotSave();
                    if (slots[x, y] != null && slots[x, y].assignedItem != null)
                    {
                        newSlotSaveStruct.pos = new Vector2Int(x, y);
                        newSlotSaveStruct.itemNameInt = (int)slots[x, y].assignedItem.itemSO.itemType;
                        newSlotSaveStruct.amount = slots[x, y].assignedItem.amount;
                    }
                    else
                        newSlotSaveStruct.amount = 0;
                    _inventoryData.slotDatas[x * 100 + y] = newSlotSaveStruct;
                }
            }
            _inventoryData.SaveJson("Inventory_" + name);
        }
        public void Load()
        {
            bool loadSucces = _inventoryData.LoadJson("Inventory_" + name);

            if (loadSucces == false || _inventoryData.slotDatas == null) return;

            int idx = 0;
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    InventorySlot slot;
                    if (idx < _slotParent.childCount)
                        slot = _slotParent.GetChild(idx).GetComponent<InventorySlot>();
                    else
                        slot = Instantiate(InventoryManager.Instance.slotPrefab, _slotParent);

                    SlotSave slotSave = _inventoryData.slotDatas[x * 100 + y];
                    if (slotSave != null && slotSave.itemNameInt != -1)
                    {
                        Item item = Instantiate(InventoryManager.Instance.itemPrefab);
                        item.Init();
                        item.itemSO = InventoryManager.Instance.ItemSODict[(ItemType)slotSave.itemNameInt];
                        item.amount = slotSave.amount;
                        slot.SetItem(item);
                    }
                    else if (slot.assignedItem != null)
                    {
                        slot.SetItem(null);
                    }
                    slots[x, y] = slot;

                    idx++;
                }
            }
        }
    }

    [Serializable]
    public class SlotSave
    {
        public Vector2Int pos;
        public int itemNameInt = -1;
        public int amount;
    }

    public class InventorySaveData : ISavable<InventorySaveData>
    {
        public SlotSave[] slotDatas;

        public void Init(int size)
        {
            slotDatas = new SlotSave[size + 1];
        }

        public void OnLoadData(InventorySaveData classData)
        {
            if (classData == null || classData.slotDatas == null || classData.slotDatas.Length == 0)
            {
                Debug.Log("불러올 데이터가 없습니다.");
                return;
            }

            slotDatas = classData.slotDatas;
        }

        public void OnSaveData(string savedFileName)
        {

        }
    }
}