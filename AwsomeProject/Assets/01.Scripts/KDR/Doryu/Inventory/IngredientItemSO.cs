using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Doryu/Item/IngredientItem")]
public class IngredientItemSO : ItemSO
{
    [Space(20)]
    [Header("Gathering")]
    public float gatheringTime;
}