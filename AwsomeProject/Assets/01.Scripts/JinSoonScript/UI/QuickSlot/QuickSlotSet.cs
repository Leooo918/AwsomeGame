using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// ��� 3���� ���°� �ִٰ� �����ϸ� ��
/// 
/// Ȱ��ȭ ���ִ� ����, ��Ȱ��ȭ ����, �Ҵ�� �ִ� ������ ������ �ִ� ����
/// ������ ���´� �� ��ũ��Ʈ�� ������ �ȵ����� ���� �����ϱ� ���϶�� �̷��� ǥ���Ѱ���
/// 
/// �̸� QuickSlotSet 3���� �����صдٰ� �ϸ�
/// 
/// 1��°�� Ȱ��ȭ ���·� ����� �غ� �Ǿ��ְ�
/// 2��°�� ��Ȱ��ȭ ���·� Ȱ��ȭ ������ �������� �پ��� ��� �Ѿ��
/// 3��°�� � �������� �Ҵ�Ǿ� �ֳ��� ������ �ִٰ� 1���� �پ��� 2���� Ȱ��ȭ���°� �ǰ� 
/// 3���� ��Ȱ��ȭ ���°� �Ǽ� �ΰ��ӿ��� ������Ʈ�� ��������� ��
/// </summary>
public class QuickSlotSet : MonoBehaviour
{
    [SerializeField] private AnimationCurve disableAnimCurve;

    public int slotNum = 0;

    private readonly QuickSlotVisual[] slots = new QuickSlotVisual[5];
    private QuickSlotSetsParent quickSlotSetsParent;

    private RectTransform quickSlotSetRect;
    private Image quickSlotSetImage;
    private Player player;
    private Shadow shadow;

    public bool isEnable = false;
    private int selectedSlot = -1;

    private Sequence seq;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
            slots[i] = transform.GetChild(i).GetComponent<QuickSlotVisual>();

        quickSlotSetsParent = transform.parent.GetComponent<QuickSlotSetsParent>();
        quickSlotSetRect = GetComponent<RectTransform>();
        quickSlotSetImage = GetComponent<Image>();
        shadow = GetComponent<Shadow>();

        player = PlayerManager.Instance.Player;
    }

    private void OnDisable()
    {
        UnSetQuickSlotInput();
    }

    public void SetItem(ItemSO item, int index)
    {
        slots[index].SetItem(item);
    }

    private void UseQuickSlot()
    {
        if (selectedSlot == -1) return;

        if (slots[selectedSlot].AssignedPortion != null)
        {
            slots[selectedSlot].UseItem();
            QuickSlotManager.Instance.UseItem(selectedSlot);
        }

        bool isSlotEmpty = true;

        for (int i = 0; i < 5; i++)
        {
            if (slots[i].AssignedPortion != null)
                isSlotEmpty = false;
        }

        if (isSlotEmpty) quickSlotSetsParent.GotoNextQuickSlotSet();
    }

    public void SelectOneSlot(int num)
    {
        if (selectedSlot == num)
            num = -1;

        selectedSlot = num;

        for (int i = 0; i < 5; i++)
        {
            if (i == num)
                slots[i].EnableSlot();
            else
                slots[i].DisableSlot();
        }
    }

    public void EnableQuickSlotSet(QuickSlotOffset offset)
    {
        SetQuickSlotInput();

        float tweeningTime = 0.5f;

        if (seq != null && seq.active)
            seq.Kill();

        seq = DOTween.Sequence();

        seq.Append(quickSlotSetRect.DOScale(offset.scale, tweeningTime).SetEase(Ease.Linear))
            .Join(quickSlotSetRect.DOAnchorPos(offset.position, tweeningTime))
            .Join(quickSlotSetImage.DOColor(offset.color, tweeningTime));
    }

    public void DisableQuickSlotSet()
    {
        UnSetQuickSlotInput();

        float tweeningTime = 0.5f;
        if (seq != null && seq.active)
            seq.Kill();

        seq = DOTween.Sequence();

        seq.Append(quickSlotSetRect.DOAnchorPosY(-680f, tweeningTime).SetEase(disableAnimCurve))
            .Join(quickSlotSetRect.DOAnchorPosX(Random.Range(-25f, 25f), tweeningTime))
            .OnComplete(() => Destroy(gameObject));
    }

    private void SetQuickSlotInput()
    {
        player.PlayerInput.FirstQuickSlot += () => SelectOneSlot(0);
        player.PlayerInput.SecondQuickSlot += () => SelectOneSlot(1);
        player.PlayerInput.ThirdQuickSlot += () => SelectOneSlot(2);
        player.PlayerInput.ForthQuickSlot += () => SelectOneSlot(3);
        player.PlayerInput.FifthQuickSlot += () => SelectOneSlot(4);

        player.PlayerInput.OnUseQuickSlot += UseQuickSlot;
    }

    private void UnSetQuickSlotInput()
    {
        player.PlayerInput.FirstQuickSlot -= () => SelectOneSlot(0);
        player.PlayerInput.SecondQuickSlot -= () => SelectOneSlot(1);
        player.PlayerInput.ThirdQuickSlot -= () => SelectOneSlot(2);
        player.PlayerInput.ForthQuickSlot -= () => SelectOneSlot(3);
        player.PlayerInput.FifthQuickSlot -= () => SelectOneSlot(4);

        player.PlayerInput.OnUseQuickSlot -= UseQuickSlot;
    }

    public void Init(QuickSlotItems items, bool isEnable, int slotNum)
    {
        if (isEnable)
            SetQuickSlotInput();

        this.slotNum = slotNum;
        this.isEnable = isEnable;
        shadow.enabled = isEnable;

        for (int i = 0; i < 5; i++)
            slots[i].SetItem(items.items[i]);
    }
}