using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemExplainArea : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI explain;

    private void Awake()
    {
        InventoryManager.Instance.OnSelectedSlot += SetExplain;
    }

    public void SetExplain(InventorySlot slot)
    {
        if (slot == null || slot.assignedItem == null)
        {
            itemName.SetText("");
            explain.SetText("");
            icon.color = new Color(1, 1, 1, 0);
            return;
        }

        icon.color = new Color(1, 1, 1, 1);
        ItemSO itemSO = slot.assignedItem.itemSO;
        itemName.SetText(itemSO.itemName);
        explain.SetText(itemSO.itemExplain);
        icon.sprite = itemSO.image;
    }
}
