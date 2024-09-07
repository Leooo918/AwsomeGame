using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Doryu/Item/ItemListSO")]
public class ItemListSO : ScriptableObject
{
    public IngredientItemSO[] ingredientItemSOList;
    public PotionItemSO[] throwPotionItemSOList;
    public PotionItemSO[] drinkPotionItemSOList;
}