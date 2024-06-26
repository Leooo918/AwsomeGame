using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class QuickSlotManager : Singleton<QuickSlotManager>
{
    public int maxQuickSlotCnt = 3;

    //�����ͻ� �����ϴ� �����Կ� �Ҵ�� �ִ� �����۵�
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
        //������ �� ������ �� ������ �� ������ ���ְ� ������ ��ĭ�� ����
        //�� ������ ���� �������
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