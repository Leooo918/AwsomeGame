using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rect;

    private Inventory inventory;
    private GameObject selectUI;
    public Item assignedItem { get; private set; }
    public bool isSelectedNow { get; private set; } = false;

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        selectUI = transform.Find("SelectedUI").gameObject;
        selectUI.SetActive(false);
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale = new Vector3(1.05f, 1.05f, 1f);
        if (assignedItem != null) return;
        InventoryManager.Instance.CheckSlot(this);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = new Vector3(1f, 1f, 1f);
        if (assignedItem != null) return;
        InventoryManager.Instance.CheckSlot(null);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Select();

        InventoryManager.Instance.SetExplain(assignedItem != null ? assignedItem.itemSO : null);
    }

    public virtual void InsertItem(Item item)
    {
        assignedItem = item;
        Vector3 position = Vector3.zero;
        foreach (var r in transform.GetComponentsInParent<RectTransform>())
        {
            if (r.GetComponent<Canvas>() != null || r.name == "InventoryBackground") break;
            position += r.localPosition;
        }

        item.GetComponent<RectTransform>().localPosition = position;
        item.Init(item.itemAmount, this);
    }

    public void DeleteItem()
    {
        assignedItem = null;
    }

    public void UseItem()
    {
        if (assignedItem != null)
            Destroy(assignedItem.gameObject);
        assignedItem = null;
    }

    public void Select()
    {
        if (inventory != null)
            inventory.UnSelectAllSlot();

        selectUI.SetActive(true);
    }

    public void UnSelect()
    {
        selectUI.SetActive(false);
    }

    public void Init(Inventory inventory)
    {
        this.inventory = inventory;
    }
}
