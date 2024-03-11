using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rect;
    private Image itemImage;
    private Item assignedItem;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        itemImage = transform.Find("ItemVisual").GetComponent<Image>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale = new Vector3(1.05f, 1.05f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = new Vector3(1f, 1f, 1f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
