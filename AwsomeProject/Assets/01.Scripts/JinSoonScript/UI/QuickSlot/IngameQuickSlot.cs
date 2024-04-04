using UnityEngine;
using UnityEngine.UI;

public class IngameQuickSlot : MonoBehaviour
{
    public PortionItemSO assignedItem { get; private set; }
    private PortionItem portion;
    private GameObject itemObj;

    public void SetItem(ItemSO item, int amount, bool isSelected)
    {
        if (assignedItem != null) return;

        assignedItem = item as PortionItemSO;
        
        itemObj = Instantiate(item.prefab, transform);
        if (isSelected == true)
            itemObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 45f);

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
        portion.RemoveItem(1);
        PlayerManager.instance.player.healthCompo.GetEffort(portion.posionEffect, portion.posionEffect.duration);
    }
}
