using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public ItemSetSO ItemSet;
    [SerializeField] private Inventory playerInventory;
    public Inventory PlayerInventory => playerInventory;
    public Item curMovingItem { get; private set; }
    public InventorySlot curCheckingSlot { get; private set; }

    public RectTransform inventoryRect;
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
        if (Instance != null)
            Destroy(Instance);

        Instance = this;

        explainName = explainparent.Find("Name").GetComponent<TextMeshProUGUI>();
        explainTxt = explainparent.Find("Explain").GetComponent<TextMeshProUGUI>();
        explainImage = explainparent.Find("Image").GetComponent<Image>();
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
}
