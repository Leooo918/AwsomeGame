using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class QuickSlotManager : Singleton<QuickSlotManager>
{
    public int maxQuickSlotCnt = 3;

    //데이터상 존재하는 퀵슬롯에 할당되 있는 아이템들
    [HideInInspector] public QuickSlotItems[] quickSlots;
    public QuickSlotInserterSetsParent quickSlotInserter;
    public QuickSlotSetsParent quickSlot;

    public int MaxQuickSlotCnt
    {
        get
        {
            return maxQuickSlotCnt;
        }
        set
        {
            maxQuickSlotCnt = value;
        }
    }

    private void Awake()
    {
        MaxQuickSlotCnt = maxQuickSlotCnt;
        quickSlots = new QuickSlotItems[maxQuickSlotCnt];

        for (int i = 0; i < maxQuickSlotCnt; i++)
            quickSlots[i] = new QuickSlotItems(i);
    }

    public QuickSlotItems GetNextQuickSlot()
    {
        //퀵슬롯 맨 앞줄을 다 썼으니 맨 앞줄을 없애고 앞으로 한칸씩 보내
        //맨 뒷줄은 새로 만들어줘
        QuickSlotItems[] temp = quickSlots;

        for (int i = 1; i < temp.Length; i++)
            quickSlots[i - 1] = temp[i];

        quickSlots[quickSlots.Length - 1] = new QuickSlotItems(quickSlots.Length - 1);

        return quickSlots[0];
    }

    public QuickSlotItems GetQuickSlot(int idx) => quickSlots[idx];


    public void RemoveItem(int slotIdx, int selectedSlot)
    {
         quickSlots[slotIdx].items[selectedSlot] = null;
        quickSlot.RemoveItem(slotIdx, selectedSlot);
    }

    public void InsertItem(int slotIdx, int selectedSlot, ItemSO item)
    {
        quickSlots[slotIdx].items[selectedSlot] = item;
        quickSlot.SetItem(item, slotIdx, selectedSlot);
    }
}

public class QuickSlotItems
{
    public int itemIdx;
    public ItemSO[] items = new ItemSO[5];

    public QuickSlotItems(int idx)
    {
        itemIdx = idx;
        items = new ItemSO[5];
    }
}