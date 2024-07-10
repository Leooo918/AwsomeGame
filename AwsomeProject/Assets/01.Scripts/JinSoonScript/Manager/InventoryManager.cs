using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : Singleton<InventoryManager>
{
    public ItemSetSO ItemSet;
    [SerializeField] private Inventory playerInventory;
    private QuickSlotSet quickslot;
    public Inventory PlayerInventory => playerInventory;
    public QuickSlotSet QuickSlot => quickslot;
    public Item curMovingItem { get; private set; }
    public InventorySlot curCheckingSlot { get; private set; }

    public RectTransform inventoryRect;

    [SerializeField] private RectTransform ingredientsInventory;
    [SerializeField] private RectTransform portionInventory;
    private bool isIngredientsInventoryActive = true;

    public Transform itemParent;
    public Transform explainparent;

    [SerializeField] private InputReader inputReader;

    private TextMeshProUGUI explainName;
    private TextMeshProUGUI explainTxt;
    private Image explainImage;

    private Tween tween;
    private bool inventoryOpen = false;

    private void Awake()
    {
        explainName = explainparent.Find("Name/Txt").GetComponent<TextMeshProUGUI>();
        explainTxt = explainparent.Find("Explain/Txt").GetComponent<TextMeshProUGUI>();
        explainImage = explainparent.Find("Frame/Image").GetComponent<Image>();
        SetExplain(null);

        EnbableIgredientsInventory(true);
    }

    private void OnEnable()
    {
        inputReader.PressTabEvent += OnPressTab;
    }

    private void OnDisable()
    {
        inputReader.PressTabEvent -= OnPressTab;
    }

    public void MoveItem(Item item) => curMovingItem = item;
    public void CheckSlot(InventorySlot slot) => curCheckingSlot = slot;

    public void SetExplain(ItemSO itemSO)
    {
        if (itemSO == null)
        {
            explainName.SetText("");
            explainTxt.SetText("");
            explainImage.color = new Color(1, 1, 1, 0);
            return;
        }

        explainImage.color = new Color(1, 1, 1, 1);
        explainName.SetText(itemSO.itemName);
        explainTxt.SetText(itemSO.itemExplain);
        explainImage.sprite = itemSO.itemImage;
    }

    public void OnPressTab()
    {
        if (tween != null && tween.active)
            tween.Kill();

        if (inventoryOpen == false)
            OpenInventory();
        else
            CloseInventory();
    }

    public void OpenInventory()
    {
        tween = inventoryRect.DOAnchorPosY(0, 0.3f).SetEase(Ease.Linear);
        inventoryOpen = true;
    }

    public void CloseInventory()
    {
        tween = inventoryRect.DOAnchorPosY(-1100, 0.3f).SetEase(Ease.Linear);
        inventoryOpen = false;
    }

    public Item MakeItemInstanceByItemSO(ItemSO itemSO, int amount = 1)
    {
        if (itemSO == null) return null;
        Item item = Instantiate(itemSO.prefab, itemParent).GetComponent<Item>();
        item.SetItemAmount(amount);

        return item;
    }

    public void EnbableIgredientsInventory(bool isEnable)
    {
        isIngredientsInventoryActive = isEnable;

        if (isIngredientsInventoryActive)
        {
            ingredientsInventory.gameObject.SetActive(true);
            portionInventory.gameObject.SetActive(false);
        }
        else
        {

            ingredientsInventory.gameObject.SetActive(false);
            portionInventory.gameObject.SetActive(true);
        }
    }
}
