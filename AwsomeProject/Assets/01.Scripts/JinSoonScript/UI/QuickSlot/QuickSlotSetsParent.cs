using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotSetsParent : MonoBehaviour
{
    public GameObject quickSlotSetPf;

    //���� ��밡���� ������
    [HideInInspector] public QuickSlotSet currentQuickSlotSet;
    //���� ������
    [HideInInspector] public QuickSlotSet nextSlot;

    public QuickSlotOffset enabledOffset;
    public QuickSlotOffset disabledOffset;

    public int maxQuickSlotCnt = 3;
    private int nextIndex = 2;

    private Sequence seq;
    private Coroutine coroutine;

    public void SetItem(ItemSO item, int slotNum, int slotIdx)
    {
        //���� �����ϴ� �����Լ�Ʈ�� �������� �־�ٸ� �������� ������ ���̰�
        if (currentQuickSlotSet != null && currentQuickSlotSet.slotNum == slotNum)
            currentQuickSlotSet.SetItem(item, slotIdx);

        if (nextSlot != null && nextSlot.slotNum == slotNum)
            nextSlot.SetItem(item, slotIdx);
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
    /// �������� ������ ���� �Ҹ��ؼ� ���� �������� �Ѿ�Ե�
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


        //��� null�� ���� ���� ����
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
