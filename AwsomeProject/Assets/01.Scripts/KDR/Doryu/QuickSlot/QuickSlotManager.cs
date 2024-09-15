using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : Singleton<QuickSlotManager>
{
    [SerializeField] private Transform _passiveLine;
    [SerializeField] private Transform _activeLine;
    [SerializeField] private Inventory _activeSlotInven;
    [SerializeField] private Inventory _passoveSlotInven;

    [field: SerializeField] public Sprite[] slotOutLines { get; private set; }
    [field: SerializeField] public Sprite slotNoneItemOutLine { get; private set; }

    private int _currentSelectIdx = -1;
    private List<QuickSlot> _passiveQuickSlots = new List<QuickSlot>();
    private List<QuickSlot> _activeLineQuickSlots = new List<QuickSlot>();

    public ThrowPotion throwPotion;

    private KeyCode[] _alphaNums =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
    };

    private void Awake()
    {
        _passiveLine.GetComponentsInChildren(_passiveQuickSlots);
        _activeLine.GetComponentsInChildren(_activeLineQuickSlots);

        _activeSlotInven.OnInventoryModified += HandleActiveInventoryModified;
        _passoveSlotInven.OnInventoryModified += HandlePassiveInventoryModified;
    }

    private void Update()
    {
        for (int i = 0; i < _alphaNums.Length; i++)
        {
            if (Input.GetKeyDown(_alphaNums[i]))
            {
                if (_currentSelectIdx == i)
                    SelectQuickSlot(-1);
                else
                    SelectQuickSlot(i);
            }
        }
    }

    public QuickSlot GetSelectedPotionSlot()
    {
        if (_currentSelectIdx == -1) return null;
        return _activeLineQuickSlots[_currentSelectIdx];
    }

    private void SelectQuickSlot(int index)
    {
        if (_currentSelectIdx != -1)
            _activeLineQuickSlots[_currentSelectIdx].OnSelect(false);
        _currentSelectIdx = index;
        if (_currentSelectIdx != -1)
            _activeLineQuickSlots[_currentSelectIdx].OnSelect(true);
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
            _activeLineQuickSlots[i].SetPotion(slots[i, 0]);
        }
    }
}
