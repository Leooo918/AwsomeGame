using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
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

    public bool RemoveItem(int amount)
    {
        if (itemAmount < amount) return false;

        itemAmount -= amount;
        amountTxt.SetText(itemAmount.ToString());

        if (itemAmount <= 0)
        {
            if (assignedSlot != null)
                assignedSlot.DeleteItem();
            Destroy(gameObject);
        }

        return true;
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
        if (amountTxt == null)
            amountTxt = transform.Find("Amount").GetComponent<TextMeshProUGUI>();

        itemAmount = amount;
        amountTxt.SetText(itemAmount.ToString());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        visual.raycastTarget = false;
        InventoryManager.Instance.MoveItem(this);
        //transform._parent = selectItemParent;

        //�������� ���������� ���Կ��� �� �������� ���̻� �Ҵ������ ���� ������
        if (assignedSlot != null)
            assignedSlot.DeleteItem();

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
        //transform._parent = itemParent;

        Item toCombine = InventoryManager.Instance.combineableItem;
        if (toCombine != null && toCombine.itemSO.id == itemSO.id)
        {
            if(toCombine.itemAmount <= toCombine.maxCarryAmountPerSlot)
            {
                toCombine.SetItemAmount(itemAmount + toCombine.itemAmount);

                if (toCombine.itemAmount <= toCombine.maxCarryAmountPerSlot)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    SetItemAmount(maxCarryAmountPerSlot - itemAmount);
                    toCombine.SetItemAmount(toCombine.maxCarryAmountPerSlot);
                }
            }
        }

        //�������� ������ ���� ��
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

    public void ReturnToLastSlot()
    {
        lastSlot.InsertItem(this);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("��?");
        if (InventoryManager.Instance.curMovingItem != null)
        {
            InventoryManager.Instance.combineableItem = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("��!");
        InventoryManager.Instance.combineableItem = null;
    }
}
