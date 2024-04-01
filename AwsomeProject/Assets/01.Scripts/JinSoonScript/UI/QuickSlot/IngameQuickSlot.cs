using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameQuickSlot : MonoBehaviour
{
    public PortionItemSO assignedItem { get; private set; }
    private PortionItem portion;
    private GameObject itemObj;
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    public void SetItem(ItemSO item, int amount)
    {
        if (assignedItem != null) return;

        assignedItem = item as PortionItemSO;
        itemObj = Instantiate(item.prefab,transform);
        portion = itemObj.GetComponent<PortionItem>();
        itemObj.GetComponent<Image>().raycastTarget = false;
        portion.Init(amount, null);
    }

    public void DeleteItem()
    {
        assignedItem = null;
        Destroy(itemObj);
    }

    public void UseItem()
    {
        player.playerHealth.GetEffort(portion.posionEffect, portion.posionEffect.duration);
    }
}
