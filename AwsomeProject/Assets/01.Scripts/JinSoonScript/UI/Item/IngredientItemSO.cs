using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Mushroom,
    Grass,
    Fruit
}

[CreateAssetMenu(menuName = "SO/IngredientSO")]
public class IngredientItemSO : ItemSO
{
    public IngredientType ingredientType;
}
