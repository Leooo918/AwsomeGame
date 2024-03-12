using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public ItemSO itemSO;

    private RectTransform rect;
    private Image visual;
    private InventorySlot assignedSlot;

    private Vector2 offset;
    public int itemAmount { get; private set; } = 0;

    private void Awake()
    {
        visual = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public bool AddItem(int amount)
    {
        if (itemAmount + amount < itemSO.maxCarryAmountPerSlot)
        {
            itemAmount += amount;
            return true;
        }
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        visual.raycastTarget = false;
        InventoryManager.Instance.MoveItem(this);

        if (assignedSlot != null)
            assignedSlot.DeleteItem(this);

        offset = eventData.position - new Vector2(Screen.width / 2, Screen.height / 2) - rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition3D = eventData.position - new Vector2(Screen.width / 2, Screen.height / 2) - offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        visual.raycastTarget = true;
        InventoryManager.Instance.MoveItem(null);

        if (InventoryManager.Instance.curCheckingSlot == null)
        {
            if (assignedSlot != null)
                assignedSlot.InsertItem(this);
            return;
        }

        assignedSlot = InventoryManager.Instance.curCheckingSlot;
        assignedSlot.InsertItem(this);
    }
}
