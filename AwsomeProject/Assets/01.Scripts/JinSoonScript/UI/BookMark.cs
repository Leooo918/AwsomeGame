using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookMark : MonoBehaviour, IPointerClickHandler
{
    public BookMark another;

    private Image image;
    [SerializeField] private Sprite enableSprite;
    [SerializeField] private Sprite disableSprite;

    [SerializeField] private bool isEnableInventory = true;
    public event UnityAction<bool> clickEvent;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (isEnableInventory)
        {
            Enable();
            clickEvent?.Invoke(isEnableInventory);
            InventoryManager.Instance.EnbableIgredientsInventory(isEnableInventory);
        }
    }

    public void Enable()
    {
        another.Disable();
        image.sprite = enableSprite;
    }

    public void Disable()
    {
        image.sprite = disableSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Enable();
        clickEvent?.Invoke(isEnableInventory);
        InventoryManager.Instance.EnbableIgredientsInventory(isEnableInventory);
    }
}
