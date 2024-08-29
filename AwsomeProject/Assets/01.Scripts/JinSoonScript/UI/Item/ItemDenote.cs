using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDenote : MonoBehaviour
{
    [SerializeField] private GameObject itemPf;
    [SerializeField ]private ItemType itemType;

    public void SetItems()
    {
        //InventorySlot[,] inventory = InventoryManager.Instance.PlayerInventory.inventory;

        //for(int i = 0; i  < transform.childCount; i++)
        //{
        //    Destroy(transform.GetChild(0).gameObject);
        //}

        //for (int i = 0; i < inventory.GetLength(0); i++)
        //{
        //    for (int j = 0; j < inventory.GetLength(1); j++)
        //    {
        //        if (inventory[i, j].assignedItem != null && inventory[i, j].assignedItem.itemSO.itemType == itemType)
        //        {
        //            Image img = Instantiate(itemPf, transform).GetComponent<Image>();
        //            TextMeshProUGUI amount = img.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        //            img.sprite = inventory[i, j].assignedItem.itemSO.itemImage;
        //            amount.SetText($"{inventory[i, j].assignedItem.itemAmount}");
        //        }
        //    }
        //}
    }
}
