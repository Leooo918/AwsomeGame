using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


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

    public Dictionary<ItemType, ItemSO> ItemSODict { get; private set; } 
        = new Dictionary<ItemType, ItemSO>();
    [field:SerializeField] public InventorySlot slotPrefab { get; private set; }
    [field:SerializeField] public Item itemPrefab { get; private set; }
    [SerializeField] private ItemListSO itemListSO;
    [SerializeField] private Inventory ingredientInventory;
    [SerializeField] private Inventory potionInventory;

    [HideInInspector] public InventorySlot dragItemSlot;
    [HideInInspector] public InventorySlot stayMouseSlot;

    public Action<InventorySlot> OnSelectedSlot;
    private InventorySlot _selectedSlot;
    [HideInInspector]
    public InventorySlot SelectedSlot
    {
        get => _selectedSlot;
        set
        {
            _selectedSlot?.OnMouse(false);
            _selectedSlot?.OnSelect(false);
            _selectedSlot = value;
            _selectedSlot?.OnMouse(true);
            _selectedSlot?.OnSelect(true);
            OnSelectedSlot?.Invoke(_selectedSlot);
        }
    }

    private void Awake()
    {
        foreach (ItemSO itemSO in itemListSO.itemSOList)
        {
            ItemSODict.Add(itemSO.itemType, itemSO);
        }

        ingredientInventory.Init();
        potionInventory?.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TryAddItem(ItemSODict[ItemType.RedMushroom]);
        }
        if (_selectedSlot != null && Input.GetMouseButtonDown(0))
        {
            SelectedSlot = null;
        }

        if (dragItemSlot != null && dragItemSlot.assignedItem != null)
        {
            DragUpdate();
        }
    }

    public void DragUpdate()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    dragItemSlot.assignedItem.SetSlot();
        //    dragItemSlot = null;
        //    return;
        //}
         
        Vector2 mousePos = Input.mousePosition;
        dragItemSlot.assignedItem.transform.localPosition = mousePos - (Vector2)dragItemSlot.transform.parent.position;
    }

    public bool TryAddItem(Item item)
    {
        bool succes = false;
        if (item.itemSO is IngredientItemSO)
        {
            succes = ingredientInventory.AddItem(item);
        }
        if (item.itemSO is PotionItemListSO)
        {
            succes = potionInventory.AddItem(item);
        }
        return succes;
    }
    public bool TryAddItem(ItemSO itemSO, int amount = 1)
    {
        bool succes = false;
        if (itemSO is IngredientItemSO)
        {
            succes = ingredientInventory.AddItem(itemSO, amount);
        }
        if (itemSO is PotionItemListSO)
        {
            succes = potionInventory.AddItem(itemSO, amount);
        }
        return succes;
    }
}

