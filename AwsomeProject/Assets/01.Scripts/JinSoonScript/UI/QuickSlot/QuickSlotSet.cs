using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 얘는 3가지 상태가 있다고 생각하면 됨
/// 
/// 활성화 되있는 상태, 비활성화 상태, 할당되 있는 아이템 정보만 있는 상태
/// 마지막 상태는 이 스크립트로 구현도 안되있을 거임 생각하기 편하라고 이렇게 표현한거임
/// 
/// 미리 QuickSlotSet 3개를 세팅해둔다고 하면
/// 
/// 1번째는 활성화 상태로 사용할 준비가 되어있고
/// 2번째는 비활성화 상태로 활성화 상태의 퀵슬롯을 다쓰면 얘로 넘어옴
/// 3번째는 어떤 아이템이 할당되어 있나만 가지고 있다가 1번을 다쓰면 2번이 활성화상태가 되고 
/// 3번이 비활성화 상태가 되서 인게임에서 오브젝트가 만들어지게 됨
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

    public void RemoveItem(int idx)
    {
        slots[idx].DeleteItem();
    }

    private void SelectOneSlot(int num)
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

    private void UseQuickSlot()
    {
        if (selectedSlot == -1) return;

        if (slots[selectedSlot].AssignedPortion != null)
        {
            slots[selectedSlot].UseItem();
            QuickSlotManager.Instance.RemoveItem(0, selectedSlot, true);
        }

        bool isSlotEmpty = true;

        for (int i = 0; i < 5; i++)
        {
            if (slots[i].AssignedPortion != null)
            {
                isSlotEmpty = false;
                break;
            }
        }

        if (isSlotEmpty)
        {
            QuickSlotManager.Instance.MoveToNextQuickSlot();
            //quickSlotSetsParent.GotoNextQuickSlotSet();
        }
    }

    private void UnSelectAllSlot()
    {
        selectedSlot = -1;

        for (int i = 0; i < 5; i++)
        {
            slots[i].DisableSlot();
        }
    }

    #region QuickSlotAnimation

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

    #endregion

    #region QuickSlotInput

    private void SetQuickSlotInput()
    {
        player.PlayerInput.SelectQuickSlot += SelectOneSlot;
        player.PlayerInput.OnUseQuickSlot += UseQuickSlot;
        player.PlayerInput.SelectMysteryPortion += UnSelectAllSlot;
    }
    private void UnSetQuickSlotInput()
    {
        player.PlayerInput.SelectQuickSlot -= SelectOneSlot;
        player.PlayerInput.OnUseQuickSlot -= UseQuickSlot;
        player.PlayerInput.SelectMysteryPortion -= UnSelectAllSlot;
    }

    #endregion

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