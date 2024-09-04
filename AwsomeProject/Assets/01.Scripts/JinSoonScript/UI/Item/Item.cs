//using TMPro;
//using UnityEditor.Build.Content;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;


//public abstract class Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler
//{
//    public ItemSO itemSO;

//    #region Propertieis

//    public int itemId { get; protected set; }
//    public string itemName { get; protected set; }
//    public ItemType itemType { get; protected set; }
//    public int maxCarryAmountPerSlot { get; protected set; }
//    public string itemExplain { get; protected set; }

//    public Sprite itemImage { get; protected set; }
//    public GameObject prefab { get; protected set; }
//    public Image visual { get; protected set; }


//    #endregion

//    protected RectTransform rect;
//    public InventorySlot currentSlot;
//    protected InventorySlot prevSlot;
//    protected TextMeshProUGUI amountTxt;

//    private Vector2 offset;
//    public int itemAmount { get; protected set; } = 0;

//    protected virtual void Awake()
//    {
//        visual = GetComponent<Image>();
//        rect = GetComponent<RectTransform>();
//        amountTxt = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
//    }

//    public bool RemoveItem(int amount)
//    {
//        if (itemAmount < amount) return false;

//        itemAmount -= amount;
//        amountTxt.SetText(itemAmount.ToString());

//        if (itemAmount <= 0)
//        {
//            if (currentSlot != null)
//                currentSlot.DeleteItem();

//            //currentSlot.inventory.
//            Destroy(gameObject);
//        }

//        return true;
//    }

//    public bool AddItem(int amount)
//    {
//        //Destroy(gameObject);
//        if (itemAmount + amount <= itemSO.maxCarryAmountPerSlot)
//        {
//            Debug.Log(itemAmount + " + " + amount);
//            itemAmount += amount;
//            amountTxt.SetText(itemAmount.ToString());
//            //Debug.Log(itemName + " : " + itemAmount + " : " + amount);
//            return true;
//        }
//        return false;
//    }

//    public void SetItemAmount(int amount)
//    {
//        if (amountTxt == null)
//            amountTxt = transform.Find("Amount").GetComponent<TextMeshProUGUI>();

//        itemAmount = amount;
//        amountTxt.SetText(itemAmount.ToString());
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        visual.raycastTarget = false;
//        InventoryManager.Instance.MoveItem(this);
//        //transform._parent = selectItemParent;

//        //아이템을 선택했을때 슬롯에서 그 아이템은 더이상 할당되있지 않은 상태인
//        if (currentSlot != null)
//            currentSlot.DeleteItem();

//        offset = eventData.position - new Vector2(Screen.width / 2, Screen.height / 2) - rect.anchoredPosition;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        rect.anchoredPosition3D = eventData.position - new Vector2(Screen.width / 2, Screen.height / 2) - offset;
//    }

//    public void OnPointerUp(PointerEventData eventData)
//    {
//        visual.raycastTarget = true;
//        InventoryManager.Instance.MoveItem(null);
//        //transform._parent = itemParent;

//        //Item toCombine = InventoryManager.Instance.combineableItem;
//        //if (toCombine != null && toCombine.itemSO.id == itemSO.id)
//        //{
//        //    if(toCombine.itemAmount <= toCombine.maxCarryAmountPerSlot)
//        //    {
//        //        toCombine.SetItemAmount(itemAmount + toCombine.itemAmount);

//        //        if (toCombine.itemAmount <= toCombine.maxCarryAmountPerSlot)
//        //        {
//        //            Destroy(this.gameObject);
//        //        }
//        //        else
//        //        {
//        //            SetItemAmount(maxCarryAmountPerSlot - itemAmount);
//        //            toCombine.SetItemAmount(toCombine.maxCarryAmountPerSlot);
//        //        }
//        //    }
//        //}

//        //선택중인 슬롯이 없을 때
//        if (InventoryManager.Instance.curCheckingSlot == null)
//        {
//            if (currentSlot != null)
//                currentSlot.InsertItem(this);

//            return;
//        }

//        prevSlot = currentSlot;
//        currentSlot = InventoryManager.Instance.curCheckingSlot;
//        currentSlot.InsertItem(this);


//        currentSlot.Select();

//        InventoryManager.Instance.SetExplain(itemSO);
//    }

//    public void ReturnToLastSlot()
//    {
//        Debug.Log("Re : " + itemAmount);
//        prevSlot.InsertItem(this);
//    }

//    public void Init(int amount, InventorySlot slot)
//    {
//        itemAmount = amount;
//        currentSlot = slot;
//        Debug.Log(currentSlot.GetInstanceID() + " " + slot.GetInstanceID());
//        amountTxt.SetText(itemAmount.ToString());

//        itemId = itemSO.id;
//        itemName = itemSO.name;
//        itemType = itemSO.itemType;
//        maxCarryAmountPerSlot = itemSO.maxCarryAmountPerSlot;
//        itemExplain = itemSO.itemExplain;
//        itemImage = itemSO.itemImage;
//        prefab = itemSO.prefab;
//    }

//    private void OnAnimatorIK(int layerIndex)
//    {
        
//    }

//    public void OnPointerEnter(PointerEventData eventData)
//    {

//    }
//}
