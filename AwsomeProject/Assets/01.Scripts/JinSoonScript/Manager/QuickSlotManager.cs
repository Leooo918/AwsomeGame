using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : Singleton<QuickSlotManager>
{
    public int maxQuickSlotCnt = 3;
    [SerializeField] private QuickSlotSetsParent quickSlotSetsParent;
    [SerializeField] private QuickSlotInserterSetsParent quickSlotInserterSetsParent;

    //�����ͻ� �����ϴ� �����Կ� �Ҵ�� �ִ� �����۵�
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
        //������ �� ������ �� ������ �� ������ ���ְ� ������ ��ĭ�� ����
        //�� ������ ���� �������
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