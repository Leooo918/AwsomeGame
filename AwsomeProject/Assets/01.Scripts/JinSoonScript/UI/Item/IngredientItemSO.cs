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
    public List<GoodCompatibilityItem> goodCompatibilityItems = new List<GoodCompatibilityItem>();
    public List<BadCompatibilityItem> badCompatibilityItems = new List<BadCompatibilityItem>();

    [HideInInspector] public IngredientType ingredientType;
    [HideInInspector] public int gatheringTime;

    private void Awake()
    {
        itemType = ItemType.Ingredient;
    }
}

public struct GoodCompatibilityItem
{
    public int itemId;
    public float effectMultiple;
}

public struct BadCompatibilityItem
{
    public int itemId;
}