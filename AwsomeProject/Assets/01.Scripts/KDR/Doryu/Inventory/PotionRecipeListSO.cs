using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Doryu/PotionRecipeList")]
public class PotionRecipeListSO : ScriptableObject
{
    public PotionRecipeSO[] potionRecipes;

    private Dictionary<int, PotionItemSO> PotionRecipeDict = 
        new Dictionary<int, PotionItemSO>();

    public void OnEnable()
    {
        for (int i = 0; i < potionRecipes.Length; i++)
        {
            potionRecipes[i].Init();
            PotionRecipeDict.Add(potionRecipes[i].needIngredientValue, potionRecipes[i].potion);
        }
    }

    public PotionItemSO GetPotion(PotSlot[] ingredientItems, PotionType potionType)
    {
        int sum = (int)potionType;
        for (int i = 0; i < ingredientItems.Length; i++)
        {
            if (ingredientItems[i].assignedItem != null)
            {
                sum *= ingredientItems[i].assignedItem.itemSO.GetItemTypeNumber();
            }
        }
        if (PotionRecipeDict.ContainsKey(sum))
            return PotionRecipeDict[sum];
        else
            return null;
    }
}
