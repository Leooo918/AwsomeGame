using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Doryu.JBSave;

namespace Doryu.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private InventorySaveData _inventoryData = new InventorySaveData();
        
        [SerializeField] private Vector2Int _inventorySize;
        [SerializeField] private Transform _slotParent;
        [SerializeField] private Transform _itemParent;

        public void Init()
        {
            _inventoryData.slots = new InventorySlot[_inventorySize.x, _inventorySize.y];
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    InventorySlot slot = Instantiate(InventoryManager.Instance.slotPrefab, _slotParent);
                    _inventoryData.slots[x, y] = slot;
                }
            }
            Load();
        }

        public bool AddItem(Item item)
        {
            //이미 있는 아이템에 더하기
            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    if (_inventoryData.slots[x, y].assignedItem != null &&
                        _inventoryData.slots[x, y].assignedItem.itemSO == item.itemSO)
                    {
                        int remain = _inventoryData.slots[x, y].AddAmount(item.amount);
                        if (remain != 0)
                        {
                            item.amount = remain;
                            AddItem(item);
                        }
                        else
                        {
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
                    if (_inventoryData.slots[x, y].assignedItem == null)
                    {
                        if (item.amount > item.itemSO.maxMergeAmount)
                        {
                            Item newItem = Instantiate(InventoryManager.Instance.itemPrefab, _itemParent);
                            newItem.itemSO = item.itemSO;
                            newItem.amount = item.itemSO.maxMergeAmount;
                            _inventoryData.slots[x, y].SetItem(newItem);

                            item.amount -= item.itemSO.maxMergeAmount;
                            AddItem(item);
                        }
                        else
                        {
                            _inventoryData.slots[x, y].SetItem(item);
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
            _inventoryData.SaveJson("Inventory_" + name);
        }
        public void Load()
        {
            bool loadSucces = _inventoryData.LoadJson("Inventory_" + name);

            if (loadSucces == false) return;

            while (_slotParent.childCount != 0)
            {
                Destroy(_slotParent.GetChild(0).gameObject);
            }

            for (int y = 0; y < _inventorySize.y; y++)
            {
                for (int x = 0; x < _inventorySize.x; x++)
                {
                    InventorySlot slot = Instantiate(InventoryManager.Instance.slotPrefab, _slotParent);
                    if (_inventoryData.slots[x, y].assignedItem != null)
                        slot.SetItem(_inventoryData.slots[x, y].assignedItem);
                    _inventoryData.slots[x, y] = slot;
                }
            }
        }
    }

    public struct InventoryStruct
    {
        public Vector2Int pos;
        public Item item;
    }

    public class InventorySaveData : ISavable<InventorySaveData>
    {
        public InventorySlot[,] slots = new InventorySlot[0, 0];

        public void OnLoadData(InventorySaveData classData)
        {
            slots = classData.slots;
        }

        public void OnSaveData(string savedFileName)
        {

        }
    }
}