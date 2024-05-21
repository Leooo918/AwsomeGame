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
     public IngredientType ingredientType;
     public int gatheringTime;

    public EffectEnum effectType;
    public int effectPoint;

    private void OnEnable()
    {
        itemType = ItemType.Ingredient;
    }
}