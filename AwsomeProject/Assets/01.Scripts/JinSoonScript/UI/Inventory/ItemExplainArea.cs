using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemExplainArea : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI explain;

    private void Awake()
    {
        inventory.OnSelectItem += SetExplain;
    }

    public void SetExplain(ItemSO item)
    {
        if (item == null)
        {
            itemName.SetText("");
            explain.SetText("");
            icon.color = new Color(1, 1, 1, 0);
            return;
        }

        icon.color = new Color(1, 1, 1, 1);
        itemName.SetText(item.itemName);
        explain.SetText(item.itemExplain);
        icon.sprite = item.itemImage;
    }
}
