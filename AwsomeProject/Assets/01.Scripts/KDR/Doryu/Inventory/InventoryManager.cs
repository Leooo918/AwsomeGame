using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Doryu.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager _instance;
        public static InventoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InventoryManager>();
                    if ( _instance == null )
                    {
                        Debug.LogWarning("싱글톤 오브젝트 안넣었어요");
                    }
                }
                return _instance;
            }
        }

        public Dictionary<ItemName, Item> ItemDict { get; private set; } 
            = new Dictionary<ItemName, Item>();
        [field:SerializeField] public InventorySlot slotPrefab { get; private set; }
        [field:SerializeField] public Item itemPrefab { get; private set; }
        [SerializeField] private ItemListSO itemListSO;
        [SerializeField] private Inventory[] inventories;

        private void Awake()
        {
            inventories = FindObjectsByType<Inventory>(FindObjectsSortMode.None);

            foreach (ItemSO itemSO in itemListSO.itemSOList)
            {
                ItemDict.Add(itemSO.itemName, itemSO.prefab);
            }

            for (int i = 0; i < inventories.Length; i++)
            {
                inventories[i].Init();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Item item = ItemDict[ItemName.RedMushroom];
                item.amount = 1;
                AddItem(item);
            }
        }

        public void AddItem(Item item)
        {
            for (int i = 0; i < inventories.Length; i++)
            {
                inventories[i].AddItem(item);
            }
        }
    }
}
