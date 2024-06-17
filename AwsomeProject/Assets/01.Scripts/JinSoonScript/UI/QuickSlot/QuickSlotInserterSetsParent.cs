using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotInserterSetsParent : MonoBehaviour
{
    public int maxQuickSlotCnt = 0;
    public GameObject quickSlotSetPf;

    public QuickSlotOffset quickSlotOffset;

    private TextMeshProUGUI curSlotNumTxt;

    private QuickSlotInserterSet curQuickSlot;
    private QuickSlotInserterSet nextQuickSlot;
    private int curSlotNum = 0;

    private Vector2 originPos = new Vector2(0f, 385f);
    private Vector2 downPeek = new Vector2(40f, 270f);
    private Vector2 upPeek = new Vector2(-40f, 500f);
    private Color disableColor = new Color(0.8f, 0.8f, 0.8f, 1f);

    private void Awake()
    {
        curSlotNumTxt = transform.Find("CurrentSlotNum").GetComponent<TextMeshProUGUI>();
    }

    public void GoSlotNumUp()
    {
        curSlotNum++;

        if (curSlotNum >= maxQuickSlotCnt)
            curSlotNum = 0;

        curSlotNumTxt.SetText($"{curSlotNum + 1}");

        QuickSlotItems items = QuickSlotManager.Instance.quickSlots[curSlotNum];
        nextQuickSlot.Init(items);
        curQuickSlot.GoDisable(upPeek, originPos, disableColor);
        nextQuickSlot.GoEnable(originPos, Color.white);

        QuickSlotInserterSet temp = curQuickSlot;
        curQuickSlot = nextQuickSlot;
        nextQuickSlot = temp;
    }

    public void GoSlotNumDown()
    {
        curSlotNum--;

        if (curSlotNum < 0)
            curSlotNum = maxQuickSlotCnt - 1;

        curSlotNumTxt.SetText($"{curSlotNum + 1}");

        QuickSlotItems items = QuickSlotManager.Instance.quickSlots[curSlotNum];
        nextQuickSlot.Init(items);
        curQuickSlot.GoDisable(downPeek, originPos, disableColor);
        nextQuickSlot.GoEnable(originPos, Color.white);

        QuickSlotInserterSet temp = curQuickSlot;
        curQuickSlot = nextQuickSlot;
        nextQuickSlot = temp;
    }

    public void Init()
    {
        nextQuickSlot = GetQuickSlot();
        curQuickSlot = GetQuickSlot();
    }

    public QuickSlotInserterSet GetQuickSlot()
    {
        QuickSlotInserterSet quickSlotSet =
            Instantiate(quickSlotSetPf, transform).GetComponent<QuickSlotInserterSet>();
        QuickSlotItems items = QuickSlotManager.Instance.quickSlots[curSlotNum];
        quickSlotSet.Init(items);

        RectTransform rect = quickSlotSet.GetComponent<RectTransform>();
        Image img = rect.GetComponent<Image>();

        rect.anchoredPosition = quickSlotOffset.position;
        rect.localScale = quickSlotOffset.scale;
        img.color = quickSlotOffset.color;

        return quickSlotSet;
    }
}
