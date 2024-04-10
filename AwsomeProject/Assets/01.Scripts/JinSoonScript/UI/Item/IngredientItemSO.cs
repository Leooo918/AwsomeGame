using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Mushroom,
    Grass,
    Fruit,
    Booty
}

[CreateAssetMenu(menuName = "SO/Item/IngredientSO")]
public class IngredientItemSO : ItemSO
{
    [Space(20)]
    [HideInInspector] public IngredientType ingredientType;
    [HideInInspector] public int gatheringTime;

    private void Awake()
    {
        itemType = ItemType.Ingredient;
    }
}
