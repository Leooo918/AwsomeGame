using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : Singleton<QuickSlotManager>
{
    public int maxQuickSlotCnt = 3;
    [SerializeField] private QuickSlotSetsParent quickSlotSetsParent;
    [SerializeField] private QuickSlotInserterSetsParent quickSlotInserterSetsParent;

    //데이터상 존재하는 퀵슬롯에 할당되 있는 아이템들
    [HideInInspector] public QuickSlotItems[] quickSlots;

    public QuickSlotSetsParent QuickSlotSetsParent => quickSlotSetsParent;
    public QuickSlotInserterSetsParent QuickSlotInserterSetsParent => quickSlotInserterSetsParent;

    public int MaxQuickSlotCnt
    {
        get
        {
            return maxQuickSlotCnt;
        }
        set
        {
            maxQuickSlotCnt = value;
            QuickSlotSetsParent.maxQuickSlotCnt = value;
            QuickSlotInserterSetsParent.maxQuickSlotCnt = value;
        }
    }

    private void Awake()
    {
        MaxQuickSlotCnt = maxQuickSlotCnt;

        quickSlots = new QuickSlotItems[maxQuickSlotCnt];

        for (int i = 0; i < maxQuickSlotCnt; i++)
            quickSlots[i] = new QuickSlotItems();

        QuickSlotSetsParent.InitQuickSlotSet(quickSlots[0], quickSlots[1]);
        QuickSlotInserterSetsParent.Init();
    }

    public QuickSlotItems GetNextQuickSlot()
    {
        //퀵슬롯 맨 앞줄을 다 썼으니 맨 앞줄을 없애고 앞으로 한칸씩 보내
        //맨 뒷줄은 새로 만들어줘
        QuickSlotItems[] temp = quickSlots;

        for(int i = 1; i < temp.Length; i++)
            quickSlots[i - 1] = temp[i];

        quickSlots[quickSlots.Length - 1] = new QuickSlotItems();

        return quickSlots[1];
    }

    public QuickSlotItems GetCurQuickSlot() => quickSlots[0];

    public void UseItem(int selectedSlot)
    {
        quickSlots[0].items[selectedSlot] = null;
    }
}

public class QuickSlotItems
{
    public ItemSO[] items = new ItemSO[5];
}