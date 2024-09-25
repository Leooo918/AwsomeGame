using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encyclopedia : MonoBehaviour
{
    [SerializeField] private EncyPotionRecipe _encyPotionRecipe;
    [SerializeField] private EncySlot[] _potionSlots;

    [SerializeField] private PotionRecipeListSO _potionRecipeListSO;
    [SerializeField] private ItemListSO _itemListSO;

    private void Awake()
    {
        _encyPotionRecipe.SetData();

        for (int i = 0; i < _potionSlots.Length; i++)
        {
            _potionSlots[i].Init(i, _potionRecipeListSO, this);
        }
    }

    public void ShowPotionData(PotionItemSO potionItemSO)
    {
        _encyPotionRecipe.SetData(_potionRecipeListSO, _itemListSO, potionItemSO);
    }
}
