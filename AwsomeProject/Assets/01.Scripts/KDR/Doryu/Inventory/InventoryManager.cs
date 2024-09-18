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

    public Dictionary<IngredientItemType, ItemSO> IngredientItemSODict { get; private set; } 
        = new Dictionary<IngredientItemType, ItemSO>();
    public Dictionary<PotionItemType, ItemSO> ThrowPotionItemSODict { get; private set; } 
        = new Dictionary<PotionItemType, ItemSO>();
    public Dictionary<PotionItemType, ItemSO> DrinkPotionItemSODict { get; private set; } 
        = new Dictionary<PotionItemType, ItemSO>();
    [field:SerializeField] public InventorySlot slotPrefab { get; private set; }
    [field:SerializeField] public Item itemPrefab { get; private set; }
    [SerializeField] private ItemListSO itemListSO;
    [SerializeField] private Inventory ingredientInventory;
    [SerializeField] private Inventory throwPotionInventory;
    [SerializeField] private Inventory drinkPotionInventory;

    private List<Inventory> _inventories;

    [HideInInspector] public InventorySlot dragItemSlot;
    [HideInInspector] public InventorySlot stayMouseSlot;

    private void Awake()
    {
        foreach (IngredientItemSO itemSO in itemListSO.ingredientItemSOList)
        {
            IngredientItemSODict.Add(itemSO.itemType, itemSO);
        }
        foreach (ThrowPotionItemSO itemSO in itemListSO.throwPotionItemSOList)
        {
            ThrowPotionItemSODict.Add(itemSO.itemType, itemSO);
        }
        foreach (DrinkPotionItemSO itemSO in itemListSO.drinkPotionItemSOList)
        {
            DrinkPotionItemSODict.Add(itemSO.itemType, itemSO);
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
        #region Debug
        if (Input.GetKeyDown(KeyCode.I))
        {
            TryAddItem(IngredientItemSODict[IngredientItemType.RedMushroom]);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TryAddItem(ThrowPotionItemSODict[PotionItemType.WindPotion], level:2);
            TryAddItem(ThrowPotionItemSODict[PotionItemType.WeakPotion], level:2);
            TryAddItem(ThrowPotionItemSODict[PotionItemType.StonePotion], level:2);
            TryAddItem(ThrowPotionItemSODict[PotionItemType.HealPotion], level:2);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            TryAddItem(DrinkPotionItemSODict[PotionItemType.WindPotion], level: 2);
            TryAddItem(DrinkPotionItemSODict[PotionItemType.WeakPotion], level: 2);
            TryAddItem(DrinkPotionItemSODict[PotionItemType.StonePotion], level: 2);
            TryAddItem(DrinkPotionItemSODict[PotionItemType.HealPotion], level: 2);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TryAddItem(ThrowPotionItemSODict[PotionItemType.HealPotion]);
        }
        #endregion

        if (dragItemSlot != null && dragItemSlot.assignedItem != null)
        {
            DragUpdate();
        }
    }

    public void DragUpdate()
    {
        Vector2 mousePos = Input.mousePosition;
        dragItemSlot.assignedItem.transform.localPosition = 
            mousePos - (Vector2)dragItemSlot.inventory.itemStorage.position;
    }

    public bool TryAddItem(Item item)
    {
        bool succes = false;
        if (item.itemSO is IngredientItemSO)
        {
            succes = ingredientInventory.AddItem(item);
        }
        else if (item.itemSO is ThrowPotionItemSO)
        {
            succes = throwPotionInventory.AddItem(item);
        }
        else if (item.itemSO is DrinkPotionItemSO)
        {
            succes = drinkPotionInventory.AddItem(item);
        }
        return succes;
    }
    public bool TryAddItem(ItemSO itemSO, int amount = 1, int level = 0)
    {
        bool succes = false;
        if (itemSO is IngredientItemSO)
        {
            succes = ingredientInventory.AddItem(itemSO, amount, level);
        }
        else if (itemSO is ThrowPotionItemSO throwPotionSO)
        {
            succes = throwPotionInventory.AddItem(itemSO, amount, level);
        }
        else if (itemSO is DrinkPotionItemSO drinkPotionSO)
        {
            succes = drinkPotionInventory.AddItem(itemSO, amount, level);
        }
        return succes;
    }

    [ContextMenu("ResetSaveData")]
    public void ResetSaveDate()
    {
        if (_inventories == null)
        {
            _inventories = new List<Inventory>();
            _inventories = FindObjectsByType<Inventory>(FindObjectsSortMode.None).ToList();
        }

        _inventories.ForEach(inven => inven.ResetSaveData());
    }
}

