using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MysteryPortionInventory : Inventory
{
    private InventorySlot[] _slots;
    [SerializeField] private int _slotCnt = 3;
    [SerializeField] private PortionItemSO[] _mysteryPortions;

    [SerializeField] private GameObject _slotPf;
    [SerializeField] private Transform _slotParent;

    private int _openedMysteryPortionCnt = 1;
    private string _path;

    protected override  void Awake()
    {
        _slots = new InventorySlot[_slotCnt];

        for (int i = 0; i < _slotCnt; i++)
        {
            _slots[i] = Instantiate(_slotPf, _slotParent).GetComponent<InventorySlot>();
            _slots[i].transform.SetSiblingIndex(i);
            _slots[i].Init(this);
        }
    }

    protected override void Start()
    {
        StartCoroutine("DelaySetItem");
    }

    protected override void OnDisable() { }

    protected override void OnEnable() { }

    public void UnlockMysteryPortion(int portionCnt)
    {
        if (_openedMysteryPortionCnt < portionCnt) _openedMysteryPortionCnt = portionCnt;

        Item item = InventoryManager.Instance.MakeItemInstanceByItemSO(_mysteryPortions[portionCnt - 1]);
        item.GetComponent<Image>().raycastTarget = false;
        _slots[portionCnt - 1].InsertItem(item);
    }

    public override void UnSelectAllSlot()
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i].UnSelect();
        }
    }

    private IEnumerator DelaySetItem()
    {
        yield return null;
        yield return null;
        yield return null;

        for (int i = 1; i <= _openedMysteryPortionCnt; i++)
            UnlockMysteryPortion(i);
    }
}