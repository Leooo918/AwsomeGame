using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    public Item assignedItem { get; private set; }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale = new Vector3(1.05f, 1.05f, 1f);
        InventoryManager.Instance.CheckSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = new Vector3(1f, 1f, 1f);
        InventoryManager.Instance.CheckSlot(null);
    }

    public void InsertItem(Item item)
    {
        assignedItem = item;
        item.GetComponent<RectTransform>().localPosition = rect.localPosition - new Vector3(-370f, -88f);
    }

    public void DeleteItem(Item item)
    {
        assignedItem = null;
    }
}
