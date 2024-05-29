using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionTable : MonoBehaviour
{
    private PortionCraftingIngredientSlot[] ingredientsSlot = new PortionCraftingIngredientSlot[5];

    private void Awake()
    {
        for (int i = 0; i < ingredientsSlot.Length; i++)
        {
            ingredientsSlot[i] = transform.GetChild(i).GetComponent<PortionCraftingIngredientSlot>();
        }
    }

    public void MakePortion()
    {
        List<EffectInfo> effects = new List<EffectInfo>();

        for (int i = 0; i < 5; i++)
        {
            if (ingredientsSlot[i].assignedItem != null)
            {
                IngredientItemSO item = ingredientsSlot[i].assignedItem.itemSO as IngredientItemSO;
                EffectInfo effect = new EffectInfo();
                effect.effect = item.effectType;
                effect.requirePoint = item.effectPoint;

                effects.Add(effect);
            }
        }

        PortionManager.Instance.portionSet.FindMakeablePortion(effects.ToArray(), out PortionItemSO portion);
        Item itemInstance = InventoryManager.Instance.MakeItemInstanceByItemSO(portion);

        if (InventoryManager.Instance.PlayerInventory.TryInsertItem(itemInstance) == false)
        {
            Debug.Log("인벤토리 자리없는데숭");
        }
        else
        {
            List<IngredientItemSO> ingredients = new List<IngredientItemSO>();
            for (int i = 0; i < 5; i++)
            {
                if (ingredientsSlot[i].assignedItem != null)
                {
                    ingredients.Add(ingredientsSlot[i].assignedItem.itemSO as IngredientItemSO);
                    Destroy(ingredientsSlot[i].assignedItem.gameObject);
                }
            }

            RecipeManager.Instance.AddRecipe(ingredients.ToArray(), portion);
        }
    }
}
