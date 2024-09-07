using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    private Transform _itemParent;
    private Image _coolTimeImage;
    private Image _selectedImage;
    private TextMeshProUGUI _coolTimeText;

    public Item assignedItem { get; private set; }

    private void Awake()
    {
        _itemParent = transform.Find("PotionParent");
        _coolTimeImage = transform.Find("CoolTime").GetComponent<Image>();
        _selectedImage = transform.Find("SelectedImage").GetComponent<Image>();
        _coolTimeText = _coolTimeImage.transform.Find("Timer").GetComponent<TextMeshProUGUI>();


        _coolTimeImage.fillAmount = 0;
        _coolTimeText.text = "";
        OnSelect(false);
    }

    public void OnSelect(bool on)
    {
        _selectedImage.color = on ? Color.white : new Color(1, 1, 1, 0);
    }

    public void SetPotion(InventorySlot inventorySlot)
    {
        if (inventorySlot.assignedItem == null)
        {
            Debug.Log("°¥2");
            if (assignedItem != null)
            {
                Debug.Log("°¥");
                Destroy(assignedItem.gameObject);
                assignedItem = null;
            }
            return;
        }
        if (assignedItem == null)
        {
            assignedItem = Instantiate(InventoryManager.Instance.itemPrefab, _itemParent);
            assignedItem.Init();
        }
        assignedItem.amount = inventorySlot.assignedItem.amount;
        assignedItem.itemSO = inventorySlot.assignedItem.itemSO;
        assignedItem.SetSlot();
    }

    public bool UsePotion()
    {
        return false;
    }
}
