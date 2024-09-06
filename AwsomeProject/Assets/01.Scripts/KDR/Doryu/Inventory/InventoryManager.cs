using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;


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
    [SerializeField] private Inventory throwPotionInventory;
    [SerializeField] private Inventory drinkPotionInventory;

    private List<Inventory> _inventories;

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

        _inventories = new List<Inventory>();
        _inventories = FindObjectsByType<Inventory>(FindObjectsSortMode.None).ToList();

        _inventories.ForEach(inven =>
        {
            inven.Init();
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TryAddItem(ItemSODict[ItemType.RedMushroom]);
        }
        if (Input.GetKeyDown(KeyCode.P))
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
        dragItemSlot.assignedItem.transform.localPosition = 
            mousePos - (Vector2)dragItemSlot.transform.parent.position;
    }

    public bool TryAddItem(Item item)
    {
        bool succes = false;
        if (item.itemSO is IngredientItemSO)
        {
            succes = ingredientInventory.AddItem(item);
        }
        else if (item.itemSO is PotionItemListSO drinkPotionSO && 
            drinkPotionSO.potionType == PotionType.Drink)
        {
            succes = drinkPotionInventory.AddItem(item);
        }
        else if (item.itemSO is PotionItemListSO throwPotionSO && 
            throwPotionSO.potionType == PotionType.Throw)
        {
            succes = throwPotionInventory.AddItem(item);
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
        else if (itemSO is PotionItemListSO drinkPotionSO &&
            drinkPotionSO.potionType == PotionType.Drink)
        {
            succes = drinkPotionInventory.AddItem(itemSO, amount);
        }
        else if (itemSO is PotionItemListSO throwPotionSO &&
            throwPotionSO.potionType == PotionType.Throw)
        {
            succes = throwPotionInventory.AddItem(itemSO, amount);
        }
        return succes;
    }
}

