using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientItem : Item
{
    protected override void Awake()
    {
        base.Awake();
        itemType = ItemType.Ingredient;
    }
}
