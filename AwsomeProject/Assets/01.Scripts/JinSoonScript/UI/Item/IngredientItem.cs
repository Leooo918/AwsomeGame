using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientItem : Item
{
    private void Awake()
    {
        itemType = ItemType.Ingredient;
    }
}
