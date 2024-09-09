using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Pot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private PotSlot[] _slots;
    [SerializeField] private PotionRecipeListSO _potionRecipeListSO;
    [SerializeField] private PotionTypeSlider _potionTypeSlider;

    private void Update()
    {
        if (_slots[0].assignedItem != null && _slots[1].assignedItem != null)
        {
            _slots[2].gameObject.SetActive(true);
        }
        else
        {
            _slots[2].ReturnItem();
            _slots[2].gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftPotion();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.05f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    public void ReturnItem()
    {
        for (int i = 0; i < 3; i++)
        {
            _slots[i].ReturnItem();
        }
    }
    public void ClearItem()
    {
        for (int i = 0; i < 3; i++)
        {
            _slots[i].ClearItem();
        }
    }

    private void CraftPotion()
    {
        PotionItemSO potionSO = _potionRecipeListSO.GetPotion(_slots, _potionTypeSlider.GetPotionType());
        if (potionSO == null) return;


        Item item = Instantiate(InventoryManager.Instance.itemPrefab);
        item.Init();
        item.itemSO = potionSO;
        item.amount = 1;

        bool succes = InventoryManager.Instance.TryAddItem(item);
        if (succes)
        {
            ItemGatherPanel gather = UIManager.Instance.GetUI(UIType.ItemGather) as ItemGatherPanel;
            gather.Init(item.itemSO);
            gather.Open();

            ClearItem();
        }
        else
        {
            Destroy(item.gameObject);
        }
    }

    private void OnDisable()
    {
        ClearItem();
    }
}
