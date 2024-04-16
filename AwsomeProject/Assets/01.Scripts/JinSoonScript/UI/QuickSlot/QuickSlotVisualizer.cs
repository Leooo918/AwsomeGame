using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class QuickSlotVisualizer : MonoBehaviour
{
    private IngameQuickSlot[] slots = new IngameQuickSlot[5];
    private Sequence seq;
    private RectTransform rect; 
    [SerializeField] private Player player;

    private Tween tween;

    private int selectedSlot = -1;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
            slots[i] = transform.GetChild(i).GetComponent<IngameQuickSlot>();

        rect = transform.parent.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        player.PlayerInput.FirstQuickSlot += () => SelectOneSlot(0);
        player.PlayerInput.SecondQuickSlot += () => SelectOneSlot(1);
        player.PlayerInput.ThirdQuickSlot += () => SelectOneSlot(2);
        player.PlayerInput.ForthQuickSlot += () => SelectOneSlot(3);
        player.PlayerInput.FifthQuickSlot += () => SelectOneSlot(4);

        player.PlayerInput.OnUseQuickSlot += UseQuickSlot;
    }

    private void OnDisable()
    {
        player.PlayerInput.FirstQuickSlot -= () => SelectOneSlot(0);
        player.PlayerInput.SecondQuickSlot -= () => SelectOneSlot(1);
        player.PlayerInput.ThirdQuickSlot -= () => SelectOneSlot(2);
        player.PlayerInput.ForthQuickSlot -= () => SelectOneSlot(3);
        player.PlayerInput.FifthQuickSlot -= () => SelectOneSlot(4);

        player.PlayerInput.OnUseQuickSlot -= UseQuickSlot;
    }

    public void SetQuickSlot()
    {
        for (int i = 0; i < 5; i++)
        {
            slots[i].DeleteItem();
            Item item = InventoryManager.Instance.PlayerInventory.quickSlot[i].assignedItem;
            if (item != null)
                slots[i].SetItem(item.itemSO, item.itemAmount, i == selectedSlot);
        }
    }

    public void SelectOneSlot(int num)
    {
        if (seq != null && seq.active)
            seq.Kill();

        QuickSlotManager.Instance.curSelectingPortion = slots[num];
        if (selectedSlot == num)
            num = -1;

        selectedSlot = num;

        seq = DOTween.Sequence();

        for (int i = 0; i < 5; i++)
        {
            if (i == num)
            {
                seq.Join(slots[i].GetComponent<RectTransform>().DOSizeDelta(new Vector2(120, 210), 0.5f))
                                .Join(slots[i].transform.Find("Amount").GetComponent<RectTransform>().DOAnchorPosY(85f, 0.5f));

                if (slots[i].transform.childCount > 1)
                    seq.Join(slots[i].transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosY(45f, 0.5f));
            }
            else
            {
                seq.Join(slots[i].GetComponent<RectTransform>().DOSizeDelta(new Vector2(120, 120), 0.5f))
                                .Join(slots[i].transform.Find("Amount").GetComponent<RectTransform>().DOAnchorPosY(35f, 0.5f));

                if (slots[i].transform.childCount > 1)
                    seq.Join(slots[i].transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosY(0f, 0.5f));
            }
        }
    }

    public void UseQuickSlot()
    {
        if (QuickSlotManager.Instance.curSelectingPortion == null || QuickSlotManager.Instance.curSelectingPortion.assignedItem == null) return;

        QuickSlotManager.Instance.curSelectingPortion.UseItem();
        for (int i = 0; i < 5; i++)
        {
            if(QuickSlotManager.Instance.curSelectingPortion == slots[i])
                InventoryManager.Instance.PlayerInventory.quickSlot[i].UseItem();
        }
    }

    public void EnableQuickSlot()
    {
        if(tween!= null && tween.active == true)
            tween.Kill();

        tween = rect.DOAnchorPosY(-200f, 0.5f);
    }

    public void DisableQuickSlot()
    {
        if (tween != null && tween.active == true)
            tween.Kill();

        tween = rect.DOAnchorPosY(0f, 0.5f);
    }
}
