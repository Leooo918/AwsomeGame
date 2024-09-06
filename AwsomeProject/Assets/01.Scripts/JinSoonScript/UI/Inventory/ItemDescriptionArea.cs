using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ItemDescriptionArea : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI explain;

    private InventorySlot _selectedSlot;

    private void Awake()
    {
        //InventoryManager.Instance.OnSelectedSlot += SetExplain;
    }

    public void SetExplain(InventorySlot slot)
    {
        if (_selectedSlot != null)
        {
            _selectedSlot.isSelected = false;
            _selectedSlot.OnMouse(false);
            _selectedSlot.OnSelect(false);
        }
        if (slot == null || _selectedSlot == slot || slot.assignedItem == null)
        {
            _selectedSlot = null;
            itemName.SetText("");
            explain.SetText("");
            icon.color = new Color(1, 1, 1, 0);
            return;
        }
        if (_selectedSlot != slot)
        {
            _selectedSlot = slot;
            _selectedSlot.isSelected = true;
            _selectedSlot.OnMouse(true);
            _selectedSlot.OnSelect(true);
        }

        //if (_selectedSlot != null && Input.GetMouseButtonDown(0))
        //{
        //    _selectedSlot = null;
        //}

         
        icon.color = new Color(1, 1, 1, 1);
        ItemSO itemSO = slot.assignedItem.itemSO;
        itemName.SetText(itemSO.itemName);
        explain.SetText(itemSO.itemExplain);
        icon.sprite = itemSO.image;
    }
}
