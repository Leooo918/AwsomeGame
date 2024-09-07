using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotBelt : MonoBehaviour
{
    [SerializeField] private Transform _passiveLine;
    [SerializeField] private Transform _activeLine;
    [SerializeField] private Inventory _activeSlotInven;
    [SerializeField] private Inventory _passoveSlotInven;

    public List<QuickSlot> passiveQuickSlots = new List<QuickSlot>();
    public List<QuickSlot> activeLineQuickSlots = new List<QuickSlot>();

    private void Awake()
    {
        _passiveLine.GetComponentsInChildren(passiveQuickSlots);
        _activeLine.GetComponentsInChildren(activeLineQuickSlots);

        _activeSlotInven.OnInventoryModified += HandleActiveInventoryModified;
        _passoveSlotInven.OnInventoryModified += HandlePassiveInventoryModified;
    }

    private void HandlePassiveInventoryModified(InventorySlot[,] slots)
    {
        //for (int i = 0; i < 3; i++)
        //{
        //       activeLineQuickSlots[i].SetPotion(slots[i, 0]);
        //}
    }
    private void HandleActiveInventoryModified(InventorySlot[,] slots)
    {
        for (int i = 0; i < 5; i++)
        {
            activeLineQuickSlots[i].SetPotion(slots[i, 0]);
        }
    }
}
