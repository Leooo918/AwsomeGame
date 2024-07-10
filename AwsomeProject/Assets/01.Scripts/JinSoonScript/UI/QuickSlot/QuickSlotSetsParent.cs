using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class QuickSlotSetsParent : MonoBehaviour
{
    public GameObject quickSlotSetPf;

    //���� ��밡���� ������
    public QuickSlotSet currentQuickSlotSet;
    //���� ������
    public QuickSlotSet nextQuickSlotSet;
    public MysteryPortionIndicator portionIndicator;

    public QuickSlotOffset enabledOffset;
    public QuickSlotOffset disabledOffset;

    public int maxQuickSlotCnt = 3;
    private int nextIndex = 2;

    private Sequence seq;
    private Coroutine coroutine;

    private void Start()
    {
        QuickSlotItems firstSet = QuickSlotManager.Instance.GetQuickSlot(0);
        QuickSlotItems secondSet = QuickSlotManager.Instance.GetQuickSlot(1);
        InitQuickSlotSet(firstSet, secondSet);
    }

    public void SetItem(ItemSO item, int slotIdx, int selectedSlot)
    {
        //���� �����ϴ� �����Լ�Ʈ�� �������� �־�ٸ� �������� ������ ���̰�
        if (currentQuickSlotSet != null && currentQuickSlotSet.slotNum == slotIdx)
            currentQuickSlotSet.SetItem(item, selectedSlot);

        if (nextQuickSlotSet != null && nextQuickSlotSet.slotNum == slotIdx)
            nextQuickSlotSet.SetItem(item, selectedSlot);
    }

    public void RemoveItem(int slotIdx, int selectedSlot)
    {
        if (currentQuickSlotSet != null && currentQuickSlotSet.slotNum == slotIdx)
            currentQuickSlotSet.RemoveItem(selectedSlot);

        if (nextQuickSlotSet != null && nextQuickSlotSet.slotNum == slotIdx)
            nextQuickSlotSet.RemoveItem(selectedSlot);
    }

    /// <summary>
    /// ���� �������� �ΰ��ӿ� �ν��Ͻ�ȭ����
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
    /// ���� �������� ��������
    /// </summary>
    /// <param name="quickSlotSet"></param>
    public void SetNextQuickSlotSet(QuickSlotItems quickSlotSet, int slotNum)
    {
        float tweeningTime = 0.5f;
        nextQuickSlotSet = Instantiate(quickSlotSetPf, transform).GetComponent<QuickSlotSet>();
        nextQuickSlotSet.Init(quickSlotSet, false, slotNum);

        RectTransform rect = nextQuickSlotSet.GetComponent<RectTransform>();
        Image img = nextQuickSlotSet.GetComponent<Image>();
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
    /// �������� ������ ���� �Ҹ��ؼ� ���� �������� �Ѿ�Ե�
    /// </summary>
    public void GotoNextQuickSlotSet()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(GoNextQuickSlotRoutine());
    }

    /// <summary>
    /// When Use All Quickslot and move to next quickSlots
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoNextQuickSlotRoutine()
    {
        //currentQuickSlotSet�� ������!
        currentQuickSlotSet.DisableQuickSlotSet();
        //���� ������ ���� �������� ��ü
        currentQuickSlotSet = nextQuickSlotSet;
        yield return new WaitForSeconds(0.4f);

        currentQuickSlotSet.slotNum = 0;
        currentQuickSlotSet.EnableQuickSlotSet(enabledOffset);
        nextQuickSlotSet = null;
        yield return new WaitForSeconds(0.5f);


        if (nextIndex >= maxQuickSlotCnt) yield break;

        QuickSlotItems items = QuickSlotManager.Instance.quickSlots[1];
        SetNextQuickSlotSet(items, 1);
    }

    /// <summary>
    /// Initialize QuickSlot
    /// SetQuickSlots, currentSlot, nextQuickSlotSet
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
