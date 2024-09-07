using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    private Item assignedItem;
    private Transform _itemParent;
    private Image _coolTimeImage;
    private TextMeshProUGUI _coolTimeText;

    private void Awake()
    {
        _itemParent = transform.Find("PotionParent");
        _coolTimeImage = transform.Find("CoolTime").GetComponent<Image>();
        _coolTimeText = _coolTimeImage.transform.Find("Timer").GetComponent<TextMeshProUGUI>();


        _coolTimeImage.fillAmount = 0;
        _coolTimeText.text = "";
    }

    public void SetPotion(InventorySlot inventorySlot)
    {
        if (inventorySlot.assignedItem == null)
        {
            if (assignedItem != null) 
                Destroy(assignedItem.gameObject);
            return;
        }
        if (assignedItem == null)
        {
            assignedItem = Instantiate(InventoryManager.Instance.quickSlotItemPrefab, _itemParent);
            assignedItem.Init();
        }
        assignedItem.amount = inventorySlot.assignedItem.amount;
        assignedItem.itemSO = inventorySlot.assignedItem.itemSO;
    }

    public bool UsePotion()
    {
        return false;
    }
}
