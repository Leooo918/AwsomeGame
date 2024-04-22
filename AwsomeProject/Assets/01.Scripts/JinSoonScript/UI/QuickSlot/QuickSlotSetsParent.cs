using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotSetsParent : MonoBehaviour
{
    public GameObject quickSlotSetPf;

    //현재 사용가능한 퀵슬롯
    [HideInInspector] public QuickSlotSet currentQuickSlotSet;
    //다음 퀵슬롯
    [HideInInspector] public QuickSlotSet nextSlot;

    public QuickSlotOffset enabledOffset;
    public QuickSlotOffset disabledOffset;

    public int maxQuickSlotCnt = 3;
    private int nextIndex = 2;

    private Sequence seq;
    private Coroutine coroutine;

    public void SetItem(ItemSO item, int slotNum, int slotIdx)
    {
        //현재 존재하는 퀵슬롯세트에 아이템을 넣어다면 아이템이 넣을게 보이게
        if (currentQuickSlotSet != null && currentQuickSlotSet.slotNum == slotNum)
            currentQuickSlotSet.SetItem(item, slotIdx);

        if (nextSlot != null && nextSlot.slotNum == slotNum)
            nextSlot.SetItem(item, slotIdx);
    }

    /// <summary>
    /// 현재 퀵슬롯을 인게임에 인스턴스화해줘
    /// </summary>
    /// <param name="quickSlotSet"></param>
    public void SetCurrentQuickSlotSet(QuickSlotItems quickSlotSet, int slotNum)
    {
        currentQuickSlotSet = Instantiate(quickSlotSetPf, transform).GetComponent<QuickSlotSet>();
        currentQuickSlotSet.Init(quickSlotSet, true, slotNum);

        RectTransform rect = currentQuickSlotSet.GetComponent<RectTransform>();
        Image img = currentQuickSlotSet.GetComponent<Image>();
        currentQuickSlotSet.transform.SetAsFirstSibling();

        rect.anchoredPosition = enabledOffset.position;
        rect.localScale = enabledOffset.scale;
        img.color = enabledOffset.color;
    }

    /// <summary>
    /// 다음 퀵슬롯을 세팅해줌
    /// </summary>
    /// <param name="quickSlotSet"></param>
    public void SetNextQuickSlotSet(QuickSlotItems quickSlotSet, int slotNum)
    {
        float tweeningTime = 0.5f;
        nextSlot = Instantiate(quickSlotSetPf, transform).GetComponent<QuickSlotSet>();
        nextSlot.Init(quickSlotSet, false, slotNum);

        RectTransform rect = nextSlot.GetComponent<RectTransform>();
        Image img = nextSlot.GetComponent<Image>();
        currentQuickSlotSet.transform.SetAsLastSibling();

        Color color = disabledOffset.color;
        color.a = 0;
        img.color = color;
        rect.anchoredPosition = disabledOffset.position - (enabledOffset.scale - disabledOffset.scale);
        rect.localScale = disabledOffset.scale - (enabledOffset.scale - disabledOffset.scale);

        if (seq != null && seq.active)
            seq.Kill();

        seq = DOTween.Sequence();

        seq.Append(rect.DOScale(disabledOffset.scale, tweeningTime).SetEase(Ease.Linear))
            .Join(rect.DOAnchorPos(disabledOffset.position, tweeningTime))
            .Join(img.DOColor(disabledOffset.color, tweeningTime));

    }

    /// <summary>
    /// 퀵슬롯의 포션을 전부 소모해서 다음 슬롯으로 넘어가게됨
    /// </summary>
    public void GotoNextQuickSlotSet()
    {
        if (coroutine != null) StopCoroutine(coroutine);

        coroutine = StartCoroutine(GoNextQuickSlotRoutine());
    }

    /// <summary>
    /// When Use All Quickslot and moveto next quickSlots
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoNextQuickSlotRoutine()
    {
        currentQuickSlotSet.DisableQuickSlotSet();
        currentQuickSlotSet = nextSlot;
        yield return new WaitForSeconds(0.5f);


        //얘는 null이 나올 수도 있음
        if (nextSlot != null)
        {
            nextSlot.EnableQuickSlotSet(enabledOffset);
            nextSlot = null;
            yield return new WaitForSeconds(0.5f);
        }

        if (nextIndex >= maxQuickSlotCnt) yield break;

        QuickSlotItems items = QuickSlotManager.Instance.GetNextQuickSlot();
        SetNextQuickSlotSet(items, nextIndex);
    }

    /// <summary>
    /// Initialize QuickSlot
    /// SetQuickSlots, currentSlot, nextSlot
    /// </summary>
    public void InitQuickSlotSet(QuickSlotItems firstSlot, QuickSlotItems secondSlot)
    {
        SetCurrentQuickSlotSet(firstSlot, 0);
        SetNextQuickSlotSet(secondSlot, 1);

        nextIndex = 2;
    }
}

[System.Serializable]
public struct QuickSlotOffset
{
    public Vector3 position;
    public Vector3 scale;
    public Color color;
}
