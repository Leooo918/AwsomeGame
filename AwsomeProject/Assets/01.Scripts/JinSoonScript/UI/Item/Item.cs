using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public ItemSO itemSO;

    #region Propertieis

    public int itemId { get; protected set; }
    public string itemName { get; protected set; }
    public ItemType itemType { get; protected set; }
    public int maxCarryAmountPerSlot { get; protected set; }
    public string itemExplain { get; protected set; }

    public Sprite itemImage { get; protected set; }
    public GameObject prefab { get; protected set; }

    #endregion

    protected RectTransform rect;
    protected Image visual;
    protected InventorySlot assignedSlot;
    protected InventorySlot lastSlot;
    protected TextMeshProUGUI amountTxt;

    private Vector2 offset;
    public int itemAmount { get; protected set; } = 0;

    protected virtual void Awake()
    {
        visual = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        amountTxt = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
    }

    public bool AddItem(int amount)
    {
        if (itemAmount + amount <= itemSO.maxCarryAmountPerSlot)
        {
            itemAmount += amount;
            amountTxt.SetText(itemAmount.ToString());
            return true;
        }
        return false;
    }

    public void SetItemAmount(int amount)
    {
        itemAmount = amount;
        amountTxt.SetText(itemAmount.ToString());
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

        lastSlot = assignedSlot;
        assignedSlot = InventoryManager.Instance.curCheckingSlot;
        assignedSlot.InsertItem(this);


        assignedSlot.Select();

        InventoryManager.Instance.SetExplain(itemSO);
    }

    public void Init(int amount, InventorySlot slot)
    {
        itemAmount = amount;
        assignedSlot = slot;
        amountTxt.SetText(itemAmount.ToString());

        itemId = itemSO.id;
        itemName = itemSO.name;
        itemType = itemSO.itemType;
        maxCarryAmountPerSlot = itemSO.maxCarryAmountPerSlot;
        itemExplain = itemSO.itemExplain;
        itemImage = itemSO.itemImage;
        prefab = itemSO.prefab;
    }
}
