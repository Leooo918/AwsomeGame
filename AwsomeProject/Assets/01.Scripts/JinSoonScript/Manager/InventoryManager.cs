using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public ItemSetSO ItemSet;
    private Inventory inventory;
    public Item curMovingItem { get; private set; }
    public InventorySlot curCheckingSlot { get; private set; }

    public Transform itemParent;
    public Transform explainparent;

    private TextMeshProUGUI explainName;
    private TextMeshProUGUI explainTxt;
    private Image explainImage;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;

        explainName = explainparent.Find("Name").GetComponent<TextMeshProUGUI>();
        explainTxt = explainparent.Find("Explain").GetComponent<TextMeshProUGUI>();
        explainImage = explainparent.Find("Image").GetComponent<Image>();
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
}
