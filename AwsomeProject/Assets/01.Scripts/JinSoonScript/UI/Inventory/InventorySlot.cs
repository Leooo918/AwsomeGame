using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rect;
    public Item assignedItem { get; private set; }

    private Inventory inventory;
    private  GameObject selectUI;
    public bool isSelectedNow = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        selectUI = transform.Find("SelectedUI").gameObject;
        selectUI.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale = new Vector3(1.05f, 1.05f, 1f);
        if (assignedItem != null) return;
        InventoryManager.Instance.CheckSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = new Vector3(1f, 1f, 1f);
        if (assignedItem != null) return;
        InventoryManager.Instance.CheckSlot(null);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Select();

        InventoryManager.Instance.SetExplain(assignedItem != null ? assignedItem.itemSO : null);
    }

    public void InsertItem(Item item)
    {
        assignedItem = item;
        Vector3 position = Vector3.zero;
        foreach(var r in transform.GetComponentsInParent<RectTransform>())
        {
            if (r.GetComponent<Canvas>() != null) continue;
            position += r.localPosition;
            Debug.Log($"{r}:{r.localPosition}");
        }
        item.GetComponent<RectTransform>().localPosition = position;
    }

    public void DeleteItem(Item item)
    {
        assignedItem = null;
    }

    public void Select()
    {
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
