using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionTable : MonoBehaviour
{
    private IngredientItemSO[] ingredients  = new IngredientItemSO[5];

    public void MakePortion()
    {
    }

    public void AddItem(int number, IngredientItemSO ingredient)
    {
        ingredients[number] = ingredient;
    }

    public void RemoveItem(int number)
    {
        ingredients[number] = null;
    }
}
