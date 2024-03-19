using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Recipe")]
public class RecipeSO : ScriptableObject
{
    public int id;
    public IngredientItemSO[] ingredients = new IngredientItemSO[2];
    public PosionItemSO posion;
}
